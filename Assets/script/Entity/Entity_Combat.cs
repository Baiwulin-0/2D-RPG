using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    [SerializeField]protected BoxDetector boxDetection;
    [SerializeField]protected CircleDetector circleDetection;
    public event Action<float> OnDoingPhysicalDamage;

    protected Entity_SFX sfx;
    protected Entity_VFX vfx;
    protected Entity_Stats stats;

    public DamageScaleData basicAttackScale;


    protected virtual void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        sfx = GetComponent<Entity_SFX>();
        stats = GetComponent<Entity_Stats>();
    }
    public virtual void PerformAttack()
    {
        bool targetGotHit = false;
        foreach(var target in boxDetection.GetDetectedColliders())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                continue;

            AttackData attackData = stats.GetAttackData(basicAttackScale);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();


            float physicalDamage = attackData.phyiscalDamage;
            float elementalDamage = attackData.elementalDamage;
            ElementType element = attackData.element;

            targetGotHit = damagable.TakeDamage(physicalDamage,elementalDamage,element, transform);

            if (element != ElementType.None)
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);

            if(targetGotHit)
            {
                OnDoingPhysicalDamage?.Invoke(physicalDamage);
                vfx.CreatOnHitVFX(target.transform,attackData.isCrit,element);
                sfx?.PlayAttackHit(); 
            }
        }

        if (targetGotHit == false)
            sfx?.PlayAttackMiss();
    }

    //供子类调用的保护方法
    protected void InvokeOnDoingPhysicalDamage(float damage)
    {
        OnDoingPhysicalDamage?.Invoke(damage);
    }

}
