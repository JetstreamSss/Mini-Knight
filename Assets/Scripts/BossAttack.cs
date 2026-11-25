using System.Collections;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform firePoint;
    public Animator animator;
    [SerializeField] private EnemyStats stats;

    [SerializeField] private float windUpTime = 0.55f;
    private const float VisualCueDelay = 0.16f;
    private float timer;
    private bool windingUp;

    void Update()
    {
        if (windingUp)
            return;

        timer += Time.deltaTime;

        if (timer >= stats.cooldown)
            StartCoroutine(WindUp());
    }

    private IEnumerator WindUp()
    {
        windingUp = true;
        MusicManager.Play(GameSound.BossWarning);
        yield return new WaitForSeconds(VisualCueDelay);

        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(Mathf.Max(0f, windUpTime - VisualCueDelay));

        MusicManager.Play(GameSound.EnemyShot);
        Shoot();
        timer = 0f;
        windingUp = false;
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
            script.Launch(Vector2.right, stats);
    }
}
