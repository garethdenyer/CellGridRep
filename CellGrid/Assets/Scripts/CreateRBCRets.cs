using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateRBCRets : MonoBehaviour
{
    // On Empty Holder in Reticulocyte Scene


    public GameObject cellPrefab;

    public GameObject counterPanel;

    public int cellNumber;
    public int cellInGrid;
    public bool gender;
    public float reticulopct;
    public int spread;

    public List<GameObject> cells = new List<GameObject>();
    public List<GameObject> selectedCells = new List<GameObject>();

    //Materials for the different cell types
    public Material rbc01;
    public Material rbc02;
    public Material rbc03;
    public Material reticulo01;
    public Material reticulo02;
    public Material reticulo03;

    public TMP_Text prepSummary;

    private bool isInstantiating = false;

    void Start()
    {
        cellNumber = 1000;
        spread = 0;
    }


    // Call this method when the button is pressed to start the instantiation
    public void StartInstantiation()
    {
        spread++;

        //get rid of all cells, clear cells list and wipe cellinGrid
        foreach (GameObject celltokill in cells)
        {
            Destroy(celltokill);
        }
        cells.Clear();
        cellInGrid = 0;

        //decide on gender
        int genderdet = Random.Range(0, 2);
        if (genderdet == 0)
        {
            gender = true;
        }
        else
        {
            gender = false;
        }


        //decide on cellNumber - this could be based on gender
        //1000 gives ~160 cells in grid and 2.36 x 10^12/L which is low end for female
        //3500 gives ~440 cells in grid and 6.55 x 10^12/L which is high end for male
        cellNumber = Random.Range(1000, 3500);

        //decide on clinical condition by setting the proportion of reticulocytes
        reticulopct = Random.Range(0.06f, 6f);

        prepSummary.text = "Sample spreading....";


        if (!isInstantiating)
        {
            StartCoroutine(InstantiateCells());
        }
    }

    IEnumerator InstantiateCells()
    {
        isInstantiating = true;

        float gridx = 40f;
        float gridy = 20f;

        for (int i = 0; i < cellNumber; i++)
        {
            float z = Random.Range(-0.05f, -0.1f);
            Vector3 position = new Vector3(Random.Range(-gridx, gridx), Random.Range(-gridy, gridy), z);
            GameObject cell = Instantiate(cellPrefab, position, transform.rotation);

            if (position.x >= -10 && position.x <= 10 && position.y >= -10 && position.y <= 10)
            {
                cellInGrid += 1;
            }

            //randomise the rotation but mainly in the z, only a little in x and y
            //apply rotation to the graphic, not the cell - this keeps the ticks straight
            Vector3 rotn = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-180f, 180f));
            cell.GetComponent<ReticuloProps>().appearancequad.transform.rotation = Quaternion.Euler(rotn);

            cell.transform.name = "C" + i.ToString();

            //decide on cell size
            float cellscale = 0.6f; //set as default and will apply to all cells for the time being
            cell.transform.localScale = new Vector3(cellscale, cellscale, cellscale);

            //decide on cell type
            float proport = Random.Range(0f, 100f);
            if (proport < reticulopct) //make reticulo
            {
                int randRetic = Random.Range(1, 4); //decide on which picture to use - currently 3 choices
                Material retictouse = reticulo01;
                switch (randRetic)
                {
                    case 1:
                        retictouse = reticulo01;
                        break;
                    case 2:
                        retictouse = reticulo02;
                        break;
                    case 3:
                        retictouse = reticulo03;
                        break;

                }
                cell.GetComponent<ReticuloProps>().appearancequad.GetComponent<Renderer>().material = retictouse;
            }
            else //make a red blood cell
            {
                int randRBC = Random.Range(1, 4); //decide on which picture to use - currently 3 choices
                Material rbctouse = rbc01;
                switch (randRBC)
                {
                    case 1:
                        rbctouse = rbc01;
                        break;
                    case 2:
                        rbctouse = rbc02;
                        break;
                    case 3:
                        rbctouse = rbc03;
                        break;
                }
                cell.GetComponent<ReticuloProps>().appearancequad.GetComponent<Renderer>().material = rbctouse;
            }

            cells.Add(cell);  // This is where you add the instantiated cell to your list



            // Yield every N iterations to avoid blocking the frame for too long
            if (i % 50 == 0)
            {
                yield return null; // Yield to the next frame
            }
        }

        isInstantiating = false;

        //document the rbc
        float rbccount = cellInGrid * 15f / 1000f;

        Debug.Log(ManageLogin.SessionID + " Cell Number = " + cellNumber + " In Grid = " + cellInGrid + " RBC = " +
            rbccount.ToString("N2") + " x 10^12/L" + " Reticulo % = " + reticulopct.ToString("N3"));

        string sex = "";
        if (gender)
        {
            sex = "Female";
        }
        else
        {
            sex = "Male";
        }

        prepSummary.text = ManageLogin.SessionID + '\n' +
            "Spread " + spread.ToString() + '\n' + '\n' +
            "Sample from " + sex + '\n' + '\n' +
            "RBC count = " + '\n' +
            rbccount.ToString("N2") + " x 10<sup>12</sup>/L";


        //submit the initial parameters
        this.GetComponent<DatabaseConnect>().InitialReticSubmission(rbccount, reticulopct, sex);

    }




    public void CreateAndPositionCells()
    {
        spread++;

        //get rid of all cells, clear cells list and wipe cellinGrid
        foreach (GameObject celltokill in cells)
        {
            Destroy(celltokill);
        }
        cells.Clear();
        cellInGrid = 0;

        //decide on gender
        int genderdet = Random.Range(0, 2);
        if(genderdet == 0)
        {
            gender = true;
        }
        else
        {
            gender = false;
        }


        //decide on cellNumber - this could be based on gender
        //1000 gives ~160 cells in grid and 2.36 x 10^12/L which is low end for female
        //3500 gives ~440 cells in grid and 6.55 x 10^12/L which is high end for male
        cellNumber = Random.Range(1000, 3500);

        //decide on clinical condition by setting the proportion of reticulocytes
        float reticulopct = Random.Range(0.06f, 6f);

        //make each cell and resize
        for (int i = 0; i < cellNumber; i++)
        {
            //area for all the cells is  x -40 to 40, y -20 to 20.   negative z brings to front
            float gridx = 40f;
            float gridy = 20f;
            float z = Random.Range(-0.05f, -0.1f);
            Vector3 position = new Vector3(Random.Range(-gridx, gridx), Random.Range(-gridy, gridy), z);
            GameObject cell = Instantiate(cellPrefab, position, transform.rotation);
            if (position.x >= -10 && position.x <= 10 && position.y >= -10 && position.y <= 10)
            {
                cellInGrid +=1;
            }

                //randomise the rotation but mainly in the z, only a little in x and y
                //apply rotation to the graphic, not the cell - this keeps the ticks straight
                Vector3 rotn = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-180f, 180f));
            cell.GetComponent<ReticuloProps>().appearancequad.transform.rotation = Quaternion.Euler(rotn);

            cell.transform.name = "C" + i.ToString();

            //decide on cell size
            float cellscale = 0.6f; //set as default and will apply to all cells for the time being
            cell.transform.localScale = new Vector3(cellscale, cellscale, cellscale);

            //decide on cell type
            float proport = Random.Range(0f, 100f);
            if(proport < reticulopct) //make reticulo
            {
                int randRetic = Random.Range(1, 4); //decide on which picture to use - currently 3 choices
                Material retictouse = reticulo01;
                switch (randRetic)
                {
                    case 1:
                        retictouse = reticulo01;
                        break;
                    case 2:
                        retictouse = reticulo02;
                        break;
                    case 3:
                        retictouse = reticulo03;
                        break;

                }
                cell.GetComponent<ReticuloProps>().appearancequad.GetComponent<Renderer>().material = retictouse;
            }
            else //make a red blood cell
            {
                int randRBC = Random.Range(1, 4); //decide on which picture to use - currently 3 choices
                Material rbctouse = rbc01;
                switch (randRBC)
                {
                    case 1:
                        rbctouse = rbc01;
                        break;
                    case 2:
                        rbctouse = rbc02;
                        break;
                    case 3:
                        rbctouse = rbc03;
                        break;
                }
                cell.GetComponent<ReticuloProps>().appearancequad.GetComponent<Renderer>().material = rbctouse;
            }

            cells.Add(cell);  //this is all the cells instantiated on this run
        }

        float rbccount = cellInGrid * 15f/1000f;

        Debug.Log(ManageLogin.SessionID + " Cell Number = " + cellNumber + " In Grid = " + cellInGrid + " RBC = " + 
            rbccount.ToString("N2") + " x 10^12/L" + " Reticulo % = " + reticulopct.ToString("N3"));

        string sex = "";
        if (gender)
        {
            sex = "Female";
        }
        else
        {
            sex = "Male";
        }

        prepSummary.text = ManageLogin.SessionID + '\n' + 
            "Spread " + spread.ToString() +'\n' + '\n' +
            "Sample from " + sex + '\n' + '\n' +
            "RBC count = " + '\n' + 
            rbccount.ToString("N2") + " x 10<sup>12</sup>/L";


        //submit the initial parameters
        this.GetComponent<DatabaseConnect>().InitialReticSubmission(rbccount, reticulopct, sex);
    }

    public void ResetUserCounts()
    {
        //first clean up any tags or ticks on the selectedCells - tag them as unselected and remove any ticks
        if (selectedCells.Count > 0)
        {
            foreach (GameObject selectedcell in selectedCells)
            {
                selectedcell.GetComponent<ReticuloProps>().selection = "";
                selectedcell.GetComponent<ReticuloProps>().userselect.SetActive(false);
            }
        }

        //then expunge the list
        selectedCells.Clear();

        //then expunge any manual count and reset
        counterPanel.GetComponent<Counter>().cellsCounted = 0;
        counterPanel.GetComponent<Counter>().SetUserCounterDigits(0);
    }

}
