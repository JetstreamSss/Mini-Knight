
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    private int health;

    private void Awake() => health = stats.maxHP;
    public AudioClip deathSound;
    [Range(0f, 1f)]
    public float deathVolume = 1f;

    public void TakeDamage(int damage)
    {
        if (health <= 0)
            return;

        health -= damage;
        Debug.Log("Враг получил урон! HP: " + health);

        if (health <= 0)
        {
            Debug.Log("Враг уничтожен: " + gameObject.name);

            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(
                    deathSound,
                    transform.position,
                    deathVolume
                );
            }

            Destroy(gameObject);
        }
    }
}
