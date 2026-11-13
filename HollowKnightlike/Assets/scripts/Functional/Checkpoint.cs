using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Optional Settings")]
    public Transform customRespawnSpot; // Leave empty to respawn at the checkpoint's center
    private bool isActivated = false;


    // Use this instead if you are making a 2D game
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            ActivateCheckpoint(other.gameObject);
        }
    }

    private void ActivateCheckpoint(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Use customRespawnSpot if assigned; otherwise, use this checkpoint's transform
            Transform targetSpawn = customRespawnSpot != null ? customRespawnSpot : transform;
            playerHealth.UpdateSpawnPoint(targetSpawn);

            isActivated = true;
            // Optional: Add visual or audio feedback here (e.g., change flag color or play a sound)
        }
    }
}
