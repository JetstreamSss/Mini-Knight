using UnityEngine;

public class Charger : EnemyBase
{
    enum State { Idle, WindUp, Charge, Recover }

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
                    MusicManager.Play(GameSound.ChargeWarning);
                    timer = stats.windUpTime;
                    state = State.WindUp;
                }
                break;

            case State.WindUp:
                Stop();
                timer -= Time.fixedDeltaTime;
                if (timer <= stats.windUpTime - VisualCueDelay)
                    sprite.color = Color.red;
                if (timer <= 0f)
                {
                    timer = stats.chargeTime;
                    hitDone = false;
                    state = State.Charge;
                    MusicManager.Play(GameSound.Charge);
                }
                break;

            case State.Charge:
                Move(chargeDir, stats.chargeSpeed);
                if (!hitDone && Vector2.Distance(transform.position, player.position) <= stats.hitRadius)
                {
                    PlayerHealth hp = player.GetComponent<PlayerHealth>();
                    if (hp != null) hp.TakeDamage(stats.damage);
                    hitDone = true;
                }
                timer -= Time.fixedDeltaTime;
                if (timer <= 0f)
                {
                    Stop();
                    sprite.color = Color.white;
                    timer = stats.recoverTime;
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
