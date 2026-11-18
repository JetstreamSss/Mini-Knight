using UnityEngine;

public class Patroller : EnemyBase
{
    enum State { Patrol, Chase, WindUp, Cooldown }

    [Header("Patrol")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    [Header("Attack")]
    [SerializeField] SpriteRenderer sprite;

    State state = State.Patrol;
    Transform patrolTarget;
    float timer;
    float lostTimer;

    protected override void Start()
    {
        base.Start();
        patrolTarget = pointB;
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
        float dx = patrolTarget.position.x - transform.position.x;
        if (Mathf.Abs(dx) < 0.1f)
            patrolTarget = (patrolTarget == pointA) ? pointB : pointA;

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
            sprite.color = Color.red;            // телеграф
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
        if (timer > 0f) return;

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