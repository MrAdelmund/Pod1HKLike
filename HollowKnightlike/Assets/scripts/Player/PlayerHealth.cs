using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health = 6;
    public int maxHealth = 6;
    public TMP_Text text;

    private iframes iFramesScript;

    [Header("Respawn Settings")]
    public Transform spawnPoint;

    private Rigidbody rb;
    private CharacterController controller;

    [Header("UI References & Events")]
    [SerializeField] private Assets.scripts.visual.UIHealthDamage uiHealthDamage;
    public UnityEvent<int> onHealthChanged; // Sends the current health number

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAndSyncHealthUI();
    }

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

    private void FindAndSyncHealthUI()
    {
        // Find the active UIHealthDamage inside the loaded Canvas prefab
        if (uiHealthDamage == null || !uiHealthDamage.gameObject.scene.IsValid())
        {
            uiHealthDamage = FindFirstObjectByType<Assets.scripts.visual.UIHealthDamage>();
        }

        if (uiHealthDamage != null)
        {
            uiHealthDamage.UpdateHearts(health);
        }
        else
        {
            Debug.LogWarning("Health Bar Canvas prefab was not found in scene: " + SceneManager.GetActiveScene().name);
        }
    }

    public void FallAndRespawn()
    {
        health -= 1;
        UpdateHealthUI();
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
            Heal(6);
            Destroy(collision.gameObject); // Removes the health pack after picking it up
        }
    }

    public void Heal(int healAmount)
    {
        health = Mathf.Clamp(health + healAmount, 0, maxHealth);
        UpdateHealthUI();
        Debug.Log($"Player healed {healAmount}! Health remaining: {health}");
    }

    public void TakeDamage(int damageAmount)
    {
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
            iFramesScript.TriggerIFrames();
        }
    }

    void UpdateHealthUI()
    {
        if (text != null)
        {
            text.text = "Health: " + health;
        }

        // Automatically finds and updates the Health Bar Canvas prefab in any scene
        FindAndSyncHealthUI();

        // Still invokes any extra UnityEvents you have wired up
        onHealthChanged?.Invoke(health);
    }

    void Die()
    {
        Debug.Log("You died!");
        SceneManager.LoadScene("DEATH");
    }
}