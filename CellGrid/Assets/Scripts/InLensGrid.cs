using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InLensGrid : MonoBehaviour
{

    public Camera theCamera;



    // Update is called once per frame
    void Update()
    {
        float camx = theCamera.transform.position.x;
        float camy = theCamera.transform.position.y;

        transform.position = new Vector3(camx, camy, 0f);


    }
}
