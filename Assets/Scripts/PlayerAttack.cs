
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 1;
    public float attackRange = 3f;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    public void Attack()
    {
        MusicManager.Play(GameSound.Swing);

        if (attackPoint == null)
            return;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        foreach (Collider2D enemy in enemies)
        {
            BossHealth boss = enemy.GetComponentInParent<BossHealth>();

            if (boss != null)
                boss.TakeDamage(damage);

            EnemyHealth health = enemy.GetComponentInParent<EnemyHealth>();

            if (health != null)
                health.TakeDamage(damage);
        }
    }
}
