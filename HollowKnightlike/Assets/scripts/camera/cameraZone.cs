using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cameraZone : MonoBehaviour
{
    public GameObject camera;

    public Transform target;

    public Transform player;

    public bool changeSize;

    public float zoneSize;

    float baseSize;

    private void Start()
    {
        baseSize = camera.GetComponent<cameraFollow>().cameraSize;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            camera.GetComponent<cameraFollow>().target = target;

            if (changeSize)
            {
                camera.GetComponent<cameraFollow>().newCameraSize = zoneSize;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        camera.GetComponent<cameraFollow>().target = player;

        if (changeSize)
        {
            camera.GetComponent<cameraFollow>().newCameraSize = baseSize;
        }
        
    }
}
