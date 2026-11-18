using UnityEngine;

public class Fireball : MonoBehaviour
{
    private EnemyStats stats;

    private Vector2 direction;

    public void Launch(Vector2 dir, EnemyStats source)
    {
        stats = source;
        direction = dir.normalized;
        Destroy(gameObject, stats.projectileLifetime);
    }

    void Update()
    {
        if (stats == null)
            return;

        transform.Translate(direction * stats.projectileSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player != null && stats != null)
        {
            player.TakeDamage(stats.damage);
            Destroy(gameObject);
        }
    }
}