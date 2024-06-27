using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
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

    public List<GameObject> cells = new List<GameObject>();

    public GameObject rightWell;

    ChemotaxisWell rightwellscript;
    public int cellsPerFrame = 20; // Number of cells to move per frame

    public Slider basicspeed;
    

    private void Start()
    {
        rightwellscript = rightWell.GetComponent<ChemotaxisWell>();
    }


    public void StartPopulatingWell()
    {
        spread++;

        //get rid of all cells, clear cells list and wipe cellinGrid
        foreach (GameObject celltokill in cells)
        {
            Destroy(celltokill);
        }
        cells.Clear();

        //decide on cellNumber
        int cellNumber = Random.Range(1500, 2000);


        if (!IsPopulatingWell)
        {
            StartCoroutine(PopulateWell(cellNumber));
        }
    }

    IEnumerator PopulateWell(int cellstospawn)
    {
        IsPopulatingWell = true;

        float gridx = 0.4f;
        float gridz = 0.4f;

        for (int i = 0; i < cellstospawn; i++)
        {
            float y = 0.05f; //depth in well
            Vector3 startingposition = new Vector3(Random.Range(-gridx, gridx),  y, Random.Range(-gridz, gridz)); //position within a square
            //check within radius
            float radius = Mathf.Sqrt((startingposition.x * startingposition.x) + (startingposition.z * startingposition.z));

            if (radius < 0.4f)
            {
                GameObject cell = Instantiate(cellPrefab);
                cell.transform.SetParent(centralWell.transform);           
                cell.transform.localPosition = startingposition;
                
                //decide on cell mobility
                float mob = MakeRandFromNormal(0.2f); //the range is 0-1, so the std dev is set at 0.2
                cell.GetComponent<ChemotaxisCell>().mobility = mob;

                cells.Add(cell);  // This is where you add the instantiated cell to your list
                cell.transform.name = "C" + cells.Count.ToString();
            }

            // Yield every N iterations to avoid blocking the frame for too long
            if (i % 50 == 0)
            {
                yield return null; // Yield to the next frame
            }
        }

        //end of coroutine
        IsPopulatingWell = false;
    }

    public float MakeRandFromNormal(float stdDev)
    {
        float result; //value to return
        float mean = 0.5f;
        //now for the black magic
        float u1 = Random.Range(0.0f, 1.0f);
        float u2 = Random.Range(0.0f, 1.0f);
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        result = mean + stdDev * randStdNormal;
        if (result < 0.02f) //if the random value was in bottom 2%, make it a negative so we have some cells that go backwards
        {
            result *= -1f;
        }
        return result;
    }

    public void BeginCellMovement()
    {
        StartCoroutine(MoveCells());
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
        float basespeed = basicspeed.value;
        ChemotaxisCell cellScript = cell.GetComponent<ChemotaxisCell>();
        if (cellScript != null && rightwellscript != null)
        {
            float speed = cellScript.mobility * rightwellscript.attractiveness * basespeed;
            cell.transform.position = Vector3.MoveTowards(cell.transform.position, rightWell.transform.position, speed * Time.deltaTime);
        }
    }

}
