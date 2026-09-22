using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }
    public override void Update()
    {
        base.Update();

        //检测到玩家
        //进入战斗状态
        if(enemy.PlayerDetected() == true)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
     
    
}
