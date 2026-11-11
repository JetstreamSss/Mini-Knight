using UnityEngine;

public class Charger : EnemyBase
{
    enum State { Idle, WindUp, Charge, Recover }

    [Header("Charge")]
    [SerializeField] float windUp = 0.7f;
    [SerializeField] float chargeSpeed = 9f;
    [SerializeField] float chargeTime = 0.6f;
    [SerializeField] float recoverTime = 1.5f;

    [Header("Hit")]
    [SerializeField] int damage = 1;
    [SerializeField] float hitRadius = 0.9f;
    [SerializeField] SpriteRenderer sprite;

    State state = State.Idle;
    float timer;
    int chargeDir = 1;
    bool hitDone;

    void FixedUpdate()
    {
        switch (state)
        {
            case State.Idle:
                Stop();
                if (CanSeePlayer())
                {
                    chargeDir = player.position.x > transform.position.x ? 1 : -1;
                    Face(chargeDir);
                    sprite.color = Color.red;      // телеграф
                    timer = windUp;
                    state = State.WindUp;
                }
                break;

            case State.WindUp:
                Stop();
                timer -= Time.fixedDeltaTime;
                if (timer <= 0f)
                {
                    timer = chargeTime;
                    hitDone = false;
                    state = State.Charge;
                }
                break;

            case State.Charge:
                Move(chargeDir, chargeSpeed);
                if (!hitDone && Vector2.Distance(transform.position, player.position) <= hitRadius)
                {
                    PlayerHealth hp = player.GetComponent<PlayerHealth>();
                    if (hp != null) hp.TakeDamage(damage);
                    hitDone = true;
                }
                timer -= Time.fixedDeltaTime;
                if (timer <= 0f)
                {
                    Stop();
                    sprite.color = Color.white;
                    timer = recoverTime;
                    state = State.Recover;
                }
                break;

            case State.Recover:
                Stop();
                timer -= Time.fixedDeltaTime;
                if (timer <= 0f) state = State.Idle;
                break;
        }
    }
}