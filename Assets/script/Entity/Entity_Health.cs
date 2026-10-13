using System;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamagable
{
    public event Action OnTakingDamage;


    private Slider healthBar;
    private Entity entity;
    private Entity_VFX entityVfx;
    private Entity_Stats entityStats;
    private Entity_DropManager dropManager;

    [SerializeField] protected float currentHealth;
    [Header("Health Regen")]
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private bool canRegenerateHealth = true;
    public float lastDamageTaken {  get; private set; }
    public bool isDead { get; private set; }
    protected bool canTakeDamage = true;

    [Header("On Damage Knockback")]
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private Vector2 onDamageKnockback = new Vector2(1.5f, 2.5f);
    [Header("On Heavy Damage Knockback")]
    [Range(0, 1)]
    [SerializeField] private float heavyDamageThreshold = .3f;//生命值一次受损超过30%，强击退；
    [SerializeField] private float heavyKnockbackDuration = 0.5f;
    [SerializeField] private Vector2 onHeavyDamageKnockback = new Vector2(7, 7);

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();
        dropManager = GetComponent<Entity_DropManager>();

        SetupHealth();

    }

    private void SetupHealth()
    {
        if (entityStats == null)
            return;

        currentHealth = entityStats != null ? entityStats.GetMaxHealth() : 1;
        UpdateHealthBar();
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead || canTakeDamage == false)
            return false;

        if (AttackEvaded())
        {
            return false;
        }

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;
        float mitigation = entityStats != null ? entityStats.GetArmorMitigation(armorReduction) : 0;
        float resistance = entityStats != null ? entityStats.GetElementalResistance(element) : 0;

        float physicalDamageTaken = damage * (1 - mitigation);
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        TakeKnockback(damageDealer, physicalDamageTaken);
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);

        lastDamageTaken = physicalDamageTaken + elementalDamageTaken;

        OnTakingDamage?.Invoke();
        return true;
    }

    public void SetCanTakeDamage(bool canTakeDamage) => this.canTakeDamage = canTakeDamage;


    private bool AttackEvaded()
    {
        if (entityStats == null)
            return false;
        else
            return UnityEngine.Random.Range(0, 100) < entityStats.GetEvasion();
    }

    private void RegenerateHealth()
    {
        if (canRegenerateHealth == false)
        {
            return;
        }

        float regenAmount = entityStats != null ? entityStats.resources.healthRegen.GetValue() : 0;
        IncreaseHealth(regenAmount);
    }

    public void IncreaseHealth(float healAmount)
    {
        if (isDead)
            return;

        float newHealth = currentHealth + healAmount;
        float maxHealth = entityStats != null ? entityStats.GetMaxHealth():1;

        if (newHealth > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth = newHealth;

        UpdateHealthBar();
    }

    public void ReduceHealth(float damage)
    {
        entityVfx?.PlayOnDamageVfx();
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
            Die();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;

        healthBar.value = currentHealth / entityStats.GetMaxHealth();
    }
    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        float duration = calculateDuration(finalDamage);
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);

        entity?.ReciveKnockback(knockback, duration);
    }

    protected virtual void Die()
    {
        isDead = true;
        Debug.Log("entity die!!!!");
        entity.EntityDeath();
        dropManager?.DropItems();
    }

    public float GetHealthPercent() => currentHealth / entityStats.GetMaxHealth();

    public void SetHealthToPercent(float percent)
    {
        currentHealth = entityStats.GetMaxHealth() * Mathf.Clamp01(percent);
        UpdateHealthBar();
    }
    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        //伤害量判断
        Vector2 knockback = IsHeavyDamage(damage) ? onHeavyDamageKnockback : onDamageKnockback;

        knockback.x = knockback.x * direction;

        return knockback;
    }

    //击退判定
    private float calculateDuration(float damage)
    {
        return IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;
    }
    private bool IsHeavyDamage(float damage)
    {
        if (entityStats == null)
            return false;
        else
            return damage / entityStats.GetMaxHealth() > heavyDamageThreshold;

    }
}
