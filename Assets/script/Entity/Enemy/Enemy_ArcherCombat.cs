using UnityEngine;

public class Enemy_ArcherCombat : Entity_Combat
{
    [Header("Archer")]
    [SerializeField] private EnemyArrow arrowPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float arrowSpeed = 14f;

    private Enemy enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
    }

    public override void PerformAttack()
    {
        if (arrowPrefab == null || firePoint == null)
        {
            Debug.LogError("Enemy_ArcherCombat: 没有配置 Arrow Prefab / Fire Point", this);
            return;
        }

        Vector2 direction = GetAimDirection();
        AttackData attackData = stats.GetAttackData(basicAttackScale);
        EnemyArrow arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        arrow.Setup(attackData, direction, transform, arrowSpeed);
    }

    private Vector2 GetAimDirection()
    {
        Transform target = enemy != null ? enemy.player : null;

        if (target == null && enemy != null)
        {
            RaycastHit2D hit = enemy.PlayerDetected();
            if (hit.collider != null)
                target = hit.transform;
        }

        if (target != null)
            return ((Vector2)target.position - (Vector2)firePoint.position).normalized;

        return new Vector2(enemy != null ? enemy.facingDir : 1, 0);
    }
}