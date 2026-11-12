using UnityEngine;

public class bossleap : MonoBehaviour
{


    public class BossLeap : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform player;
        private Rigidbody2D rb;

        [Header("Leap Settings")]
        [SerializeField] private float jumpForceY = 10f; // Upward force
        [SerializeField] private float leapForceX = 5f;  // Horizontal speed factor
        [SerializeField] private float leapInterval = 3f; // Time between leaps

        private float leapTimer;
        private bool isGrounded;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float checkRadius = 0.2f;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            leapTimer = leapInterval;

            // Automatically find the player if not assigned in Inspector
            if (player == null)
            {
                GameObject pObj = GameObject.FindGameObjectWithTag("Player");
                if (pObj != null) player = pObj.transform;
            }
        }

        void Update()
        {
            // Check if the boss is touching the ground
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

            leapTimer -= Time.deltaTime;

            if (leapTimer <= 0 && isGrounded)
            {
                PerformLeap();
                leapTimer = leapInterval;
            }
        }

        private void PerformLeap()
        {
            if (player == null) return;

            // Determine direction towards the player (-1 for left, 1 for right)
            float directionX = Mathf.Sign(player.position.x - transform.position.x);

            // Apply a vector force: horizontal jump towards player + vertical jump force
            Vector2 leapDirection = new Vector2(directionX * leapForceX, jumpForceY);
            rb.linearVelocity = leapDirection; // Use rb.velocity if using older Unity versions (< 6)
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
            }
        }
    }
}
