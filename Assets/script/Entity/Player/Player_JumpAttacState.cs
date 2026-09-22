using UnityEngine;

public class Player_JumpAttacState : PlayerState
{
    private bool touchedGround;
    public Player_JumpAttacState(Player player, StateMachine stateMachine, string abimBoolName) : base(player, stateMachine, abimBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        touchedGround = false;

        player.SetVelocity(player.jumpAttacVelocity.x *player.facingDir, player.jumpAttacVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if(player.groundDetected && touchedGround == false)
        {
            touchedGround = true;
            anim.SetTrigger("jumpAttackTrigger");
            player.SetVelocity(0, rb.linearVelocity.y);
        }

        if(triggerCalled && player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
