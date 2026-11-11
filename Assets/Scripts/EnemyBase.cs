using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Vision")]
    [SerializeField] protected float visionRange = 6f;
    [SerializeField] protected float viewAngle = 60f;      // половина конуса, градусы
    [SerializeField] protected LayerMask sightMask;        // Ground + Player
    [SerializeField] protected Transform eyes;

    protected Transform player;
    protected Rigidbody2D rb;
    protected int facing = 1;                              // 1 = вправо, -1 = влево

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");   // один раз, не в Update
        if (p != null) player = p.transform;
    }

    protected bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 origin = eyes != null ? (Vector2)eyes.position : (Vector2)transform.position;
        Vector2 toPlayer = (Vector2)player.position - origin;

        if (toPlayer.magnitude > visionRange) return false;                    // 1. дистанция
        if (Vector2.Angle(new Vector2(facing, 0), toPlayer) > viewAngle)       // 2. угол
            return false;

        RaycastHit2D hit = Physics2D.Raycast(origin, toPlayer.normalized,      // 3. стены
                                             visionRange, sightMask);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    protected void Move(float dir, float speed)
    {
        rb.linearVelocity = new Vector2(dir * speed, rb.linearVelocity.y);
        if (dir != 0) Face((int)Mathf.Sign(dir));
    }

    protected void Stop()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    protected void Face(int dir)
    {
        if (dir == facing) return;
        facing = dir;
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * dir;   // если спрайт по умолчанию смотрит влево, поменяй знак
        transform.localScale = s;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
}