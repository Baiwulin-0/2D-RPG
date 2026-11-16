using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Enemy_NightBornCombat : Entity_Combat
{
    private Enemy_NightBorn enemy_NightBorn;


    protected override void Awake()
    {
        base.Awake();
        enemy_NightBorn = GetComponent<Enemy_NightBorn>();
    }

    public override void PerformAttack()
    {
        bool targetGotHit = false;
        if(enemy_NightBorn.CanSelfDestruct())
        {
            Debug.Log("zibaoCircle");
            targetGotHit = circleTarget(targetGotHit);
        }
        else
        {
            targetGotHit = boxTarget(targetGotHit);
        }

        if (targetGotHit == false)
            sfx?.PlayAttackMiss();
    }

    private bool boxTarget(bool targetGotHit)
    {
        foreach (var target in boxDetection.GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;

            AttackData attackData = stats.GetAttackData(basicAttackScale);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();


            float physicalDamage = attackData.phyiscalDamage;
            float elementalDamage = attackData.elementalDamage;
            ElementType element = attackData.element;

            targetGotHit = damagable.TakeDamage(physicalDamage, elementalDamage, element, transform);

            if (element != ElementType.None)
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);

            if (targetGotHit)
            {
                InvokeOnDoingPhysicalDamage(physicalDamage);
                vfx.CreatOnHitVFX(target.transform, attackData.isCrit, element);
                sfx?.PlayAttackHit();
            }
        }

        return targetGotHit;
    }
    private bool circleTarget(bool targetGotHit)
    {
        foreach (var target in circleDetection.GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;

            AttackData attackData = stats.GetAttackData(basicAttackScale);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();


            float physicalDamage = attackData.phyiscalDamage;
            float elementalDamage = attackData.elementalDamage;
            ElementType element = attackData.element;

            targetGotHit = damagable.TakeDamage(physicalDamage, elementalDamage, element, transform);

            if (element != ElementType.None)
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);

            if (targetGotHit)
            {
                InvokeOnDoingPhysicalDamage(physicalDamage);
                vfx.CreatOnHitVFX(target.transform, attackData.isCrit, element);
                sfx?.PlayAttackHit();
                Debug.Log("炸到了！");
            }
        }

        return targetGotHit;
    }
}
