using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    [Header("Ground & Wall Detection")]
    public LayerMask groundLayer;
    public Transform ledgeCheck;
    public float checkDistance = 0.5f;

    private Rigidbody2D rb;
    private Transform currentTarget;
    private int direction = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointB;
        UpdateDirection();
    }

    void FixedUpdate()
    {
        // 1. Move horizontally towards the current waypoint while preserving gravity
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

        // 2. Check for walls ahead and ground below
        RaycastHit2D wallHit = Physics2D.Raycast(ledgeCheck.position, Vector2.right * direction, checkDistance, groundLayer);
        RaycastHit2D groundHit = Physics2D.Raycast(ledgeCheck.position, Vector2.down, checkDistance, groundLayer);

        // 3. Check if the enemy reached the horizontal position of the waypoint
        bool reachedWaypoint = Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.2f;

        // Switch target if at waypoint, blocked by a wall, or at a ledge
        if (reachedWaypoint || wallHit.collider != null || groundHit.collider == null)
        {
            SwitchTarget();
        }
    }

    void SwitchTarget()
    {
        currentTarget = (currentTarget == pointB) ? pointA : pointB;
        UpdateDirection();
    }

    void UpdateDirection()
    {
        // Determine if the new target is to the right (1) or left (-1)
        direction = (currentTarget.position.x > transform.position.x) ? 1 : -1;

        // Flip the sprite to match the movement direction
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    void OnDrawGizmos()
    {
        if (ledgeCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + Vector3.right * direction * checkDistance);
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + Vector3.down * checkDistance);
        }
    }
}