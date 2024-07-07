
using UnityEngine;


public class CameraChemotaxis : MonoBehaviour
{
    float scanSpeed = 0.2f;
    float zoomSpeed = 1f;

    float minZ = -30f; // Farthest point
    float maxZ = -0.5f; // Closest point

    // Update is called once per frame
    void Update()
    {



        float updown = Input.GetAxis("Vertical") * scanSpeed;
        float leftright = Input.GetAxis("Horizontal") * scanSpeed;


        // Adjust zoom speed based on the camera's Z position
        float distance = Mathf.Clamp(transform.position.z, minZ, maxZ);
        float t = (distance - minZ) / (maxZ - minZ);
        zoomSpeed = Mathf.Lerp(0.1f, 5f, Mathf.Pow(t, 2)); // Quadratic interpolation

        transform.Translate(leftright, updown, 0);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (transform.position.z < maxZ || scroll < 0f)
        {
            transform.Translate(0, 0, scroll * zoomSpeed);
        }
    }

}

