using UnityEngine;

public class BossArenaTrigger : MonoBehaviour
{
    public GameObject bossWallGates;  // Colliders that block the exit once entered
    public GameObject bossHealthUI;   // The Canvas panel holding the health slider
    public BossController bossScript;  // Reference to the boss

    void Start()
    {
        bossWallGates.SetActive(false);
        bossHealthUI.SetActive(false);
        bossScript.enabled = false; // Keep boss AI turned off initially
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossWallGates.SetActive(true); // Close the doors
            bossHealthUI.SetActive(true);   // Display health bar
            bossScript.enabled = true;    // Awaken the boss AI

            // Disable this trigger zone so it doesn't fire repeatedly
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
