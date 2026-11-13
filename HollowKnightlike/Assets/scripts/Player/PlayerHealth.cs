using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerHealth : MonoBehaviour
{
    public int health = 6;
    // The text object we want to update
    public TMP_Text text;

    // Reference to your "iframes" script
    private iframes iFramesScript;

    [Header("Respawn Settings")]
    public Transform spawnPoint; // Drag an empty GameObject here for your spawn location

    private Rigidbody rb;
    private CharacterController controller;
    public UnityEvent onTakeDamage;
    public UnityEvent onHeal;

    void Start()
    {
        iFramesScript = GetComponent<iframes>();
        UpdateHealthUI();

        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        // If no spawn point is assigned, use the player's starting position
        if (spawnPoint == null)
        {
            GameObject defaultSpawn = new GameObject("DefaultSpawnPoint");
            defaultSpawn.transform.position = transform.position;
            spawnPoint = defaultSpawn.transform;
        }
    }

    public void FallAndRespawn()
    {
        // 1. Subtract 1 health
        health -= 1;
        Debug.Log("Player fell! Remaining health: " + health);

        // 2. Check if the player still has health left
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
        // Temporarily disable CharacterController if your player uses one 
        // (Unity's CharacterController overrides transform.position changes if active)
        if (controller != null) controller.enabled = false;

        // Move player back to the spawn point
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        // Reset falling momentum if using a 3D Rigidbody
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Re-enable CharacterController
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
            health += 1;
            UpdateHealthUI();
        }
    }
      
        // ... your existing health subtraction code ...

        // Trigger the UI animation event
        
    
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
