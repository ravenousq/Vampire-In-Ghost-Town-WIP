using UnityEngine;

public class HillbillyIdleState : HillbillyGroundedState
{
    public HillbillyIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hillbilly enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        //enemy.stats.OnDamaged += enemy.BecomeAggresive;
    }

    public override void Update()
    {
        base.Update();

        enemy.ResetVelocity();

        if (enemy.IsPlayerDetected(false) && Time.time - enemy.primary.lastTimeAttacked > enemy.attackCooldown)
            stateMachine.ChangeState(enemy.primary);

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.move);
    }

    public override void Exit()
    {
        base.Exit();

        if (enemy.IsWallDetected())
            enemy.Flip();
    }
}
