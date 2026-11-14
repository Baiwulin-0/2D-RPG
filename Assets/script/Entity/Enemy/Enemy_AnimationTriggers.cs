using System;
using UnityEngine;

public class Enemy_AnimationTriggers : Entity_AnimationTriggers
{
    private Enemy enemy;
    private Enemy_VFX enemyVfx;
    [Header("Back&Front Speed")]
    [SerializeField] private float xBackSpeed = 1;
    [SerializeField]private float yBackSpeed = 1;
    [SerializeField] private float xFrontSpeed = 1;
    [SerializeField] private float yFrontSpeed = 1;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponentInParent<Enemy>();
        enemyVfx = GetComponentInParent<Enemy_VFX>();
    }

    private void EnableCounterWindow()
    {
        enemyVfx.EnableAttackAlert(true);
        enemy.EnableCounterWindow(true);
    }
    private void DisableCounterWindow()
    {
        enemyVfx.EnableAttackAlert(false);
        enemy.EnableCounterWindow(false);
    }

    private void GiveBackVelocity()
    {
        enemy.rb.linearVelocity = new Vector2(xBackSpeed*(-enemy.facingDir),yBackSpeed);
    }
    private void GiveFrontVelocity()
    {
        enemy.rb.linearVelocity = new Vector2(xFrontSpeed * (enemy.facingDir), yFrontSpeed);
    }

    private void EnterDeath()
    {
        enemy.EnterDeath();
    }

    
}
