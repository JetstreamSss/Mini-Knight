
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Рыцарь вошёл в зону врага!");

        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(stats.damage);
        }
        else
        {
            Debug.LogWarning("На объекте Player нет PlayerHealth!");
        }
    }
}
