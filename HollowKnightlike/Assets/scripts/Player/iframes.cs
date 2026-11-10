using System.Collections;
using UnityEngine;

public class iframes : MonoBehaviour
{
    [Header("I-Frames Settings")]
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private int numberOfFlashes = 5;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Public property so other scripts can check if the player is invincible
    public bool IsInvulnerable { get; private set; }

    private void Awake()
    {
        // Automatically grab the SpriteRenderer if not assigned in the Inspector
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void TriggerIFrames()
    {
        if (!IsInvulnerable)
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        IsInvulnerable = true;

        float flashInterval = duration / (numberOfFlashes * 2);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f); // Semi-transparent
            yield return new WaitForSeconds(flashInterval);

            spriteRenderer.color = Color.white; // Normal color
            yield return new WaitForSeconds(flashInterval);
        }

        spriteRenderer.color = Color.white;
        IsInvulnerable = false;
    }
}
