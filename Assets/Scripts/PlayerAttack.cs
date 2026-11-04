
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int damage = 1;
    public float attackRange = 3f;
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public AudioSource attackAudio;

    private void Awake()
    {
        if (attackAudio == null)
            attackAudio = GetComponent<AudioSource>();
    }

    public void Attack()
    {
        if (attackAudio != null && attackAudio.clip != null)
            attackAudio.PlayOneShot(attackAudio.clip);

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
