using UnityEngine;

public class LeapingEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float leapCooldown = 2f;
    public Vector2 leapForce = new Vector2(5f, 7f); // X handles forward distance, Y handles height

    [Header("Ground Check")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Transform player;
    private bool isGrounded;
    private float cooldownTimer;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Automatically finds the player via tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        // 1. Check if the enemy is standing on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 2. Track cooldown timers
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // 3. Look at the player and leap if ready
        if (player != null && isGrounded && cooldownTimer <= 0)
        {
            FacePlayer();
            Leap();
        }
    }

    void Leap()
    {
        cooldownTimer = leapCooldown;

        // Determine direction to the player (-1 for Left, 1 for Right)
        float direction = player.position.x > transform.position.x ? 1f : -1f;

        // Reset current velocity so leaps are uniform
        rb.linearVelocity = Vector2.zero;

        // Apply instantaneous diagonal force toward the player
        Vector2 appliedForce = new Vector2(leapForce.x * direction, leapForce.y);
        rb.AddForce(appliedForce, ForceMode2D.Impulse);
    }

    void FacePlayer()
    {
        if (player.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Visualize the ground check circle in the Unity Editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}