using UnityEngine;

public class ProjectileAlone : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // transform.right fires the projectile in the direction its right side is facing.
        // Because our boss flips 180 degrees to look at the player, the projectile 
        // will naturally shoot left or right perfectly!
        rb.linearVelocity = transform.right * speed;

        // Failsafe: Destroy the bullet after 4 seconds so it doesn't fly forever into space
        Destroy(gameObject, 4f);
    }
}
