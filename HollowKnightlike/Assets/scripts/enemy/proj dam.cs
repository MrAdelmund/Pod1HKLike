using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public int damageValue = 10; // How much damage this specific bullet does

    // Automatically runs when this trigger collider hits another collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Check if the object we collided with is the player
        if (other.CompareTag("Player"))
        {
            // 2. Try to grab the PlayerHealth script off the object we hit
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // 3. If the script exists, make the player take damage
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageValue);
            }

            // 4. Destroy the projectile so it doesn't pass completely through the player
            Destroy(gameObject);
        }

        // Optional: Destroy the bullet if it hits a solid wall/environment
        else if (other.CompareTag("Ground") || other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
