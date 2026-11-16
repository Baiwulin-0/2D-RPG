using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private GameObject hitVfx;

    private Rigidbody2D rb;
    private AttackData attackData;
    private Transform shooter;
    private bool hasHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(AttackData data, Vector2 direction, Transform shooter, float speed)
    {
        attackData = data;
        this.shooter = shooter;

        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit || attackData == null)
            return;

        // 忽略射手自己
        if (shooter != null &&
            (collision.transform == shooter || collision.transform.IsChildOf(shooter)))
            return;

        // 只对玩家造成伤害，撞到其他东西（墙/地形/别的敌人）直接消失
        if (collision.CompareTag("Player") == false)
        {
            Destroy(gameObject);
            return;
        }

        IDamagable damagable = collision.GetComponent<IDamagable>();

        if (damagable == null)
            return;

        hasHit = true;

        bool targetGotHit = damagable.TakeDamage(
            attackData.phyiscalDamage,
            attackData.elementalDamage,
            attackData.element,
            shooter);


        if (targetGotHit)
        {
            if (attackData.element != ElementType.None)
            {
                Entity_StatusHandler statusHandler = collision.GetComponent<Entity_StatusHandler>();
                statusHandler?.ApplyStatusEffect(attackData.element, attackData.effectData);
            }

            if (hitVfx != null)
                Instantiate(hitVfx, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}