using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Counter : MonoBehaviour
{
    // Attached to Empty Holder (could be attached to Counter)

    public TMP_Text usercounter1s;
    public TMP_Text usercounter10s;
    public TMP_Text usercounter100s;

    public int cellsCounted;

    public bool autoCount;
    public GameObject manualPanel;

    CreateRBCRets createScript; //in case we will use RBCRets

    private void Start()
    {
        cellsCounted = 0;
        SetUserCounterDigits(cellsCounted);
        autoCount = true;

        // Check if the RBCRets sscript is attached to a GameObject in the scene
        createScript = FindObjectOfType<CreateRBCRets>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            cellsCounted++;
            SetUserCounterDigits(cellsCounted);
        }

        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            cellsCounted--;
            SetUserCounterDigits(cellsCounted);
        }
    }

    public void ShowHideManualPanel(string mode)
    {
        if (mode == "Manual")
        {
            autoCount = false;
            manualPanel.SetActive(true);
        }
        else
        {
            autoCount = true;
            manualPanel.SetActive(false);
        }

        //if we are dealing with Rets then use teh REsetUsercounts in the main script
        if (createScript != null)
        {
            createScript.ResetUserCounts();

        }
        else //otherwise use the reset user counts in CountCells
        {
            this.GetComponent<CountCells>().ResetUserCounts();
        }    
    }

    public void ManualChange(string direction)
    {
        if(direction == "plus")
        {
            cellsCounted += 1;
        }
        else
        {
            if(cellsCounted > 0)
            {
              cellsCounted -= 1;
            }
        }
   
        SetUserCounterDigits(cellsCounted);
    }

    public void SetUserCounterDigits(int userselcount) //takes an input and displays it on the counter wheels
    {
        float hundreds = Mathf.Floor(userselcount / 100);
        float tenspart = userselcount - (hundreds * 100);
        float tens = Mathf.Floor(tenspart / 10);
        float ones = userselcount - (hundreds * 100) - (tens * 10);

        usercounter100s.text = hundreds.ToString();
        usercounter10s.text = tens.ToString();
        usercounter1s.text = ones.ToString();
    }

}
