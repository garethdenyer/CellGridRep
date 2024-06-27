using UnityEngine;


public class CameraUDLRZ : MonoBehaviour
{
    float scanspeed = 0.2f;
    float zoomSpeed = 1f;

    float maxZ = -0.5f;  //stops camera getting too close and clipping content


    // Update is called once per frame
    void Update()
    {
        float updown = Input.GetAxis("Vertical") * scanspeed;
        float leftright = Input.GetAxis("Horizontal") * scanspeed;

        transform.Translate(1 * leftright, 1 * updown, 0);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (transform.position.z < maxZ || scroll < 0f)
        {
            transform.Translate(0, 0, scroll * zoomSpeed);
        }

    }
}

