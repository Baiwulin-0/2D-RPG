using UnityEngine;

public class Enemy_NightBorn : Enemy, ICounterable
{
    public bool CanBeCountered { get => canBeStunned; }

    private bool canSelfDestruct = false;

    public Enemy_NightBornDestructState selfDestructState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_idleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        deadState = new Enemy_DeadState(this, stateMachine, "dead");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
        selfDestructState = new Enemy_NightBornDestructState(this, stateMachine, "selfDestruct");

    }

    protected override void Start()
    {
        base.Start();


        stateMachine.Initialize(idleState);
    }


    public void HandleCounter()
    {
        if (CanBeCountered == false)
        {
            return;
        }
        stateMachine.ChangeState(stunnedState);
    }

    public bool CanSelfDestruct()
    {
        return canSelfDestruct;
    }

    private void SetSelfDestruct(bool canSelfDestruct)
    {
        this.canSelfDestruct = canSelfDestruct;
    }

    public override void EntityDeath()
    {
        stateMachine.ChangeState(selfDestructState);
    }

}
