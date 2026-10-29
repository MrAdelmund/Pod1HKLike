using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;


public class cameraFollow : MonoBehaviour
{
    public Transform target;

    public float layer;

    public float lerpSpeed;

    public float cameraSize;

    public float newCameraSize;

    float baseSize = 7.5f;

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, layer);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * lerpSpeed);

    

        if (cameraSize != newCameraSize)
        {
            if(cameraSize > newCameraSize)
            {
                GetComponent<Camera>().fieldOfView = GetComponent<Camera>().fieldOfView - 25f * Time.deltaTime;
           
                cameraSize = GetComponent<Camera>().fieldOfView;

                if(cameraSize < newCameraSize)
                {
                    GetComponent<Camera>().fieldOfView = newCameraSize;

                        cameraSize = newCameraSize;
                }

            }
            else
            {
                GetComponent<Camera>().fieldOfView = GetComponent<Camera>().fieldOfView + 25f * Time.deltaTime;

                cameraSize = GetComponent<Camera>().fieldOfView;

                if (cameraSize > newCameraSize)
                {
                    GetComponent<Camera>().fieldOfView = newCameraSize;

                    cameraSize = newCameraSize;
                }
            }
        }

        
    }

}
