using UnityEngine;
using UnityEngine.UI; // Required for UI Image

public class HealthUI : MonoBehaviour
{
    [Header("UI Reference")]
    public Image healthImage;

    [Header("Sprites (Index 0 = Dead/Empty, Last Index = Full)")]
    public Sprite[] healthSprites;

    // Call this method whenever the player takes damage or heals
    public void UpdateHealthSprite(int currentHealth, int maxHealth)
    {
        if (healthSprites.Length == 0) return;

        // Calculate health percentage (0.0 to 1.0)
        float healthPercent = Mathf.Clamp01((float)currentHealth / maxHealth);

        // Map the percentage to the correct array index
        int spriteIndex = Mathf.RoundToInt(healthPercent * (healthSprites.Length - 1));

        // Swap the UI Image sprite
        healthImage.sprite = healthSprites[spriteIndex];
    }
}