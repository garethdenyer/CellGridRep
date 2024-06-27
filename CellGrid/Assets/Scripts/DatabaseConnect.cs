using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseConnect : MonoBehaviour
{
    // On Empty Holder

    string currtime;

    public void InitialSubmission()
    {
        string timeeng = ((Mathf.Round(((Time.time) / 60f) * 10f)) / 10f).ToString() + " m";
        currtime = System.DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

        //values from the spreading
        int cellNo = this.GetComponent<SpreadCells>().cellNumber;
        float deathRate = this.GetComponent<SpreadCells>().deathRate;
        float celldiln = this.GetComponent<SpreadCells>().dilnFactor;
        int contamin = this.GetComponent<SpreadCells>().prepquality;
        string fraction = this.GetComponent<SpreadCells>().fractionchoice;


        //create and send the form
        WWWForm form = new WWWForm();
        form.AddField("sessionPost", ManageLogin.SessionID);
        form.AddField("cellconcPost", cellNo.ToString());
        form.AddField("deadratePost", deathRate.ToString("N1"));
        form.AddField("timedatePost", currtime);
        form.AddField("usernamePost", ManageLogin.Username);
        form.AddField("dilfac_Post", celldiln.ToString());
        form.AddField("fraction_Post", fraction);
        form.AddField("contamination_Post", contamin.ToString());
        form.AddField("timeengaged_Post", timeeng);

        WWW pagetogoto = new WWW("https://labdatagen.com/AddUniSAHaemSession.php", form);
    }

    public void UserSubmission()
    {
        currtime = System.DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

        //values from the spreading
        int cellNo = this.GetComponent<SpreadCells>().cellNumber;
        float deathRate = this.GetComponent<SpreadCells>().deathRate;
        float celldiln = this.GetComponent<SpreadCells>().dilnFactor;

        //the totals of live and deads according to the computer
        int quadAcount = this.GetComponent<CountCells>().quadAlive.Count;
        int quadBcount = this.GetComponent<CountCells>().quadBlive.Count;
        int quadCcount = this.GetComponent<CountCells>().quadClive.Count;
        int quadDcount = this.GetComponent<CountCells>().quadDlive.Count;
        int quadAdeadcount = this.GetComponent<CountCells>().quadAdeads.Count;
        int quadBdeadcount = this.GetComponent<CountCells>().quadBdeads.Count;
        int quadCdeadcount = this.GetComponent<CountCells>().quadCdeads.Count;
        int quadDdeadcount = this.GetComponent<CountCells>().quadDdeads.Count;

        //the usersubmitted values
        string QuadAliveinput = this.GetComponent<CountCells>().QuadA_Liveinput.text;
        string QuadBliveinput = this.GetComponent<CountCells>().QuadB_Liveinput.text;
        string QuadCliveinput = this.GetComponent<CountCells>().QuadC_Liveinput.text;
        string QuadDliveinput = this.GetComponent<CountCells>().QuadD_Liveinput.text;
        string QuadAdeadinput = this.GetComponent<CountCells>().QuadA_Deadinput.text;
        string QuadBdeadinput = this.GetComponent<CountCells>().QuadB_Deadinput.text;
        string QuadCdeadinput = this.GetComponent<CountCells>().QuadC_Deadinput.text;
        string QuadDdeadinput = this.GetComponent<CountCells>().QuadD_Deadinput.text;

        //create and send the form
        WWWForm form = new WWWForm();
        form.AddField("sessionPost", ManageLogin.SessionID);
        form.AddField("cellconcPost", cellNo.ToString());
        form.AddField("deadratePost", deathRate.ToString());
        form.AddField("timedatePost", currtime);
        form.AddField("usernamePost", ManageLogin.Username);
        form.AddField("quadA_Post", QuadAliveinput + "-" + QuadAdeadinput);
        form.AddField("quadB_Post", QuadBliveinput + "-" + QuadBdeadinput);
        form.AddField("quadC_Post", QuadCliveinput + "-" + QuadCdeadinput);
        form.AddField("quadD_Post", QuadDliveinput + "-" + QuadDdeadinput);
        form.AddField("quadAreal_Post", quadAcount.ToString() + "-" + quadAdeadcount.ToString());
        form.AddField("quadBreal_Post", quadBcount.ToString() + "-" + quadBdeadcount.ToString());
        form.AddField("quadCreal_Post", quadCcount.ToString() + "-" + quadCdeadcount.ToString());
        form.AddField("quadDreal_Post", quadDcount.ToString() + "-" + quadDdeadcount.ToString());
        form.AddField("dilfac_Post", celldiln.ToString());

        WWW pagetogoto = new WWW("https://labdatagen.com/AddHaemGuess.php", form);
    }

    public void UserSubmission525()
    {
        string timeeng = ((Mathf.Round(((Time.time) / 60f) * 10f)) / 10f).ToString() + " m";
        currtime = System.DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

        //values from the spreading
        int cellNo = this.GetComponent<SpreadCells>().cellNumber;
        float deathRate = this.GetComponent<SpreadCells>().deathRate;
        float celldiln = this.GetComponent<SpreadCells>().dilnFactor;
        float suspvol = this.GetComponent<SpreadCells>().cellSuspVol;
        int contamin = this.GetComponent<SpreadCells>().prepquality;
        string fraction = this.GetComponent<SpreadCells>().fractionchoice;

        //the totals of live and deads according to the computer
        int computer5countlive = this.GetComponent<CountCells>().quadAlive.Count + 
            this.GetComponent<CountCells>().quadBlive.Count + 
            this.GetComponent<CountCells>().quadClive.Count +
            this.GetComponent<CountCells>().quadDlive.Count +
            this.GetComponent<CountCells>().quadElive.Count;
        int computer5countdead = this.GetComponent<CountCells>().quadAdeads.Count +
            this.GetComponent<CountCells>().quadBdeads.Count +
            this.GetComponent<CountCells>().quadCdeads.Count +
            this.GetComponent<CountCells>().quadDdeads.Count +
            this.GetComponent<CountCells>().quadEdeads.Count;
        int computerperML5s = (int)(((float)computer5countlive) * 5f * 10f * celldiln * 1000f); 
        float computerviability5s = 100 * computer5countlive / (computer5countlive + computer5countdead);
        int full25countlive = this.GetComponent<CountCells>().wholeCentralGridlive.Count;
        int full25countdead = this.GetComponent<CountCells>().wholeCentralGriddead.Count;
        int countperML25s = (int)(((float)this.GetComponent<CountCells>().wholeCentralGridlive.Count) * 10f * celldiln * 1000f); ;
        float viability25s = 100 * this.GetComponent<CountCells>().wholeCentralGridlive.Count / 
            (this.GetComponent<CountCells>().wholeCentralGridlive.Count + this.GetComponent<CountCells>().wholeCentralGriddead.Count);

        //the usersubmitted values
        string User5sCountsInput = this.GetComponent<CountCells>().FiveGridCountLiveInput.text + " - " + this.GetComponent<CountCells>().FiveGridCountDeadInput.text ;
        string User5sCountsPerMlInput= this.GetComponent<CountCells>().FiveGridCountPerMLInput.text;
        string User5sViability = this.GetComponent<CountCells>().FiveGridViabilityInput.text;
        string User25sCountsInput = this.GetComponent<CountCells>().TwentyFiveGridCountLiveInput.text + " - " + this.GetComponent<CountCells>().TwentyFiveGridCountDeadInput.text;
        string User25sCountsPerMlInput = this.GetComponent<CountCells>().TwentyFiveGridCountPerMLInput.text;
        string User25sViability = this.GetComponent<CountCells>().TwentyFiveGridViabilityInput.text;


        //create and send the form
        WWWForm form = new WWWForm();
        form.AddField("sessionPost", ManageLogin.SessionID);
        form.AddField("cellconcPost", cellNo.ToString());
        form.AddField("deadratePost", deathRate.ToString("N1"));
        form.AddField("timedate_Post", currtime);
        form.AddField("username_Post", ManageLogin.Username);
        form.AddField("dilfac_Post", celldiln.ToString());
        form.AddField("contamination_Post", contamin.ToString());
        form.AddField("fraction_Post", fraction.ToString());
        form.AddField("timeengaged_Post", timeeng);


        form.AddField("SFCLD_Post", User5sCountsInput);
        form.AddField("SFCPM_Post", User5sCountsPerMlInput);
        form.AddField("SFV_Post", User5sViability);

        form.AddField("CFCLD_Post", computer5countlive + "-" + computer5countdead);
        form.AddField("CFCPM_Post", computerperML5s);
        form.AddField("CFV_Post", computerviability5s.ToString());

        form.AddField("STCLD_Post", User25sCountsInput);
        form.AddField("STCPM_Post", User25sCountsPerMlInput);
        form.AddField("STV_Post", User25sViability);

        form.AddField("CTCLD_Post", full25countlive + "-" + full25countdead);
        form.AddField("CTCPM_Post", countperML25s.ToString());
        form.AddField("CTV_Post", viability25s.ToString("N1"));

        //for testing
        //form.AddField("SFCLD_Post", "34-8");
        //form.AddField("SFCPM_Post", "3.321245x10^7 cells/mL");
        //form.AddField("SFV_Post", "98.35456%");

        //form.AddField("CFCLD_Post", "36-9");
        //form.AddField("CFCPM_Post", "3,456,213");
        //form.AddField("CFV_Post", "3.4%");

        //form.AddField("STCLD_Post", "235-23");
        //form.AddField("STCPM_Post", "3,000,000");
        //form.AddField("STV_Post", "97.4%");

        //form.AddField("CTCLD_Post", "345-60");
        //form.AddField("CTCPM_Post", "3,456,789");
        //form.AddField("CTV_Post", "4.6%");



        WWW pagetogoto = new WWW("https://labdatagen.com/AddUniSAHaemGuess.php", form);
    }

    public void InitialReticSubmission(float rbccount, float reticpct, string gender)
    {
        string timeeng = ((Mathf.Round(((Time.time) / 60f) * 10f)) / 10f).ToString() + " m";
        currtime = System.DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");

        //values from the spreading
        int spread = this.GetComponent<CreateRBCRets>().spread;

        //create and send the form
        WWWForm form = new WWWForm();
        form.AddField("sessionPost", ManageLogin.SessionID);
        form.AddField("spreadPost", spread.ToString());
        form.AddField("timedatePost", currtime);
        form.AddField("usernamePost", ManageLogin.Username);
        form.AddField("rbccountPost", rbccount.ToString("N2"));
        form.AddField("reticpctPost", reticpct.ToString("N2"));
        form.AddField("genderPost", gender);

        WWW pagetogoto = new WWW("https://labdatagen.com/AddReticSession.php", form);
    }
}
