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

        // 1. Snap player to barrel and apply impulse force
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        player.transform.position = launchPoint.position;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Use rb.velocity if on Unity 2022 or older
            rb.AddForce(launchPoint.right * launchForce, ForceMode2D.Impulse);
        }

        // 2. Let the player fly briefly before the screen starts darkening
        yield return new WaitForSeconds(flightTimeBeforeFade);

        // 3. Smoothly fade the CanvasGroup alpha from 0 to 1
        if (fadeCanvasGroup != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
                yield return null; // Wait for the next frame
            }
            fadeCanvasGroup.alpha = 1f;
        }

        // 4. Load the next scene once the screen is completely black
        SceneManager.LoadScene(nextSceneName);
    }
}