using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public Animator animator;
    public float attackCooldown = 2f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= attackCooldown)
        {
            timer = 0f;
            animator.SetTrigger("Attack");
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject fire = Instantiate(
            fireballPrefab,
            firePoint.position,
            Quaternion.identity
        );

        fire.SetActive(true);

        Fireball script = fire.GetComponent<Fireball>();

        if (script != null)
            script.Launch(Vector2.right);
    }
}