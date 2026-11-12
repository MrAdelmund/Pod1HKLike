using UnityEngine;

public class SpawnUnderPlayerOnEnter : MonoBehaviour
{
    public GameObject prefabToSpawn; // Assign your prefab in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering is the player (by tag or component)
        if (other.CompareTag("Player"))
        {
            // Instantiate the prefab at the player's position and rotation
            GameObject spawnedObject = Instantiate(prefabToSpawn, other.transform.position, other.transform.rotation);

            // Set the player as the parent so the object moves under/with the player
            spawnedObject.transform.SetParent(other.transform);
        }
    }
}