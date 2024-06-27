using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuadReportChars : MonoBehaviour
{
    public GameObject QuadLetterDisplay;
    public TMP_Text QuadCountDisplay;

    public void UpdateQuadReport(string QuadLetter, string QuadCount)
    {
        QuadLetterDisplay.GetComponent<TMP_Text>().text = QuadLetter;
        QuadCountDisplay.text = QuadCount;
    }

    //this one can come out
    public void UpdateFinalReport(string Count)
    {
        QuadLetterDisplay.GetComponent<TMP_Text>().text = "";
        QuadCountDisplay.text = Count;
    }
}
