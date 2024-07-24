using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemotaxisCellSelect : MonoBehaviour
{
    //attached to chemotaxis cell

    Counter counterscript;
    public string selection;
    public GameObject userselect;

    private void Start()
    {
        counterscript = FindObjectOfType<Counter>();
    }

    private void OnMouseDown()
    {

            if (selection == "selected")
            {
                selection = "";  //deselect
                userselect.SetActive(false);  //hide tick
                counterscript.cellsCounted--;
            }

            else
            {
                selection = "selected";  //select
                userselect.SetActive(true); //show tick
                counterscript.cellsCounted++;
            }    
    }
}
