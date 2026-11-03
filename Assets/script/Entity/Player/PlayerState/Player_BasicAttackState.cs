using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer;
    private float lastTimeAttacked;

    private const int FirstComboIndex = 1;
    private int attackDir;
    private int comboLimit = 3;
    private int comboIndex = 1;

    private bool comboAttackQueued;
    public Player_BasicAttackState(Player player, StateMachine stateMachine, string abimBoolName) : base(player, stateMachine, abimBoolName)
    {
        if(comboLimit != player.attackVelocity.Length)
        {
            Debug.LogWarning("已同步连击段数限制");
            comboLimit = player.attackVelocity.Length;
        }
    }


    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndexIfNeeded();
        SyncAttackSpeed();

        if(player.moveInput.x != 0)
        {
            attackDir = ((int)player.moveInput.x);
        }
        else
        {
            attackDir = player.facingDir;
        }
        anim.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();

    }

    

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.player.Attack.WasPressedThisFrame())
        {
            QueueNextAttack();
        }

        if(triggerCalled)
        {
            HandleStateExit();
        }
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttacked = Time.time;
    }

    private void QueueNextAttack()
    {
        if(comboIndex < comboLimit)
        {
            comboAttackQueued = true;
        }
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;
        if (attackVelocityTimer < 0)
        {
            player.SetVelocity(0, rb.linearVelocity.y);
        }
    }
    private void ApplyAttackVelocity()
    {
        attackVelocityTimer = player.attackVelocityDuration;

        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
        

    }

    private void ResetComboIndexIfNeeded()
    {
        if (comboIndex > comboLimit || Time.time -lastTimeAttacked > player.comboResetTime)
        {
            comboIndex = FirstComboIndex;
        }
    }
}
