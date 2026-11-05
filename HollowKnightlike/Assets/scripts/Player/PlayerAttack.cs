using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public int attackDamage = 1;
    public float attackRange = 0.5f;
    public float attackCooldown = 0.3f;

    [Header("Attack Detection")]
    public Transform attackPoint;
    public LayerMask enemyLayers;

    [Header("References")]
    public PlayerMovement playerMovement;
    public bool isAttacking = false;

    float cooldownTimer;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && cooldownTimer <= 0)
        {
            Attack();
            isAttacking = true;
        }

        if(cooldownTimer <= 0)
        {
            isAttacking = false;
        }
    }

    void Attack()
    {
        cooldownTimer = attackCooldown;

        // Determine attack direction based on facing
        Vector2 attackDirection = Vector2.right * playerMovement.facingDirection;

        // Calculate attack position
        Vector2 attackPosition = (Vector2)attackPoint.position + attackDirection * attackRange * 0.5f;
        //play attack animation
        if (attackPoint.GetComponent<Animator>() != null)
        {
            attackPoint.GetComponent<Animator>().SetTrigger("Attack");
        }
        // Detect enemies
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(
            attackPosition,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D enemy in enemiesHit)
        {
            enemy.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        if (playerMovement == null) return;

        Vector2 attackDirection = Vector2.right * playerMovement.facingDirection;
        Vector2 attackPosition = (Vector2)attackPoint.position + attackDirection * attackRange * 0.5f;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}
