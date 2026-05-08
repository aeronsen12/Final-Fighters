using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    public int damage = 10;

    public Transform attackPoint;
    public float attackRange = 1f;

    public LayerMask enemyLayer;

    public int test;

    public bool isGuarding;

    PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    public void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        if (hit != null)
        {
            PlayerHealth target = hit.GetComponent<PlayerHealth>();

            if (target != null)
            {
                int finalDamage = damage;

                if (target.GetComponent<PlayerCombat>().isGuarding)
                {
                    finalDamage /= 2;
                }

                target.TakeDamage(finalDamage);

                Rigidbody2D targetRb = hit.GetComponent<Rigidbody2D>();

                if (targetRb != null)
                {
                    Vector2 knockback =
                        new Vector2(controller.GetFacing() * 5f, 2f);

                    targetRb.AddForce(knockback, ForceMode2D.Impulse);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}