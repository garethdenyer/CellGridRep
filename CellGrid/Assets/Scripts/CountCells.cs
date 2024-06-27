using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountCells : MonoBehaviour
{
    // Attached to Empty Holder

    //all the lists for recording quadrant contents
    public List<GameObject> quadAlive = new List<GameObject>();
    public List<GameObject> quadBlive = new List<GameObject>();
    public List<GameObject> quadClive = new List<GameObject>();
    public List<GameObject> quadDlive = new List<GameObject>();
    public List<GameObject> quadElive = new List<GameObject>();

    public List<GameObject> quadAdeads = new List<GameObject>();
    public List<GameObject> quadBdeads = new List<GameObject>();
    public List<GameObject> quadCdeads = new List<GameObject>();
    public List<GameObject> quadDdeads = new List<GameObject>();
    public List<GameObject> quadEdeads = new List<GameObject>();

    public List<GameObject> quadOlist = new List<GameObject>();

    public List<GameObject> wholeCentralGridlive = new List<GameObject>();
    public List<GameObject> wholeCentralGriddead = new List<GameObject>();

    public List<GameObject> userSelected = new List<GameObject>();

    //constants for defining boundaries
    public float ACleft;
    public float ACright;
    public float BDleft;
    public float BDright;
    public float ABtop;
    public float ABbottom;
    public float CDtop;
    public float CDbottom;
    public float Eleft;
    public float Eright;
    public float Etop;
    public float Ebottom;

    //fields for user input and reporting
    //when asking for each of the five minigrids
    public TMP_InputField QuadA_Liveinput;
    public TMP_InputField QuadB_Liveinput;
    public TMP_InputField QuadC_Liveinput;
    public TMP_InputField QuadD_Liveinput;
    public TMP_InputField QuadE_Liveinput;
    public TMP_InputField QuadA_Deadinput;
    public TMP_InputField QuadB_Deadinput;
    public TMP_InputField QuadC_Deadinput;
    public TMP_InputField QuadD_Deadinput;
    public TMP_InputField QuadE_Deadinput;
    public TMP_InputField CellCountinput;

    //when asking for the collated values - based on 5 or 25 minigrids
    public TMP_InputField FiveGridCountLiveInput;
    public TMP_InputField FiveGridCountDeadInput;
    public TMP_InputField FiveGridCountPerMLInput;
    public TMP_InputField FiveGridViabilityInput;
    public TMP_InputField TwentyFiveGridCountLiveInput;
    public TMP_InputField TwentyFiveGridCountDeadInput;
    public TMP_InputField TwentyFiveGridCountPerMLInput;
    public TMP_InputField TwentyFiveGridViabilityInput;

    //for reporting on each quadrant and copyable report
    public GameObject quadReportPrefab;
    public GameObject tablereportPanel;
    public InputField tablereport;

    //for reporting with a non-copyable report
    public TMP_Text reporttext;

    //submission panel
    public GameObject enterButton;
    public GameObject submitcancelpanel;

    //scripts for interacting
    Counter counterscript;
    Camera mainCamera;

    private void Start()
    {
        ACleft = -3.67f;
        ACright = -2.2f;
        BDleft = 2.19f;
        BDright = 3.64f;
        ABtop = 3.67f;
        ABbottom = 2.24f;
        CDtop = -2.17f;
        CDbottom = -3.62f;
        Etop = 0.75f;
        Ebottom = -0.71f;
        Eleft = -0.75f;
        Eright = 0.71f;

        counterscript = FindObjectOfType<Counter>();
        mainCamera = Camera.main;
    }

    public void ResetUserCounts()
    {
        //first clean up the userSelected cells - tag them as unselected and remove any ticks
        if (userSelected.Count > 0)
        {
            foreach (GameObject selectedcell in userSelected)
            {
                selectedcell.GetComponent<CellProps>().selection = "";
                selectedcell.GetComponent<CellProps>().userselect.SetActive(false);
            }
        }

        //then expunge the list
        userSelected.Clear();

        //then expunge any manual count and reset
        counterscript.cellsCounted = 0;
        counterscript.SetUserCounterDigits(0);
    }

    public void EstablishQuandrant()
    {
        if (this.GetComponent<SpreadCells>().cells.Count < 1) { return; }

        foreach (GameObject cell in this.GetComponent<SpreadCells>().validcells)
        {
            if ((cell.transform.position.x > ACleft && cell.transform.position.x < ACright) && (cell.transform.position.y > ABbottom && cell.transform.position.y < ABtop))
            {
                cell.GetComponent<CellProps>().quadrant = "A";
                if (cell.GetComponent<CellProps>().health == "Dead")
                {
                    quadAdeads.Add(cell);
                }
                else
                {
                    quadAlive.Add(cell);
                }
            }

            else if ((cell.transform.position.x > BDleft && cell.transform.position.x < BDright) && (cell.transform.position.y > ABbottom && cell.transform.position.y < ABtop))
            {
                cell.GetComponent<CellProps>().quadrant = "B";
                if (cell.GetComponent<CellProps>().health == "Dead")
                {
                    quadBdeads.Add(cell);
                }
                else
                {
                    quadBlive.Add(cell);
                }
            }

            else if ((cell.transform.position.x > ACleft && cell.transform.position.x < ACright) && (cell.transform.position.y > CDbottom && cell.transform.position.y < CDtop))
            {
                cell.GetComponent<CellProps>().quadrant = "C";
                if (cell.GetComponent<CellProps>().health == "Dead")
                {
                    quadCdeads.Add(cell);
                }
                else
                {
                    quadClive.Add(cell);
                }
            }

            else if ((cell.transform.position.x > BDleft && cell.transform.position.x < BDright) && (cell.transform.position.y > CDbottom && cell.transform.position.y < CDtop))
            {
                cell.GetComponent<CellProps>().quadrant = "D";
                if (cell.GetComponent<CellProps>().health == "Dead")
                {
                    quadDdeads.Add(cell);
                }
                else
                {
                    quadDlive.Add(cell);
                }
            }

            else if ((cell.transform.position.x > Eleft && cell.transform.position.x < Eright) && (cell.transform.position.y > Ebottom && cell.transform.position.y < Etop))
            {
                cell.GetComponent<CellProps>().quadrant = "E";
                if (cell.GetComponent<CellProps>().health == "Dead")
                {
                    quadEdeads.Add(cell);
                }
                else
                {
                    quadElive.Add(cell);
                }
            }

            else  //outs
            {
                cell.GetComponent<CellProps>().quadrant = "Out";
                quadOlist.Add(cell);
            }


            //now the big grid
            if ((cell.transform.position.x > ACleft && cell.transform.position.x < BDright) && (cell.transform.position.y > CDbottom && cell.transform.position.y < ABtop))
            {
                cell.GetComponent<CellProps>().incentre = true;
                if (cell.GetComponent<CellProps>().health == "Dead")
                {
                    wholeCentralGriddead.Add(cell);
                }
                else
                {
                    wholeCentralGridlive.Add(cell);
                }
            }


            cell.GetComponent<CellProps>().SetTickCross();
        }
    }

    public void ClearInputFields()
    {
        FiveGridCountDeadInput.text = "";
        FiveGridCountLiveInput.text = "";
        FiveGridCountPerMLInput.text = "";
        FiveGridViabilityInput.text = "";
        TwentyFiveGridCountDeadInput.text = "";
        TwentyFiveGridCountLiveInput.text = "";
        TwentyFiveGridCountPerMLInput.text = "";
        TwentyFiveGridViabilityInput.text = "";
    }

    public void ClearLists()
    {
        quadAlive = new List<GameObject>();
        quadBlive = new List<GameObject>();
        quadClive = new List<GameObject>();
        quadDlive = new List<GameObject>();
        quadElive = new List<GameObject>();

        quadAdeads = new List<GameObject>();
        quadBdeads = new List<GameObject>();
        quadCdeads = new List<GameObject>();
        quadDdeads = new List<GameObject>();
        quadEdeads = new List<GameObject>();

        quadOlist = new List<GameObject>();

        wholeCentralGridlive = new List<GameObject>();
        wholeCentralGriddead = new List<GameObject>();
    }

    public void RevealInOuts()
    {
        foreach (GameObject validcell in this.GetComponent<SpreadCells>().validcells)
        {
            validcell.GetComponent<CellProps>().marker.SetActive(true);
        }
    }

    public void ResetReportsAndInputs()
    {
        //find the quadreports and delete
        GameObject[] reports = GameObject.FindGameObjectsWithTag("quadreport");
        foreach (GameObject rep in reports)
        {
            Destroy(rep);
        }

        //reset all the inputfields
        QuadA_Liveinput.text = "";
        QuadB_Liveinput.text = "";
        QuadC_Liveinput.text = "";
        QuadD_Liveinput.text = "";
        QuadE_Liveinput.text = "";
        QuadA_Deadinput.text = "";
        QuadB_Deadinput.text = "";
        QuadC_Deadinput.text = "";
        QuadD_Deadinput.text = "";
        QuadE_Deadinput.text = "";
        CellCountinput.text = "";
    }

    public void ProcessEntry()
    {
        //correct any blank fields for submission
        if (QuadA_Liveinput.text == "") { QuadA_Liveinput.text = "NA"; }
        if (QuadB_Liveinput.text == "") { QuadB_Liveinput.text = "NA"; }
        if (QuadC_Liveinput.text == "") { QuadC_Liveinput.text = "NA"; }
        if (QuadD_Liveinput.text == "") { QuadD_Liveinput.text = "NA"; }
        if (QuadE_Liveinput.text == "") { QuadE_Liveinput.text = "NA"; }
        if (QuadA_Deadinput.text == "") { QuadA_Deadinput.text = "NA"; }
        if (QuadB_Deadinput.text == "") { QuadB_Deadinput.text = "NA"; }
        if (QuadC_Deadinput.text == "") { QuadC_Deadinput.text = "NA"; }
        if (QuadD_Deadinput.text == "") { QuadD_Deadinput.text = "NA"; }
        if (QuadE_Deadinput.text == "") { QuadE_Deadinput.text = "NA"; }
        if (CellCountinput.text == "") { CellCountinput.text = "NA"; }

        //ask 'are you sure'
        submitcancelpanel.SetActive(true);
        enterButton.SetActive(false);
    }

    public void ProcessEntry525()
    {
        //correct any blank fields for submission
        if (FiveGridCountLiveInput.text == "") { FiveGridCountLiveInput.text = "NA"; }
        if (FiveGridCountDeadInput.text == "") { FiveGridCountDeadInput.text = "NA"; }
        if (FiveGridCountPerMLInput.text == "") { FiveGridCountPerMLInput.text = "NA"; }
        if (FiveGridViabilityInput.text == "") { FiveGridViabilityInput.text = "NA"; }
        if (TwentyFiveGridCountLiveInput.text == "") { TwentyFiveGridCountLiveInput.text = "NA"; }
        if (TwentyFiveGridCountDeadInput.text == "") { TwentyFiveGridCountDeadInput.text = "NA"; }
        if (TwentyFiveGridCountPerMLInput.text == "") { TwentyFiveGridCountPerMLInput.text = "NA"; }
        if (TwentyFiveGridViabilityInput.text == "") { TwentyFiveGridViabilityInput.text = "NA"; }

        //ask 'are you sure'
        submitcancelpanel.SetActive(true);
        enterButton.SetActive(false);
    }

    public void SubmitEntry()
    {
        this.GetComponent<DatabaseConnect>().UserSubmission525();
        submitcancelpanel.SetActive(false);
        RevealInOuts();
        ShowQuadrantCounts();

        this.GetComponent<SpreadCells>().extrabuttonsPanel.SetActive(false);
        this.GetComponent<SpreadCells>().resultsPanel.SetActive(false);
        this.GetComponent<SpreadCells>().counterPanel.SetActive(false);

        mainCamera.transform.position = new Vector3(0f, 0f, -8.77f);

        tablereportPanel.SetActive(true);
    }

    public void CancelEntry()
    {
        submitcancelpanel.SetActive(false);
        enterButton.SetActive(true);
    }

    public void ShowQuadrantCounts()
    {
        int quadAcount = quadAlive.Count;
        int quadBcount = quadBlive.Count;
        int quadCcount = quadClive.Count;
        int quadDcount = quadDlive.Count;
        int quadEcount = quadElive.Count;
        int quadAdeadcount = quadAdeads.Count;
        int quadBdeadcount = quadBdeads.Count;
        int quadCdeadcount = quadCdeads.Count;
        int quadDdeadcount = quadDdeads.Count;
        int quadEdeadcount = quadEdeads.Count;

        //cells per mL is average of the five minisections * 25 to get to regular quadrant, then * 10 to get to per uL - so *250
        //cells per uL then * dilution factor, then * 1000 to get cells per mL
        //then * suspension volume to get cells in total
        float livePerMl = (((quadAcount + quadBcount + quadCcount + quadDcount + quadEcount) / 5f) * 250f) *
            this.GetComponent<SpreadCells>().dilnFactor * 1000f;
        float deadPerMl = (((quadAdeadcount + quadBdeadcount + quadCdeadcount + quadDdeadcount + quadEcount) / 5f) * 250f) *
            this.GetComponent<SpreadCells>().dilnFactor * 1000f;
        float livePerMl25grid = (wholeCentralGridlive.Count * 10f) *
    this.GetComponent<SpreadCells>().dilnFactor * 1000f;

        //my five lovely quad reports... not used now :-(

        //GameObject QuadArepot = Instantiate(quadReportPrefab, new Vector3(-4.8f, 0.5f, -0.2f), quadReportPrefab.transform.rotation);
        //QuadArepot.GetComponent<QuadReportChars>().UpdateQuadReport("A", quadAcount.ToString() + "-" + quadAdeadcount.ToString());
        //QuadArepot.name = "QuadReportA";

        //GameObject QuadBrepot = Instantiate(quadReportPrefab, new Vector3(4.8f, 0.5f, -0.2f), quadReportPrefab.transform.rotation);
        //QuadBrepot.GetComponent<QuadReportChars>().UpdateQuadReport("B", quadBcount.ToString() + "-" + quadBdeadcount.ToString());
        //QuadBrepot.name = "QuadReportB";

        //GameObject QuadCrepot = Instantiate(quadReportPrefab, new Vector3(-4.8f, -5.35f, -0.2f), quadReportPrefab.transform.rotation);
        //QuadCrepot.GetComponent<QuadReportChars>().UpdateQuadReport("C", quadCcount.ToString() + "-" + quadCdeadcount.ToString());
        //QuadCrepot.name = "QuadReportC";

        //GameObject QuadDrepot = Instantiate(quadReportPrefab, new Vector3(4.8f, -5.35f, -0.2f), quadReportPrefab.transform.rotation);
        //QuadDrepot.GetComponent<QuadReportChars>().UpdateQuadReport("D", quadDcount.ToString() + "-" + quadDdeadcount.ToString());
        //QuadDrepot.name = "QuadReportD";

        //GameObject QuadErepot = Instantiate(quadReportPrefab, new Vector3(-4.8f, -2.4f, -0.2f), quadReportPrefab.transform.rotation);
        //QuadErepot.GetComponent<QuadReportChars>().UpdateQuadReport("E", quadEcount.ToString() + "-" + quadEdeadcount.ToString());
        //QuadErepot.name = "QuadReportE";

        //summary.text = "Live " + livePerMl.ToString("#,#") + '\n' + "Dead " + deadPerMl.ToString("#,#") +
        //    '\n' + " in " + this.GetComponent<SpreadCells>().cellSuspVol.ToString() + " mL";


        string UserQuadA = QuadA_Liveinput.text + "--" + QuadA_Deadinput.text;
        string RealQuadA = quadAcount.ToString() + "--" + quadAdeadcount.ToString();
        string UserQuadB = QuadB_Liveinput.text + "--" + QuadB_Deadinput.text;
        string RealQuadB = quadBcount.ToString() + "--" + quadBdeadcount.ToString();
        string UserQuadC = QuadC_Liveinput.text + "--" + QuadC_Deadinput.text;
        string RealQuadC = quadCcount.ToString() + "--" + quadCdeadcount.ToString();
        string UserQuadD = QuadD_Liveinput.text + "--" + QuadD_Deadinput.text;
        string RealQuadD = quadDcount.ToString() + "--" + quadDdeadcount.ToString();
        string UserQuadE = QuadE_Liveinput.text + "--" + QuadE_Deadinput.text;
        string RealQuadE = quadEcount.ToString() + "--" + quadEdeadcount.ToString();

        //tablereport.text =
        //    "Session  " + ManageLogin.SessionID + '\n' + 
        //    "UserID   " + ManageLogin.Username + '\n' +
        //    "Fraction " + this.GetComponent<SpreadCells>().fractionchoice + '\n' + '\n' +
        //    //this.GetComponent<SpreadCells>().prepsummary.text + '\n' + '\n' +
        //    "Section Counts" + '\n' +
        //    "    " + '\t' + "Software" + '\t' + "Student" + '\n' +
        //    "A   " + '\t' + NineLong(RealQuadA) + '\t' + NineLong(UserQuadA) + '\n' +
        //    "B   " + '\t' + NineLong(RealQuadB) + '\t' + NineLong(UserQuadB) + '\n' +
        //    "C   " + '\t' + NineLong(RealQuadC) + '\t' + NineLong(UserQuadC) + '\n' +
        //    "D   " + '\t' + NineLong(RealQuadD) + '\t' + NineLong(UserQuadD) + '\n' +
        //    "E   " + '\t' + NineLong(RealQuadE) + '\t' + NineLong(UserQuadE) + '\n' + '\n' +

        //    "Cell Counts" + '\n' +
        //    "Software  " + '\n' +
        //    "          " + '\t' + "Live " + '\t' + livePerMl.ToString("N0") + " /mL" + '\n' +
        //    "          " + '\t' + "Dead " + '\t' + deadPerMl.ToString("N0") + " /mL" + '\n' +
        //    "Student  " + '\n' +
        //      "          " + '\t' + CellCountinput.text + " /mL" + '\n'
        //    ;

        //fields for 5 grid reporting
        int softwareNo5sL = quadAcount + quadBcount + quadCcount + quadDcount + quadEcount;
        int softwareNo5sD = quadAdeadcount + quadBdeadcount + quadCdeadcount + quadDdeadcount + quadEdeadcount;
        string softwareNo5sLD = softwareNo5sL.ToString() + " - " + softwareNo5sD.ToString();
        float software5Viability =  100f*((float)softwareNo5sL / ((float)softwareNo5sL + (float)softwareNo5sD));

        string userNo5sL = FiveGridCountLiveInput.text;
        string userNo5sD = FiveGridCountDeadInput.text;
        string userNo5sLD = userNo5sL + " - " + userNo5sD;
        string userNo5sVariability = FiveGridViabilityInput.text;

        //fields for 25 grid reporting
        int softwareNo25sL = wholeCentralGridlive.Count;
        int softwareNo25sD = wholeCentralGriddead.Count;
        string softwareNo25sLD = softwareNo25sL.ToString() + " - " + softwareNo25sD.ToString();
        float software25Viability = 100f * ((float)softwareNo25sL / ((float)softwareNo25sL + (float)softwareNo25sD));

        string userNo25sL = TwentyFiveGridCountLiveInput.text;
        string userNo25sD = TwentyFiveGridCountDeadInput.text;
        string userNo25sLD = userNo25sL + " - " + userNo25sD;
        string userNo25sVariability = TwentyFiveGridViabilityInput.text;

        reporttext.text =
    "<b>Session  </b>" + ManageLogin.SessionID + '\n' +
    "<b>UserID   </b>" + ManageLogin.Username + '\n' +
    "<b>Sample   </b>" + this.GetComponent<SpreadCells>().fractionchoice + '\n' + '\n' +

    "<b><size=130%>5 grid count</b><size=100%>" + '\n' +
    "    " + '\t' + "Software" + '\t' + "Student" + '\n' +
    "    " + '\t' + "LIve-Dead" + '\t' + "Live-Dead" + '\n' +
    "    " + '\t' + NineLong(softwareNo5sLD) + '\t' + NineLong(userNo5sLD) + '\n' + '\n' +
    "    " + '\t' + "Viability (%)" + '\n' +
    "    " + '\t' + "Software" + '\t' + "Student" + '\n' +
    "    " + '\t' + NineLong(software5Viability.ToString("N1"))  + '\t' + NineLong(userNo5sVariability)  + '\n' + '\n' +

    "Live Cell Counts" + '\n' +
    "Software  " + '\t' +  livePerMl.ToString("N0") + " /mL" + '\n' +
    "Student   " + '\t' + FiveGridCountPerMLInput.text + " /mL" + '\n' + '\n' + '\n' +

    "<b><size=130%>25 grid count</b><size=100%>" + '\n' +
    "    " + '\t' + "Software" + '\t' + "Student" + '\n' +
    "    " + '\t' + "LIve-Dead" + '\t' + "Live-Dead" + '\n' +
    "    " + '\t' + NineLong(softwareNo25sLD) + '\t' + NineLong(userNo25sLD) + '\n' + '\n' +
    "    " + '\t' + "Viability (%)" + '\n' +
    "    " + '\t' + "Software" + '\t' + "Student" + '\n' +
    "    " + '\t' + NineLong(software25Viability.ToString("N1")) + '\t' + NineLong(userNo25sVariability) + '\n' + '\n' +

    "Live Cell Counts" + '\n' +
    "Software  " + '\t' + livePerMl25grid.ToString("N0") + " /mL" + '\n' +
    "Student   " + '\t' + TwentyFiveGridCountPerMLInput.text + " /mL" + '\n'


    ;

    }

    string NineLong(string inputtext)
    {
        string output = "";
        int inputlength = inputtext.Length;
        switch (inputlength)
        {
            case 9:
                output = inputtext;
                break;
            case 8:
                output = inputtext + " ";
                break;
            case 7:
                output = inputtext + "  ";
                break;
            case 6:
                output = inputtext + "   ";
                break;
            case 5:
                output = inputtext + "    ";
                break;
            case 4:
                output = inputtext + "     ";
                break;
            case 3:
                output = inputtext + "      ";
                break;
            case 2:
                output = inputtext + "       ";
                break;
            default:
                output = "         ";
                break;
        }

        return output;
    }
}
