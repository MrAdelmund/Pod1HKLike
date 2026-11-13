using UnityEngine;

public class cameraZone : MonoBehaviour
{
    public GameObject camera;
    public Transform target;
    public Transform player;

    public bool changeSize;
    public float zoneSize;

    private float baseSize;
    private cameraFollow camFollow; // Cached for better performance

    private void Start()
    {
        if (camera != null)
        {
            camFollow = camera.GetComponent<cameraFollow>();
            baseSize = camFollow.cameraSize;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && camFollow != null)
        {
            camFollow.target = target;

            if (changeSize)
            {
                camFollow.newCameraSize = zoneSize;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 1. Only run when the Player exits
        // 2. Check that camFollow/camera hasn't been destroyed (e.g., when stopping Play Mode)
        if (other.CompareTag("Player") && camFollow != null)
        {
            camFollow.target = player;

            if (changeSize)
            {
                camFollow.newCameraSize = baseSize;
            }
        }
    }
}
