using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainTimer : MonoBehaviour
{
    public float timeElapsed;
    public bool timerIsRunning;
    public TMP_Text timeText;
    public float timedistort;

    public Image sliderFill;
    public Gradient sliderGradient; // Define the gradient here

    public Slider timeSlider;



    private void Start()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        timerIsRunning = false;
        timeElapsed = 0;
        timeText.text = string.Format("{0:0}:{1:00}:{2:00}", 0, 0, 0);
        timeSlider.value = 1;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            timeElapsed += Time.deltaTime * timedistort;
            DisplayTime(timeElapsed);
        }

        float t = Mathf.InverseLerp(0f, 200f, timedistort);

        // Use Gradient.Evaluate to get the color at the specified point in the gradient
        Color color = sliderGradient.Evaluate(t);

        // Set the color of the slider's fill
        sliderFill.color = color;
    }



    void DisplayTime(float timeToDisplay)
    {
        int hours = Mathf.FloorToInt(timeToDisplay / 3600) % 24;
        int minutes = Mathf.FloorToInt((timeToDisplay / 60) % 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if (timedistort == 0)
        {
            timeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds) + "\nPAUSED";
        }
        else
        {
            timeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
    }

    public void DistortButton(float distortfactor)
    {
        timedistort = distortfactor;
        timeSlider.value = distortfactor;
    }


    public void ChangedSliderSetting()
    {
        if (timeSlider.value == 0)
        {
            timedistort = 0f;
        }
        else
        {
            timedistort = Mathf.Pow(timeSlider.value, (timeSlider.value / 1.6f));
        }
    }

}
