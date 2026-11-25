using UnityEngine;

public class Shooter : EnemyBase
{
    enum State { Idle, Aim, Shoot, Cooldown }

    [Header("Attack")]
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] SpriteRenderer sprite;

    State state = State.Idle;
    float timer;

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        switch (state)
        {
            case State.Idle:
                if (CanSeePlayer())
                {
                    state = State.Aim;
                    timer = stats.windUpTime;
                    MusicManager.Play(GameSound.ShotWarning);
                }
                break;

            case State.Aim:
                if (!CanSeePlayer())
                {
                    sprite.color = Color.white;
                    state = State.Idle;
                    break;
                }
                Face(player.position.x > transform.position.x ? 1 : -1);
                timer -= Time.deltaTime;
                if (timer <= stats.windUpTime - VisualCueDelay)
                    sprite.color = Color.red;
                if (timer <= 0f) state = State.Shoot;
                break;

            case State.Shoot:
                MusicManager.Play(GameSound.EnemyShot);
                Fire();
                sprite.color = Color.white;
                state = State.Cooldown;
                timer = stats.cooldown;
                break;

            case State.Cooldown:
                timer -= Time.deltaTime;
                if (timer <= 0f) state = State.Idle;
                break;
        }
    }

    void Fire()
    {
        Vector2 dir = (Vector2)player.position - (Vector2)firePoint.position;
        GameObject fire = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        fire.SetActive(true);

        Fireball fb = fire.GetComponent<Fireball>();
        if (fb != null) fb.Launch(dir, stats);
    }
}
