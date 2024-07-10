using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;


public class ChemotaxisManager : MonoBehaviour
{
    // Attached to EmptyHolder in Chemotaxis Scene

    int spread;
    bool IsPopulatingWell;
    public GameObject cellPrefab;
    public GameObject centralWell;

    public float spawnRadius = 0.4f; // Radius within which to spawn cells

    public List<GameObject> cells = new List<GameObject>();

    public List<GameObject> chemwells = new List<GameObject>();

    public int cellsPerFrame = 20; // Number of cells to move per frame

    public Slider basicspeed;

    public Button spawnSampleButton;
    public Button clearCellsButton;
    public Button startMoveButton;

    public List<string> chemoattractants = new List<string>();
    public TMP_Dropdown rightWellDPDN;
    public TMP_Dropdown leftWellDPDN;

    public List<string> samples = new List<string>();
    public TMP_Dropdown sampleDPDN;

    private Coroutine moveCellsCoroutine;


    private void Start()
    {
        spawnSampleButton.onClick.AddListener(StartPopulatingWell);
        clearCellsButton.onClick.AddListener(ClearCells);
        spawnSampleButton.interactable = true;
        clearCellsButton.interactable = false;
        startMoveButton.interactable = false;

        chemoattractants = new List<string>() { "Diluent", "fMLP", "PMA", "LPS", "C5a", "LTB4" };
        rightWellDPDN.AddOptions(chemoattractants);
        leftWellDPDN.AddOptions(chemoattractants);

        samples = new List<string>() {"Control", "Patient A", "Patient B", "Patient C" };
        sampleDPDN.AddOptions(samples);
    }

    public void SetChemoattractant (string dpdnname)
    {
        if(dpdnname == "right")  
        {
            chemwells[0].GetComponent<ChemotaxisWell>().chemicalIndex = rightWellDPDN.value;
        }
        else
        {
            chemwells[1].GetComponent<ChemotaxisWell>().chemicalIndex = leftWellDPDN.value;
        }
    }


    public void StartPopulatingWell()
    {
        spread++;
        ClearCells();
        spawnSampleButton.interactable = false;
        clearCellsButton.interactable = true;

        int cellNumber = Random.Range(1500, 2000);

        if (!IsPopulatingWell)
        {
            StartCoroutine(PopulateWell(cellNumber));
        }
    }

    public void ClearCells()
    {
        if (moveCellsCoroutine != null)
        {
            StopCoroutine(moveCellsCoroutine);
            moveCellsCoroutine = null;
        }

        foreach (GameObject cell in cells)
        {
            Destroy(cell);
        }
        cells.Clear();

        spawnSampleButton.interactable = true;
        clearCellsButton.interactable = false;
        sampleDPDN.interactable = true;
        rightWellDPDN.interactable = true;
        leftWellDPDN.interactable = true;

        this.GetComponent<MainTimer>().ResetTimer();
        startMoveButton.interactable = false;
    }

    IEnumerator PopulateWell(int cellstospawn)
    {
        IsPopulatingWell = true;

        float gridx = 0.4f;
        float gridz = 0.4f;

        for (int i = 0; i < cellstospawn; i++)
        {
            float y = 0.05f; //depth in well
            Vector3 startingposition = new Vector3(Random.Range(-gridx, gridx), y, Random.Range(-gridz, gridz)); //position within a square
            //check within radius
            float radius = Mathf.Sqrt((startingposition.x * startingposition.x) + (startingposition.z * startingposition.z));

            if (radius < 0.4f)
            {
                GameObject cell = Instantiate(cellPrefab);
                cell.transform.name = "C" + cells.Count.ToString();
                cell.transform.SetParent(centralWell.transform);
                cell.transform.localPosition = startingposition;



                //generate a cell type in proportion to what is desired.
                float choicefactor = Random.Range(0f, 100f);
                if (choicefactor > 10f)  //90% will be G, H, I, or J
                {
                    switch (sampleDPDN.value)
                    {
                        case 0:
                            cell.GetComponent<ChemotaxisCell>().cellType = 'G';
                            break;
                        case 1:
                            cell.GetComponent<ChemotaxisCell>().cellType = 'H';
                            break;
                        case 2:
                            cell.GetComponent<ChemotaxisCell>().cellType = 'I';
                            break;
                        case 3:
                            cell.GetComponent<ChemotaxisCell>().cellType = 'J';
                            break;
                        default:
                            cell.GetComponent<ChemotaxisCell>().cellType = 'G';
                            break;                           
                    }                   
                }
                else //10% will be F
                {
                    cell.GetComponent<ChemotaxisCell>().cellType = 'F';
                }
          
                cell.GetComponent<ChemotaxisCell>().SetChemoattractantAffinities();

                cells.Add(cell);

            }

            // Yield every N iterations to avoid blocking the frame for too long
            if (i % 50 == 0)
            {
                yield return null; // Yield to the next frame
            }
        }

        //end of coroutine
        IsPopulatingWell = false;

        startMoveButton.interactable = true;
        sampleDPDN.interactable = false;
    }

    public void BeginCellMovement()
    {
        //first check that the cooroutine isn't already going...
        if (moveCellsCoroutine != null)
        {
            StopCoroutine(moveCellsCoroutine);
        }
        
        //start the coroutine and keep a referncec to it
        moveCellsCoroutine = StartCoroutine(MoveCells());

        Debug.Log("Move Cells started on " + cells.Count.ToString() + " cells");
        this.GetComponent<MainTimer>().timerIsRunning = true;
        startMoveButton.interactable = false;
        rightWellDPDN.interactable = false;
        leftWellDPDN.interactable = false;
    }

    private IEnumerator MoveCells()
    {
        while (true)
        {
            // Move a subset of cells each frame
            for (int i = 0; i < cells.Count; i += cellsPerFrame)
            {
                
                for (int j = 0; j < cellsPerFrame && (i + j) < cells.Count; j++)
                {
                    MoveCell(cells[i + j]);
                }
                yield return null;
            }
        }
    }

    private void MoveCell(GameObject cell)
    {
        float basespeed = this.GetComponent<MainTimer>().timedistort/100f;  //take the timedistort and tweak with a magic number

        ChemotaxisCell cellScript = cell.GetComponent<ChemotaxisCell>();

        // Check if cellScript is not null and has affinities
        if (cellScript != null && cellScript.chemoattractantAffinities != null && cellScript.chemoattractantAffinities.Count > 0)
        {
            for (int w = 0; w < chemwells.Count; w++)
            {
                ChemotaxisWell wellScript = chemwells[w].GetComponent<ChemotaxisWell>();
                if (wellScript != null)
                {
                    int chemindex = wellScript.chemicalIndex;

                    // Check if chemindex is within the bounds of chemoattractantAffinities list
                    if (chemindex >= 0 && chemindex < cellScript.chemoattractantAffinities.Count)
                    {
                        float speed = cellScript.GetAffinityToChemoattractant(chemindex) * basespeed;
                        //Debug.Log($"Cell: {cell.name}, Well: {chemwells[w].name}, ChemIndex: {chemindex}, Speed: {speed}");

                        if (speed > 0)
                        {
                            cell.transform.position = Vector3.MoveTowards(cell.transform.position, chemwells[w].transform.position, speed * Time.deltaTime);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Invalid chemindex: {chemindex} for cell: {cell.name}");
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning($"CellScript is null or affinities are not properly set for cell: {cell.name}");
        }
    }
}
