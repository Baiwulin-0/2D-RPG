using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string abimBoolName) : base(player, stateMachine, abimBoolName)
    {

    }

    public override void Update()
    {
        base.Update();

        if(input.player.jump.WasPressedThisFrame())
        {
           stateMachine.ChangeState(player.jumpState);
        }

        if(rb.linearVelocity.y<0 && !player.groundDetected)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (input.player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.basicAttackState);
        }

        if(input.player.CounterAttack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.counterAttackState);
        }

        if (input.player.RangeAttack.WasPressedThisFrame() && skillManager.swordThrow.CanUseSkill())
            stateMachine.ChangeState(player.swordThrowState);
    }


}
