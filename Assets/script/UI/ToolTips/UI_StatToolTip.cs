using System;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UI_StatToolTip : UI_ToolTip
{
    private Player_Stats playerStats;
    private TextMeshProUGUI statToolTipText;

    protected override void Awake()
    {
        base.Awake();
        playerStats = FindAnyObjectByType<Player_Stats>();
        statToolTipText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowToolTip(bool show,RectTransform targetRect,StatType statType)
    {
        base.ShowToolTip(show, targetRect);
        statToolTipText.text = GetStatTextByType(statType);
    }

    public string GetStatTextByType(StatType type)
    {
        switch(type)
        {
            // major stats
            case StatType.Strength:
                return "每点提供：+ 1 物伤.";
            case StatType.Agility:
                return "每点提供：" +
                    "\n+ 0.3% 暴击率." +
                    "\n+ 0.5%闪避率.";
            case StatType.Intelligence:
                return "每点提供：" +
                    "\n+ 0.5% 元素抵抗." +
                    "\n+ 1 基础元素伤害加成." +
                    "（仅对已持有元素生效）";
            case StatType.Vitality:
                return "每点提供：" +
                    "\n+ 5 最大生命值.";

            //offense stats
            case StatType.Damage:
                return "当前攻击时的物理伤害值.";
            case StatType.CritChance:
                return "当前暴击率.";
            case StatType.CritPower:
                return "当前暴击效果.";
            case StatType.ArmorReduction:
                return "当前盔甲穿透值.";
            case StatType.AttackSpeed:
                return "当前攻击速度.";

            // Defense stats
            case StatType.MaxHealth:
                return "当前你拥有的最大生命值.";
            case StatType.HealthRegen:
                return "自然恢复量，即每秒自动回血值.";
            case StatType.Evasion:
                return "躲避敌人攻击的机率." +
                    "\n最大值： 85 %.";
            case StatType.Armor:
                return "护甲值，你拥有的值越多，敌人对你造成的伤害越低." +
                    "\n 最多可减免： 85 %." +
                    "\n 当前伤害减免：" + playerStats.GetArmorMitigation(0) * 100 + " %.";

            //Elemental damage stats
            case StatType.IceDamage:
                return "冰属性伤害，使敌人减速.";
            case StatType.FireDamage:
                return "火属性伤害，使敌人燃烧.";
            case StatType.LightningDamage:
                return "雷属性伤害，累计伤害触发爆炸效果.";
            case StatType.ElementalDamage:
                return "元素伤害总和.";

            //Elemental resistance stats
            case StatType.IceResistance:
                return "减少你受到的冰属性伤害.";
            case StatType.FireResistance:
                return "减少你受到的火属性伤害.";
            case StatType.LightningResistance:
                return "减少你受到的雷属性伤害.";

            default:
                return "404???";
        }
    }
}
