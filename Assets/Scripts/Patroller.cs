using UnityEngine;

public class Patroller : EnemyBase
{
    enum State { Patrol, Chase, WindUp, Cooldown }

    [Header("Patrol")]
    [SerializeField] Vector2 patrolOffsets;

    [Header("Attack")]
    [SerializeField] SpriteRenderer sprite;

    State state = State.Patrol;
    float spawnX;
    bool towardsB = true;
    float timer;
    float lostTimer;

    protected override void Start()
    {
        base.Start();
        spawnX = transform.position.x;
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Patrol:   DoPatrol();   break;
            case State.Chase:    DoChase();    break;
            case State.WindUp:   DoWindUp();   break;
            case State.Cooldown: DoCooldown(); break;
        }
    }

    void DoPatrol()
    {
        float dx = spawnX + (towardsB ? patrolOffsets.y : patrolOffsets.x) - transform.position.x;
        if (Mathf.Abs(dx) <= stats.patrolSpeed * Time.fixedDeltaTime)
        {
            towardsB = !towardsB;
            dx = spawnX + (towardsB ? patrolOffsets.y : patrolOffsets.x) - transform.position.x;
        }

        Move(Mathf.Sign(dx), stats.patrolSpeed);

        if (CanSeePlayer()) { state = State.Chase; lostTimer = 0f; }
    }

    void DoChase()
    {
        float dx = player.position.x - transform.position.x;
        Move(Mathf.Sign(dx), stats.chaseSpeed);

        if (Vector2.Distance(transform.position, player.position) <= stats.attackRange)
        {
            Stop();
            state = State.WindUp;
            timer = stats.windUpTime;
            MusicManager.Play(GameSound.MeleeWarning);
            return;
        }

        if (CanSeePlayer()) lostTimer = 0f;
        else
        {
            lostTimer += Time.fixedDeltaTime;
            if (lostTimer >= stats.loseSightTime) state = State.Patrol;
        }
    }

    void DoWindUp()
    {
        Stop();
        timer -= Time.fixedDeltaTime;
        if (timer <= stats.windUpTime - VisualCueDelay)
            sprite.color = Color.red;
        if (timer > 0f) return;

        MusicManager.Play(GameSound.EnemySwing);

        // удар: только если игрок всё ещё рядом (можно увернуться)
        if (Vector2.Distance(transform.position, player.position) <= stats.attackRange)
        {
            PlayerHealth hp = player.GetComponent<PlayerHealth>();
            if (hp != null) hp.TakeDamage(stats.damage);
        }

        sprite.color = Color.white;
        state = State.Cooldown;
        timer = stats.cooldown;
    }

    void DoCooldown()
    {
        Stop();
        timer -= Time.fixedDeltaTime;
        if (timer <= 0f) state = State.Chase;
    }
}
