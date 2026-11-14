using UnityEngine;

public class Enemy_NightBornDestructState : EnemyState
{
    public Enemy_NightBornDestructState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("进入自爆状态！");

        // 1. 锁死受击和移动，防止被打断
        enemy.SetVelocity(0, 0);
        enemy.EnableCounterWindow(false);
    }

    public override void Update()
    {
        // 这里不调用 base.Update() 是没问题的，因为自爆是个不可打断的单向过程
        // 你可以在这里写自爆的倒计时、屏幕震动等逻辑
    }

    public override void Exit()
    {
        base.Exit(); // 务必保留
        Debug.Log("退出自爆状态！被切到了: " + stateMachine.currentState.GetType().Name);
    }

    // 通过动画事件调用这个方法
    public void TriggerExplosionDamage()
    {
        Debug.Log("自爆伤害判定触发！");
        // 在这里执行范围伤害、特效，并销毁自身
        // Object.Destroy(enemy.gameObject);
    }
}