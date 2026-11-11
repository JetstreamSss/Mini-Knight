using UnityEngine;

public class Patroller : EnemyBase
{
    enum State { Patrol, Chase, WindUp, Cooldown }

    [Header("Patrol")]
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float patrolSpeed = 1.5f;

    [Header("Chase")]
    [SerializeField] float chaseSpeed = 3f;
    [SerializeField] float loseSightTime = 2f;

    [Header("Attack")]
    [SerializeField] float attackRange = 1.2f;
    [SerializeField] float windUp = 0.5f;
    [SerializeField] float cooldown = 1.5f;
    [SerializeField] int damage = 1;
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

        Move(Mathf.Sign(dx), patrolSpeed);

        if (CanSeePlayer()) { state = State.Chase; lostTimer = 0f; }
    }

    void DoChase()
    {
        float dx = player.position.x - transform.position.x;
        Move(Mathf.Sign(dx), chaseSpeed);

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            Stop();
            state = State.WindUp;
            timer = windUp;
            sprite.color = Color.red;            // телеграф
            return;
        }

        if (CanSeePlayer()) lostTimer = 0f;
        else
        {
            lostTimer += Time.fixedDeltaTime;
            if (lostTimer >= loseSightTime) state = State.Patrol;
        }
    }

    void DoWindUp()
    {
        Stop();
        timer -= Time.fixedDeltaTime;
        if (timer > 0f) return;

        // удар: только если игрок всё ещё рядом (можно увернуться)
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerHealth hp = player.GetComponent<PlayerHealth>();
            if (hp != null) hp.TakeDamage(damage);
        }

        sprite.color = Color.white;
        state = State.Cooldown;
        timer = cooldown;
    }

    void DoCooldown()
    {
        Stop();
        timer -= Time.fixedDeltaTime;
        if (timer <= 0f) state = State.Chase;
    }
}