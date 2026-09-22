using Unity.VisualScripting;
using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private Transform lastTarget;
    private float lastTimeWasInBattle;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        updateBattleTimer();

        if (player == null)
            player = enemy.GetPlayerReference();


        if (ShouldRetreat())
        {   //enemy.retreatVelocity.x
            rb.linearVelocity = new Vector2((enemy.retreatVelocity.x * enemy.activeSlowMultiplier) * -DirectionToPlayer(), enemy.retreatVelocity.y);
            Debug.Log($"retreatVelocity.x value: {enemy.retreatVelocity.x}");
            enemy.HandleFlip(DirectionToPlayer());
        }

    }
    
    public override void Update()
    {
        base.Update();
        

        if (enemy.PlayerDetected())
        {
            UpdateTargetIfNeeded();
            updateBattleTimer();
        }

        if (BattleTimeIsOver())
        {
            stateMachine.ChangeState(enemy.idleState);
        }

        if(WithinAttackRange() && enemy.PlayerDetected())
        {
            
            stateMachine.ChangeState(enemy.attackState);
            
        }
        else
        {
            enemy.SetVelocity(enemy.GetBattleMoveSpeed() * DirectionToPlayer(), rb.linearVelocity.y);
        }
    }
    private void UpdateTargetIfNeeded()
    {
        if (enemy.PlayerDetected() == false)
            return;

        Transform newTarget = enemy.PlayerDetected().transform;

        if(newTarget != lastTarget)
        {
            lastTarget = newTarget;
            player = newTarget;
        }
    }
    private void updateBattleTimer() =>lastTimeWasInBattle = Time.time;
    private bool BattleTimeIsOver()=>Time.time-lastTimeWasInBattle > enemy.battleTimeDuration;
    private bool WithinAttackRange() => DistanceToPlayer() < enemy.attackDistance;
    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;

    
    private float DistanceToPlayer()
    {
        if(player == null)
        {
            return float.MaxValue;
        }
        //与玩家间的距离（绝对值）
        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    private int DirectionToPlayer()
    {
        if (player == null)
        {
            return 0;
        }
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
