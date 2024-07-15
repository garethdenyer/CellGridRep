using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        chemoattractants = new List<string>() { "Diluent", "fMLP", "PMA", "LPS", "C5a", "LTB<sub>4</sub>" };
        rightWellDPDN.AddOptions(chemoattractants);
        leftWellDPDN.AddOptions(chemoattractants);

        samples = new List<string>() { "Control", "Patient A", "Patient B", "Patient C" };
        sampleDPDN.AddOptions(samples);
    }

    public void SetChemoattractant(string dpdnname)
    {
        if (dpdnname == "right")
        {
            chemwells[0].GetComponent<ChemotaxisWell>().chemicalIndex = rightWellDPDN.value;
        }
        else
        {
            chemwells[1].GetComponent<ChemotaxisWell>().chemicalIndex = leftWellDPDN.value;
        }
    }

    public void ToggleWellQuads()
    {

        Renderer quadrenderer = centralWell.GetComponentInChildren<Renderer>();
        if (quadrenderer.enabled)
        {
            quadrenderer.enabled = false;
        }
        else
        {
            quadrenderer.enabled = true;
        }

    }

    public void StartPopulatingWell()
    {
        spread++;
        ClearCells();
        spawnSampleButton.interactable = false;
        clearCellsButton.interactable = true;

        int cellNumber = Random.Range(2500, 3000);

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

        //make a list for totting up the cell type totals and populate with five zeros
        List<int> typetotals = new List<int>();
        for (int c = 0; c < 5; c++)
        {
            typetotals.Add(0);
        }

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

                //randomise the rotation but mainly in the z, only a little in x and y
                //apply rotation to the cell
                Vector3 rotn = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-180f, 180f));
                cell.transform.rotation = Quaternion.Euler(rotn);

                //generate a cell type in proportion to what is desired.
                float choicefactor = Random.Range(0f, 100f);
                if (choicefactor > 10f)  //90% will be F, H, I, or J
                {
                    switch (sampleDPDN.value)
                    {
                        case 0:
                            cell.GetComponent<ChemotaxisCell>().cellType = 'F';
                            typetotals[0] += 1;
                            break;
                        case 1:
                            cell.GetComponent<ChemotaxisCell>().cellType = 'H';
                            typetotals[2] += 1;
                            break;
                        case 2:
                            cell.GetComponent<ChemotaxisCell>().cellType = 'I';
                            typetotals[3] += 1;
                            break;
                        case 3:
                            cell.GetComponent<ChemotaxisCell>().cellType = 'J';
                            typetotals[4] += 1;
                            break;
                        default:
                            cell.GetComponent<ChemotaxisCell>().cellType = 'F';
                            typetotals[0] += 1;
                            break;
                    }
                }
                else //10% will be G
                {
                    cell.GetComponent<ChemotaxisCell>().cellType = 'G';
                    typetotals[1] += 1;
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

        //Debug.Log("F " + typetotals[0] + ", G " + typetotals[1] + ", H " + typetotals[2] + ", I " + typetotals[3] + ", J " + typetotals[4]);

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

        //Debug.Log("Move Cells started on " + cells.Count.ToString() + " cells");
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
        float magicspeeddivisor = 20f;

        float basespeed = this.GetComponent<MainTimer>().timedistort / magicspeeddivisor;  //take the timedistort and tweak with a magic number

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
                        float randsp = Random.Range(-0.05f, 0.05f);
                        //Debug.Log($"Cell: {cell.name}, Well: {chemwells[w].name}, ChemIndex: {chemindex}, Speed: {speed}");
                        speed += (speed * randsp);
                        //if (speed > 0)
                        //{
                            cell.transform.position = Vector3.MoveTowards(cell.transform.position, chemwells[w].transform.position, speed * Time.deltaTime);
                        //}
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
