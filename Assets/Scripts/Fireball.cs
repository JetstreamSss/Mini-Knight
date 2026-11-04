using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 6f;
    public float lifeTime = 3f;
    public int damage = 20;

    private Vector2 direction;

    public void Launch(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}