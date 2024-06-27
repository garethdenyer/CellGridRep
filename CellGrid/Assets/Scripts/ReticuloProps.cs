using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticuloProps : MonoBehaviour
{
    // Attached to Reticulo cell

    public GameObject appearancequad;

    CreateRBCRets mainscript;
    Counter counterscript;

    public string selection;
    public GameObject userselect;

    private void Start()
    {
        mainscript = FindObjectOfType<CreateRBCRets>();
        counterscript = FindObjectOfType<Counter>();
    }



    private void OnMouseDown()
    {
        if (counterscript.autoCount)  //only if couter set to autocounting - nothing happens on manual
        {
            if (selection == "selected")
            {
                selection = "";  //deselect
                userselect.SetActive(false);  //hide tick
                mainscript.selectedCells.Remove(gameObject); //remove from list
            }

            else
            {
                selection = "selected";  //select
                userselect.SetActive(true); //show tick
                mainscript.selectedCells.Add(gameObject); //add to list
            }

            counterscript.SetUserCounterDigits(mainscript.selectedCells.Count);  //update the counter
        }
    }

}
