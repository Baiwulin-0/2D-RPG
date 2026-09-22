using UnityEngine;

public class Player_DashState : PlayerState
{
    private float originalGravityScale;
    private int dashDir;
    public Player_DashState(Player player, StateMachine stateMachine, string abimBoolName) : base(player, stateMachine, abimBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skillManager.dash.OnStartEffect();
        player.vfx.DoImageEchoEffect(player.dashDuration);

        if(player.moveInput.x != 0)
        {
            dashDir = ((int)player.moveInput.x);
        }
        else dashDir = player.facingDir;
        stateTimer = player.dashDuration ;

        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

        player.health.SetCanTakeDamage(false);
    }

    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();

        player.SetVelocity(player.dashSpeed * dashDir, 0);

        if(stateTimer < 0 )
        {
            if (player.groundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else if(!player.groundDetected)
            {
                stateMachine.ChangeState(player.fallState);
            }
        }
        


    }

    public override void Exit()
    {
        base.Exit();

        skillManager.dash.OnEndEffect();

        player.health.SetCanTakeDamage(true);
        player.SetVelocity(rb.linearVelocity.x*0.5f,rb.linearVelocity.y);
        rb.gravityScale = originalGravityScale;
    }

    private void CancelDashIfNeeded()
    {
        if (player.wallDetected)
        {
            
            if (player.groundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else if (!player.groundDetected)
            {
                stateMachine.ChangeState(player.wallSlideState);
            }
        }
    }
}
