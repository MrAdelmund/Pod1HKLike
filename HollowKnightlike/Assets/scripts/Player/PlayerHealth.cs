using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 6;
    public int maxHealth = 6; // Added to track when health is full again
    public TMP_Text text;

    // Reference to your "iframes" script
    private iframes iFramesScript;

    [Header("Respawn Settings")]
    public Transform spawnPoint;

    private Rigidbody rb;
    private CharacterController controller;

    [Header("UI Events")]
    public UnityEvent onTakeDamage;
    public UnityEvent onHeal;

    void Start()
    {
        maxHealth = health;
        iFramesScript = GetComponent<iframes>();
        UpdateHealthUI();

        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        if (spawnPoint == null)
        {
            GameObject defaultSpawn = new GameObject("DefaultSpawnPoint");
            defaultSpawn.transform.position = transform.position;
            spawnPoint = defaultSpawn.transform;
        }
    }

    public void FallAndRespawn()
    {
        health -= 1;
        UpdateHealthUI();
        onTakeDamage?.Invoke(); // Triggers the damaged UI animation
        Debug.Log("Player fell! Remaining health: " + health);

        if (health > 0)
        {
            Respawn();
        }
        else
        {
            Die();
        }
    }

    void Respawn()
    {
        if (controller != null) controller.enabled = false;

        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (controller != null) controller.enabled = true;
    }

    public void UpdateSpawnPoint(Transform newSpawnPoint)
    {
        if (spawnPoint != newSpawnPoint)
        {
            spawnPoint = newSpawnPoint;
            Debug.Log("Checkpoint reached: " + newSpawnPoint.name);
        }
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
            health = Mathf.Min(health + 1, maxHealth);
            UpdateHealthUI();

            // Reverts the damaged UI animation when healed back to full
            if (health >= maxHealth)
            {
                onHeal?.Invoke();
            }
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
        onTakeDamage?.Invoke(); // Triggers the damaged UI animation
        Debug.Log($"Player took {damageAmount} damage! Health remaining: {health}");

        if (health <= 0)
        {
            Die();
        }
        else if (iFramesScript != null)
        {
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


