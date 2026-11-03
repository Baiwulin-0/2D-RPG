using System.Runtime.CompilerServices;
using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string abimBoolName) : base(player, stateMachine, abimBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();

        HandleWallSlide();

        if (input.player.jump.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.wallJumpState);
        }

        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            if (player.moveInput.x  != player.facingDir)
            {
                player.Flip();
            }
        }
        else if(!player.groundDetected && !player.wallDetected)
        {
            stateMachine.ChangeState(player.fallState);
        }


        
    }
    private void HandleWallSlide()
    {
        if(player.moveInput.y <0)
        {
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y);
        }
        else
        {
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y * player.wallSlideSlowMultiplier);
        }
    }
}
