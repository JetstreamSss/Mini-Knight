
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    private int health;

    private void Awake() => health = stats.maxHP;

    public void TakeDamage(int damage)
    {
        if (health <= 0)
            return;

        health -= damage;
        MusicManager.Play(GameSound.Hit);
        Debug.Log("Враг получил урон! HP: " + health);

        if (health <= 0)
        {
            Debug.Log("Враг уничтожен: " + gameObject.name);

            MusicManager.Play(GameSound.Death);

            Destroy(gameObject);
        }
    }
}
