using UnityEngine;

public class Player_SwordThrowState : PlayerState
{
    public Player_SwordThrowState(Player player, StateMachine stateMachine, string abimBoolName) : base(player, stateMachine, abimBoolName)
    {
    }
    Camera mainCamera;

    public override void Enter()
    {
        base.Enter();

        skillManager.swordThrow.EnableDots(true);

        if (mainCamera != Camera.main)
        {
            mainCamera = Camera.main;
        }
    }
    public override void Update()
    {
        base.Update();

        Vector2 dirToMouse = DirectionToMouse();

        player.SetVelocity(0, rb.linearVelocity.y);
        player.HandleFlip(dirToMouse.x);
        skillManager.swordThrow.PredictTrajectory(dirToMouse);

        if(input.player.Attack.WasPressedThisFrame())
        {
            anim.SetBool("swordThrowPerformed", true);

            skillManager.swordThrow.EnableDots(false);
            skillManager.swordThrow.Confirmtrajectory(dirToMouse);
        }

        if(input.player.RangeAttack.WasReleasedThisFrame() || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("swordThrowPerformed", false);
        skillManager.swordThrow.EnableDots(false);
       
    }

    private Vector2 DirectionToMouse()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(player.mousePosition);

        Vector2 direction = worldMousePosition - playerPosition;

        return direction.normalized;
    }
}
