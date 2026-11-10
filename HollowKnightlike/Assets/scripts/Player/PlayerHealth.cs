using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerHealth : MonoBehaviour
{
    public int health = 6;
    // The text object we want to update
    public TMP_Text text;

    // Reference to your "iframes" script
    private iframes iFramesScript;

    void Start()
    {
        iFramesScript = GetComponent<iframes>();
        UpdateHealthUI();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("I hit an enemy!");
            TakeDamage(1);
        }

        if (collision.gameObject.CompareTag("HealthPack"))
        {
            health += 1;
            UpdateHealthUI();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        // Stop immediately if i-frames are currently active
        if (iFramesScript != null && iFramesScript.IsInvulnerable)
        {
            return;
        }

        health -= damageAmount;
        UpdateHealthUI();
        Debug.Log($"Player took {damageAmount} damage! Health remaining: {health}");

        if (health <= 0)
        {
            Die();
        }
        else if (iFramesScript != null)
        {
            // Activate i-frames after taking damage
            iFramesScript.TriggerIFrames();
        }
    }

    void UpdateHealthUI()
    {
        if (text != null)
        {
            text.text = "Health: " + health;
        }
    }

    void Die()
    {
        Debug.Log("You died!");
        SceneManager.LoadScene("DEATH");
    }
}
