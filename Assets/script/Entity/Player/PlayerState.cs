using System.Net.Http.Headers;
using UnityEngine;

public class PlayerState:EntityState
{
    protected Player player;
    protected PlayerInputSet input;
    protected Player_SkillManager skillManager;
  

    public PlayerState(Player player,StateMachine stateMachine,string abimBoolName):base(stateMachine,abimBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        input = player.input;
        stats = player.stats;
        skillManager = player.skillManager;
    }
    public override void Update()
    {
        base.Update();

        if(input.player.Dash.WasPressedThisFrame() && canDash())
        {
            skillManager.dash.SetSkillOnCooldown();
            stateMachine.ChangeState(player.dashState);
        }

        if(input.player.UltimateSpell.WasPressedThisFrame() && skillManager.domainExpansion.CanUseSkill())
        {
            if (skillManager.domainExpansion.InstantDomain())
                skillManager.domainExpansion.CreateDomain();
            else
                stateMachine.ChangeState(player.domainExpansionState);

            skillManager.domainExpansion.SetSkillOnCooldown();
        }
    }

    private bool canDash()
    {
        if (skillManager.dash.CanUseSkill() == false)
            return false;

        if(stateMachine.currentState == player.dashState)   
            return false;

        if (stateMachine.currentState == player.dashState || stateMachine.currentState == player.domainExpansionState)
            return false;

        return true;
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }


}
