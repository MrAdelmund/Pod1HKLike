using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Use this instead if you are making a 2D Game (requires BoxCollider2D & Rigidbody2D)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.FallAndRespawn();
            }
        }
    }
}
