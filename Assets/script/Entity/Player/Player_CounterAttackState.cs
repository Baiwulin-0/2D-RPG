using Unity.VisualScripting;
using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private Player_Combat combat;
    private bool counteredSombody;
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string abimBoolName) : base(player, stateMachine, abimBoolName)
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();
        
        counteredSombody = combat.CounterAttackPergormed();
        stateTimer = combat.GetCounterRecoveryDuration();

        anim.SetBool("counterAttackPerformed", counteredSombody);
    }

    public override void Update()
    {
        base.Update();

        

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);

        if(stateTimer < 0 && !counteredSombody)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
