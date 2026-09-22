using UnityEngine;

public enum SkillUpgradeType 
{
    None,

    //dash Tree
    Dash,
    Dash_CloneOnStart,
    Dash_CloneStartAndArrival,
    Dash_ShardOnShart,
    Dash_ShardOnStartAndArrival,

    //Shard Tree
    Shard,
    Shard_MoveToEnemy,
    Shard_Multicast,
    Shard_Teleport,
    Shard_TeleportHpRewind,

    //Sword Tree
    SwordThrow,
    SwordThrow_Spin,
    SwordThrow_Pierce,
    SwordThrow_Bounce,

    //Time Echo
    TimeEcho,
    TimeEcho_SingleAttack,
    TimeEcho_MultiAttack,
    TimeEcho_ChanceToDuplicate,

    TimeEcho_HealWisp,

    TimeEcho_CleanseWisp,
    TimeEcho_CooldownWisp,

    //Domain Expansion
    Domain_SlowingDown,
    Domain_EchoSpam,
    Domain_ShardSpam
}
