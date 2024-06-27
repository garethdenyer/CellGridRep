using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProps : MonoBehaviour
{
    // Attached to Cell Prefab at top level

    public string health;
    public string quadrant;
    public bool incentre;
    public string selection;
    public string celltype;

    public GameObject appearancequad;
    public Renderer appearanceRenderer;
    public Material livecellmat;
    public Material deadcellmat;

    public Material neutrophilmat;
    public Material lymphocytemat;
    public Material plateletmat;
    public Material rbcmat;
    public Material defaultCellmat;

    public GameObject userselect;
    public GameObject marker;
    public Sprite tick;
    public Sprite cross;

    public void ChangeAppearance()
    {
        //decide what type of cell to show
        if(celltype == "lymphocyte")
        {
            appearanceRenderer.material = lymphocytemat;
        }
        else if (celltype == "neutrophil")
        {
            appearanceRenderer.material = neutrophilmat;
        }
        else if (celltype == "redbloodcell")
        {
            appearanceRenderer.material = rbcmat;
        }
        else if (celltype == "platelet")
        {
            appearanceRenderer.material = plateletmat;
        }
        else
        {
            appearanceRenderer.material = defaultCellmat;
        }

        //now if dead, make colour blue
        if (health == "Dead")
        {
            appearanceRenderer.material.color = new Color(0.5f, 0.6f, 0.9f);
        }
        
    }

    public void SetTickCross()
    {
        if (quadrant == "A" || quadrant == "B" || quadrant == "C" || quadrant == "D" || quadrant == "E")
        {
            marker.GetComponent<SpriteRenderer>().sprite = tick;
        }

        else if (incentre)
        {
            marker.GetComponent<SpriteRenderer>().sprite = tick;
            marker.GetComponent<SpriteRenderer>().color = Color.magenta;
        }

        else
        {
            marker.GetComponent<SpriteRenderer>().sprite = cross;
        }
    }
}
