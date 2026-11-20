
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCannon : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private Transform launchPoint;
    [SerializeField] private float launchForce = 25f;
    [SerializeField] private float flightTimeBeforeFade = 0.5f;

    [Header("Transition Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.75f;
    [SerializeField] private string nextSceneName;

    private bool isLaunching = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLaunching && collision.CompareTag("Player"))
        {
            StartCoroutine(LaunchAndFade(collision.gameObject));
        }
    }

    private IEnumerator LaunchAndFade(GameObject player)
    {
        isLaunching = true;

        // 1. Disable the player's movement script so input cannot override the launch
        // Replace 'PlayerMovement' with your script's exact class name if needed
        PlayerMovement movementScript = player.GetComponent<PlayerMovement>();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        // 2. Snap player to barrel, clear existing momentum, and fire
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        player.transform.position = launchPoint.position;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Use rb.velocity if on Unity 2022 or older
            rb.angularVelocity = 0f;
            rb.AddForce(launchPoint.right * launchForce, ForceMode2D.Impulse);
        }

        // 3. Let the player fly briefly before fading
        yield return new WaitForSeconds(flightTimeBeforeFade);

        // 4. Smoothly fade the screen to black
        if (fadeCanvasGroup != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;
        }

        // 5. Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}