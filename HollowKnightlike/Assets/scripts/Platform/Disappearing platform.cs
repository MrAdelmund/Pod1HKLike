using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private float disappearTime = 2.0f; // Time the platform stays visible
    [SerializeField] private float reappearTime = 2.0f;  // Time the platform stays hidden

    private Renderer platformRenderer;
    private Collider2D platformCollider;

    void Start()
    {
        platformRenderer = GetComponent<Renderer>();
        platformCollider = GetComponent<Collider2D>(); 

        StartCoroutine(TogglePlatformRoutine());
    }

    IEnumerator TogglePlatformRoutine()
    {
        while (true)
        {
            // Platform is visible and solid
            platformRenderer.enabled = true;
            platformCollider.enabled = true;
            yield return new WaitForSeconds(disappearTime);

            // Platform disappears and lets objects pass through
            platformRenderer.enabled = false;
            platformCollider.enabled = false;
            yield return new WaitForSeconds(reappearTime);
        }
    }
}
