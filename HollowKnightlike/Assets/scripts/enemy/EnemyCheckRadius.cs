using UnityEngine;

public class EnemyRadiusCheck : MonoBehaviour
{
    public float checkRadius = 5f;
    public LayerMask playerLayer;
    public float moveSpeed = 3f;

    void Update()
    {
        // 1. Check if the player is inside the radius
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, checkRadius, playerLayer);

        // 2. If the player is NOT in the radius, do nothing and exit Update immediately
        if (playerCollider == null)
        {
            return;
        }

        // 3. Everything below this line ONLY runs when the player is inside the radius
        Transform playerTransform = playerCollider.transform;
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
    }

    // Draws the red circle in the Scene view so you can see the range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
