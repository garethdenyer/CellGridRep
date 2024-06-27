using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpreadCells : MonoBehaviour
{
    // Attached to Empty Holder

    public GameObject cellPrefab;
    public GameObject dirtPrefab;
    public string fractionchoice;
    public TMP_Text fractionchoiceinfo;

    public Material[] dirtMats;

    public int cellNumber;
    public float dilnFactor;
    public float cellSuspVol;

    public List<float> contamin = new List<float>();

    public float deathRate;

    public List<GameObject> cells = new List<GameObject>();
    public List<GameObject> dirtbits = new List<GameObject>();
    public List<GameObject> validcells = new List<GameObject>();

    public GameObject extrabuttonsPanel;
    public GameObject resultsPanel;
    public GameObject counterPanel;

    public GameObject[] sectorQuads;
    bool sectorrotn;
    List<float> sectorsnormal = new List<float>() { 90f, 0f, -90f, 180f, 0f, 90f, 0f, -90f, 180f };
    List<float> sectorsrotated = new List<float>() { 0f, 90f, 180f, 90f, 0f, 0f, -90f, 0f, 90f };


    public TMP_Text prepsummary;

    public Button MNLbutton;
    public Button PMNbutton;

    public int prepquality;

    Camera mainCamera;

    private void Start()
    {
        dilnFactor = 2f;
        deathRate = 50f;
        cellSuspVol = 1f;
        prepsummary.text = "";
        fractionchoice = "MNL";

        extrabuttonsPanel.SetActive(false);
        resultsPanel.SetActive(false);
        counterPanel.SetActive(false);
        HideSectorQuads();

        mainCamera = Camera.main;
    }

    public void SetFractionChoice(string inputchoice)
    {
        fractionchoice = inputchoice;
        if(fractionchoice == "MNL")
        {
            MNLbutton.GetComponent<Image>().color = Color.green;
            PMNbutton.GetComponent<Image>().color = Color.white;
        }
        else
        {
            MNLbutton.GetComponent<Image>().color = Color.white;
            PMNbutton.GetComponent<Image>().color = Color.green;
        }
        fractionchoiceinfo.text = "New " + fractionchoice + " sample";
        CleanUpStuff();
    }

    public void SetNewBasicParameters()
    {
        deathRate = Random.Range(0, 20f);
        dilnFactor = 2f;
        cellSuspVol = 1f;
        cellNumber = Random.Range(700, 1500);
        //set the level of contamination
        prepquality = Random.Range(0, 3);
        switch (prepquality)
        {
            case 0:
                contamin = new List<float>() { 5, 10, 20 };  // 5% platelets, 5% impostors, 10% rbc, 80% targets
                break;
            case 1:
                contamin = new List<float>() { 15, 25, 45 };  // 15% platelets, 10% impostors, 20% rbc, 55% targets
                break;
            case 2:
                contamin = new List<float>() { 30, 50, 80 };  // 30% platelets, 20% impostors, 30% rbc, 20% targets
                break;
        }


        //determine cell number  x cells/mL = x/1000 cells/uL = x/10,0000 cells per 0.1 uL
        //aim only to get a sample, not a formally assessable amount
        //float hc_volume = 10000f; //see http://microbiology.ucdavis.edu/privalsky/hemocytometer 
        //cellNumber = (int)((cellConc / dilnFactor) / hc_volume);
    }

    void CleanUpStuff()
    {
        //first clear any userselected cells - cant do after actual cells cleared
        this.GetComponent<CountCells>().ResetUserCounts();

        //cleanup what was there before
        CleanAwayCells();  //this cleans away the actual cells
        CleanDirt();
        this.GetComponent<CountCells>().ResetReportsAndInputs();

        prepsummary.text = "";

        extrabuttonsPanel.SetActive(false);
        resultsPanel.SetActive(false);
        counterPanel.SetActive(false);
        HideSectorQuads();
        this.GetComponent<CountCells>().tablereportPanel.SetActive(false);

        mainCamera.transform.position = new Vector3(0f, 0f, -20.77f);
    }

    public void CreateAndPositionCells(string type)
    {
        //get rid of everthing
        CleanUpStuff();

        //establish parameters
        if (type == "new")  //sets new starting parameters
        {
            SetNewBasicParameters();
        }

        else if (type == "dilute") //goes again but bigger dil fac
        {
            dilnFactor *= 2;
            cellNumber /= 2;
        }

        else if (type == "concentrate")  //goes again but more conc, less vol
        {
            cellSuspVol /= 2f;
            cellNumber *= 2;
            if (cellNumber > 1500)
            {
                cellNumber = 1500;
            }
        }

        //give some info to user
        prepsummary.text = "Cells resupsended" + '\n' + "in " + cellSuspVol.ToString("N1") + " mL. " + '\n' +
          "20 uL suspension" + '\n' + "diluted " + dilnFactor.ToString() + "x" + '\n' + "with trypan blue." + '\n' +
          "15 uL loaded onto slide.";

        //if none of the above we just use the same numbers as before


        //make each cell and resize
        for (int i = 0; i < cellNumber; i++)
        {
            //area bounded by central grid is x -3.6 to 3.6, y -3.6 to 3.6.   negative z brings to front
            //it's a square so easy to vary with a constant.  Make instantiation area a touch bigger than 3.6
            float gridxy = 3.8f;
            Vector3 position = new Vector3(Random.Range(-gridxy, gridxy), Random.Range(-gridxy, gridxy), Random.Range(-0.05f, -0.1f));
            GameObject cell = Instantiate(cellPrefab, position, transform.rotation);

            //randomise the rotation but mainly in the z, only a little in x and y
            //apply rotation to the graphic, not the cell
            Vector3 rotn = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-180f, 180f));
            cell.GetComponent<CellProps>().appearancequad.transform.rotation = Quaternion.Euler(rotn);

            cell.transform.name = "C" + i.ToString();

            //decide on cell type and cell size
            string celltype = "lymphocyte";  //set as default
            float cellscale = 0.2f; //set as default

            //generate a random number
            float cellfactor = Random.Range(0f, 100f);
            //use the random number to refer to contamin list.... if

            if (cellfactor < contamin[0])
            {
                celltype = "platelet";
                cellscale = 0.07f;
            }
            else if (cellfactor < contamin[1])
            {
                if (fractionchoice == "MNL")
                {
                    celltype = "neutrophil";
                    cellscale = 0.25f;
                }
                else
                {
                    celltype = "lymphocyte";
                    cellscale = 0.2f;
                }
            }
            else if (cellfactor < contamin[2])
            {
                celltype = "redbloodcell";
                cellscale = 0.1f;
            }
            else
            {
                if (fractionchoice == "MNL")
                {
                    celltype = "lymphocyte";
                    cellscale = 0.2f;
                }
                else
                {
                    celltype = "neutrophil";
                    cellscale = 0.25f;
                }
                validcells.Add(cell);  //valid cells refers to the total number of target cells put into the whole central grid and just beyond
            }


            cell.transform.localScale = new Vector3(cellscale, cellscale, cellscale);
            cell.GetComponent<CellProps>().celltype = celltype;


            //is the cell alive or dead
            float lifefactor = Random.Range(0f, 100f);
            if (lifefactor < deathRate)
            {
                cell.GetComponent<CellProps>().health = "Dead";
            }

            //modify cell appearance
            cell.GetComponent<CellProps>().ChangeAppearance();

            cells.Add(cell);  //this is all the cells instantiated on this run
        }

        //we could have a break here = no need to do much else until the user starts interacting with the spread

        this.GetComponent<CountCells>().EstablishQuandrant();

        CreateDirt();

        SetSectorQuads();

        //submit the initial parameters
        this.GetComponent<DatabaseConnect>().InitialSubmission();

        //toggle the buttons and panels
        extrabuttonsPanel.SetActive(true);
        resultsPanel.SetActive(true);
        counterPanel.SetActive(true);
        this.GetComponent<CountCells>().enterButton.SetActive(true);
    }

    public void CreateDirt()
    {
        float bitstomake = 1000;
        int nodirtmats = dirtMats.Length;  //how many materials to choose from

        for (int i = 0; i < bitstomake; i++)
        {
            //set position and rotatoin and make
            Vector3 posn = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-0.05f, -0.1f));
            Vector3 rotn = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
            GameObject bitofdirt = Instantiate(dirtPrefab, posn, Quaternion.Euler(rotn));
            bitofdirt.transform.name = "D" + i.ToString();
            dirtbits.Add(bitofdirt);

            //apply a random material
            int randmat = Random.Range(0, nodirtmats);
            bitofdirt.GetComponentInChildren<Renderer>().material = dirtMats[randmat];

            //adjust the size
            float dirtscale = Random.Range(0.1f, 0.3f);
            bitofdirt.transform.localScale = new Vector3(dirtscale, dirtscale, dirtscale);
        }
    }

    public void CleanDirt()
    {
        foreach (GameObject dirtybit in dirtbits)
        {
            Destroy(dirtybit);
        }
        dirtbits.Clear();
    }

    public void CleanAwayCells()
    {
        foreach (GameObject celltokill in cells)
        {
            Destroy(celltokill);
        }
        cells.Clear();
        validcells.Clear();
        this.GetComponent<CountCells>().ClearLists();
        this.GetComponent<CountCells>().ClearInputFields();
        HideSectorQuads();
    }

    void SetSectorQuads()
    {
        //activate the background
        for (int q = 0; q < 9; q++)
        {
            sectorQuads[q].SetActive(true);
            sectorQuads[4].SetActive(false); //not the middle (it's the 5th so 4 in list!)

            //now decide how to rotate each sector
            Vector3 rotn;
            if (sectorrotn)
            {
                rotn = new Vector3(0f, 0f, sectorsrotated[q]);
                sectorrotn = false;
            }
            else
            {
                rotn = new Vector3(0f, 0f, sectorsnormal[q]);
                sectorrotn = true;
            }
            sectorQuads[q].transform.rotation = Quaternion.Euler(rotn);
        }
    }

    void HideSectorQuads()
    {
        for (int q = 0; q < 9; q++)
        {
            sectorQuads[q].SetActive(false);
        }
    }

    public void StopCellMovement() //freezes teh 3D cells and gets rid of their rigidbody
    {
        foreach (GameObject movingcell in cells)
        {
            movingcell.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Destroy(movingcell.GetComponent<Rigidbody>());
        }
    }
}
