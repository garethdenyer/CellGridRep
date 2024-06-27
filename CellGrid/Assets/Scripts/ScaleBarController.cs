using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ScaleBarController : MonoBehaviour
{
    public Camera mainCamera;  // Reference to the main camera
    public TMP_Text scaleLabel;  // Reference to the Text component that displays the scale value

    void Update()
    {
        UpdateScaleBar();
    }

    void UpdateScaleBar()
    {
        float cameraZ = mainCamera.transform.position.z;

        // Update the scale label text
        scaleLabel.text = "Whole Grid Width " + (-cameraZ).ToString("F1") + " mm";
    }
}
