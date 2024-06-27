using UnityEngine;

public class TagCells : MonoBehaviour
{
    //attached to sphere part of cell (not the top level)

    public GameObject toplevelcell;  //not necessary, could be done by GetComponentInParent
    CountCells cellcountscript;
    Counter counterscript;

    private void Start()
    {
        cellcountscript = FindObjectOfType<CountCells>();
        counterscript = FindObjectOfType<Counter>();
    }

    private void OnMouseDown()
    {
        if (counterscript.autoCount)  //only if couter set to autocounting - nothing happens on manual
        {
            if (toplevelcell.GetComponent<CellProps>().selection == "selected")
            {
                toplevelcell.GetComponent<CellProps>().selection = "";  //deselect
                toplevelcell.GetComponent<CellProps>().userselect.SetActive(false);  //hide tick
                cellcountscript.userSelected.Remove(toplevelcell); //remove from list
            }

            else
            {
                toplevelcell.GetComponent<CellProps>().selection = "selected";  //select
                toplevelcell.GetComponent<CellProps>().userselect.SetActive(true); //show tick
                cellcountscript.userSelected.Add(toplevelcell); //add to list
            }

            counterscript.SetUserCounterDigits(cellcountscript.userSelected.Count);  //update the counter
        }
    }
}
