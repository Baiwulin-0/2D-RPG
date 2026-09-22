using UnityEngine;

public class Enemy_idleState : Enemy_GroundedState
{
    public Enemy_idleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;

    }

    public override void Update()
    {
        base.Update();


        if(stateTimer<0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
