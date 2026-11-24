
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 3;
    private bool isDead;

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        health -= damage;
        MusicManager.Play(GameSound.Hit);
        Debug.Log("Враг получил урон! HP: " + health);

        if (health <= 0)
        {
            Debug.Log("Враг уничтожен: " + transform.root.name);

            isDead = true;
            MusicManager.Play(GameSound.Death);

            Destroy(transform.root.gameObject);
        }
    }
}
