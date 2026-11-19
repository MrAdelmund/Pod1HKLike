using System.Collections;
using UnityEngine;

public class EnemyGrab : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public LineRenderer lineRenderer;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer; // Walls/ground that block the grab

    [Header("Grab Settings")]
    public float grabRange = 8f;
    public float extendSpeed = 25f;
    public float pullSpeed = 15f;
    public float stopDistance = 1.5f; // How close to pull the player
    public float cooldown = 3f;

    private Transform player;
    private bool isGrabbing = false;
    private float nextGrabTime = 0f;

    void Start()
    {
        // Find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (player == null || isGrabbing || Time.time < nextGrabTime) return;

        float distanceToPlayer = Vector2.Distance(firePoint.position, player.position);

        // Check if player is within range
        if (distanceToPlayer <= grabRange)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;

            // Raycast to check if there is a clear line of sight (not blocked by walls)
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, grabRange, playerLayer | obstacleLayer);

            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
            {
                StartCoroutine(PerformGrab(hit.collider.gameObject));
            }
        }
    }

    private IEnumerator PerformGrab(GameObject targetPlayer)
    {
        isGrabbing = true;
        lineRenderer.enabled = true;

        Rigidbody2D playerRb = targetPlayer.GetComponent<Rigidbody2D>();

        // 1. Extend the tether toward the player
        Vector2 currentEndPos = firePoint.position;
        while (Vector2.Distance(currentEndPos, targetPlayer.transform.position) > 0.2f)
        {
            currentEndPos = Vector2.MoveTowards(currentEndPos, targetPlayer.transform.position, extendSpeed * Time.deltaTime);
            UpdateLine(firePoint.position, currentEndPos);
            yield return null;
        }

        // Optional: Disable player's movement script here so they can't fight the pull
        // targetPlayer.GetComponent<PlayerMovement>().enabled = false;

        // 2. Pull the player toward the enemy
        float originalGravity = playerRb != null ? playerRb.gravityScale : 0f;
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero; // Use playerRb.velocity in older Unity versions
            playerRb.gravityScale = 0f; // Prevents gravity from dragging them down mid-pull
        }

        while (Vector2.Distance(firePoint.position, targetPlayer.transform.position) > stopDistance)
        {
            targetPlayer.transform.position = Vector2.MoveTowards(
                targetPlayer.transform.position,
                firePoint.position,
                pullSpeed * Time.deltaTime
            );
            UpdateLine(firePoint.position, targetPlayer.transform.position);
            yield return null;
        }

        // 3. Release the player
        if (playerRb != null)
        {
            playerRb.gravityScale = originalGravity;
        }

        // Re-enable player's movement script here
        // targetPlayer.GetComponent<PlayerMovement>().enabled = true;

        lineRenderer.enabled = false;
        isGrabbing = false;
        nextGrabTime = Time.time + cooldown;
    }

    private void UpdateLine(Vector2 start, Vector2 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    // Visualizes the grab range in the Scene view
    private void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, grabRange);
    }
}

