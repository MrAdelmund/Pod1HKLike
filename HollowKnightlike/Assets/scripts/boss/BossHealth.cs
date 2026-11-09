using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 500;
    private int currentHealth;

    [Header("UI Components")]
    public Slider healthSlider; // Assign a UI Slider from your Canvas

    [Header("Phases")]
    public bool isEnraged = false;
    public int enrageThreshold = 200;

    private Animator anim;
    private BossController controller;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        controller = GetComponent<BossController>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        anim.SetTrigger("Hurt");

        // Check for Phase 2 (Enrage)
        if (currentHealth <= enrageThreshold && !isEnraged)
        {
            EnrageBoss();
        }

        if (currentHealth <= 0)
            Die();
    }

    void EnrageBoss()
    {
        isEnraged = true;
        anim.SetBool("IsEnraged", true);

        // Make the boss faster/more aggressive for Phase 2
        controller.moveSpeed *= 1.5f;
    }

    void Die()
    {
        controller.HandleDeath();
    }
}
