using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image mainFill;
    [SerializeField] private Image damageFill;
    [SerializeField] private TMP_Text healthText;      
    [SerializeField] private TMP_Text bossNameText;    

    [Header("Show Range")]
    [SerializeField] private float showRange = 10f;
    [SerializeField] private float hideRangeOffset = 2f;
    [SerializeField] private float fadeSpeed = 4f;

    [Header("Damage Fill")]
    [SerializeField] private float damageDelay = 0.3f;
    [SerializeField] private float catchUpSpeed = 0.5f;

    [Header("Boss Search")]
    [SerializeField] private float searchInterval = 0.5f;

    private Player player;
    private Enemy boss;
    private Entity_Health bossHealth;

    private float targetPercent;
    private float delayedPercent;
    private float delayTimer;
    private float searchTimer;
    private float targetAlpha;

    private void Start()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        player = Player.instance;
        if (player == null)
            player = FindAnyObjectByType<Player>();

        searchTimer = 0f;
        TryFindBoss();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Update()
    {
        // Boss 还没找到 / 被销毁时，定时重新找
        if (boss == null)
        {
            searchTimer -= Time.deltaTime;

            if (searchTimer <= 0f)
            {
                searchTimer = searchInterval;
                TryFindBoss();
            }
        }

        UpdateVisibility();
        UpdateBars();
    }

    private void TryFindBoss()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            if (enemy.IsBoss())
            {
                SetBoss(enemy);
                return;
            }
        }
    }

    private void SetBoss(Enemy newBoss)
    {
        Unsubscribe();

        boss = newBoss;
        bossHealth = boss != null ? boss.health : null;

        if (bossHealth != null)
        {
            bossHealth.OnHealthUpdate += HandleHealthUpdate;

            targetPercent = bossHealth.GetHealthPercent();
            delayedPercent = targetPercent;
            delayTimer = 0f;

            if (mainFill != null) mainFill.fillAmount = targetPercent;
            if (damageFill != null) damageFill.fillAmount = delayedPercent;
        }

        if (bossNameText != null)
            bossNameText.text = boss != null ? boss.questTargetId : "";

        UpdateHealthText();
    }

    private void Unsubscribe()
    {
        if (bossHealth != null)
            bossHealth.OnHealthUpdate -= HandleHealthUpdate;
    }

    private void HandleHealthUpdate()
    {
        // 每次掉血，浅色条先停一下再追
        delayTimer = damageDelay;

        UpdateHealthText();
    }

    private void UpdateBars()
    {
        if (bossHealth == null || mainFill == null || damageFill == null)
            return;

        targetPercent = bossHealth.GetHealthPercent();

        // 主血条立即跟随
        mainFill.fillAmount = targetPercent;

        // 回血：两条一起涨，不延迟
        if (targetPercent > delayedPercent)
            delayedPercent = targetPercent;

        // 掉血：延迟后慢慢追上
        if (delayedPercent > targetPercent)
        {
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0f)
                delayedPercent = Mathf.MoveTowards(delayedPercent, targetPercent, catchUpSpeed * Time.deltaTime);
        }

        damageFill.fillAmount = delayedPercent;
    }

    private void UpdateVisibility()
    {
        bool shouldShow = false;

        if (boss != null && bossHealth != null && bossHealth.isDead == false && player != null)
        {
            float distance = Vector2.Distance(player.transform.position, boss.transform.position);
            float hideRange = showRange + hideRangeOffset;

            if (distance <= showRange)
                shouldShow = true;
            else if (distance <= hideRange && canvasGroup.alpha > 0.01f)
                shouldShow = true;   // 回滞：避免站在边界反复闪
        }

        targetAlpha = shouldShow ? 1f : 0f;
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
    }

    private void UpdateHealthText()
    {
        if (healthText == null || bossHealth == null || boss == null || boss.stats == null)
            return;

        float current = bossHealth.GetCurrentHealth();
        float max = boss.stats.GetMaxHealth();

        healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }
}