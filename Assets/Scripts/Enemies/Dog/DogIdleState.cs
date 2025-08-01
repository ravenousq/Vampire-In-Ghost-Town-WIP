using UnityEngine;

public class DogIdleState : DogGroundedState
{
    public DogIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Dog enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

        enemy.stats.OnDamaged += enemy.BecomeAggresive;
        stateTimer = enemy.attackCooldown;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() && enemy.IsGroundDetected() && stateTimer < 0)
            stateMachine.ChangeState(enemy.run);

        enemy.ResetVelocity();
    }

    public override void Exit()
    {
        enemy.stats.OnDamaged -= enemy.BecomeAggresive;
        base.Exit();
    }
}
