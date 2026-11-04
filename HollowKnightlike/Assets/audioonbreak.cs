using UnityEngine;

public class audioonbreak : MonoBehaviour
{
    public int health = 100;
    public AudioClip deathSound; // Assign your audio file in the Inspector

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Plays the sound at the enemy's location, then automatically cleans itself up
        AudioSource.PlayClipAtPoint(deathSound, transform.position);

        // Now it is perfectly safe to destroy the enemy instantly
        Destroy(gameObject);
    }
}