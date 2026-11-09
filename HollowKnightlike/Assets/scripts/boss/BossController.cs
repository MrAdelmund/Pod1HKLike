using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public enum BossState { Intro, Idle, Attacking, Dead }
    public BossState currentState = BossState.Intro;

    [Header("Movement & Target")]
    public Transform player;
    public float moveSpeed = 3f;
    private bool isFacingRight = false;

    [Header("Attack Points")]
    public Transform[] moveSpots; // Random positions the boss can float/move to
    public Transform firePoint;
    public GameObject projectilePrefab;

    private Animator anim;
    private Rigidbody2D rb;
    private BossHealth health;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<BossHealth>();

        // Find the player automatically if not assigned
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(BossLogicRoutine());
    }

    void Update()
    {
        if (currentState != BossState.Dead && player != null)
        {
            LookAtPlayer();
        }
    }

    // A Coroutine behaves like a structured choreo manager for your boss fight
    IEnumerator BossLogicRoutine()
    {
        // 1. INTRO PHASE
        // Play intro animation, wait for it to finish
        anim.SetTrigger("Intro");
        yield return new WaitForSeconds(2f);

        // Loop fight behaviors until dead
        while (currentState != BossState.Dead)
        {
            // 2. IDLE PHASE
            currentState = BossState.Idle;
            anim.SetBool("IsWalking", false);
            yield return new WaitForSeconds(Random.Range(1f, 2.5f)); // Rest between actions

            // 3. CHOOSE AN ATTACK RANDOMIZED
            currentState = BossState.Attacking;
            int randomAttack = Random.Range(0, 2);

            if (randomAttack == 0)
            {
                yield return StartCoroutine(ChargeAttack());
            }
            else
            {
                yield return StartCoroutine(RangedAttack());
            }
        }
    }

    IEnumerator ChargeAttack()
    {
        anim.SetBool("IsWalking", true);
        // Pick a random spot from your array and move towards it
        Transform targetSpot = moveSpots[Random.Range(0, moveSpots.Length)];

        while (Vector2.Distance(transform.position, targetSpot.position) > 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetSpot.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator RangedAttack()
    {
        anim.SetTrigger("Attack"); // Triggers casting/shooting animation
        yield return new WaitForSeconds(0.3f); // Sync bullet instantiation with animation frame

        if (projectilePrefab && firePoint)
        {
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
        yield return new WaitForSeconds(0.5f); // Recovery time
    }

    void LookAtPlayer()
    {
        if (transform.position.x > player.position.x && isFacingRight)
        {
            Flip();
        }
        else if (transform.position.x < player.position.x && !isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void HandleDeath()
    {
        currentState = BossState.Dead;
        StopAllCoroutines();
        anim.SetTrigger("Death");
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false; // Disable collisions so player doesn't take damage anymore
    }
}
