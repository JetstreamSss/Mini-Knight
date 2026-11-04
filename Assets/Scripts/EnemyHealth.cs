
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 3;
    public AudioClip deathSound;
    [Range(0f, 1f)]
    public float deathVolume = 1f;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Враг получил урон! HP: " + health);

        if (health <= 0)
        {
            Debug.Log("Враг уничтожен: " + transform.root.name);

            if (deathSound != null)
            {
                AudioSource.PlayClipAtPoint(
                    deathSound,
                    transform.position,
                    deathVolume
                );
            }

            Destroy(transform.root.gameObject);
        }
    }
}
