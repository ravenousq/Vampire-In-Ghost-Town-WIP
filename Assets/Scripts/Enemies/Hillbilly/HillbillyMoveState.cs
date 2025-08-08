using UnityEngine;

public class HillbillyMoveState : HillbillyGroundedState
{
    public HillbillyMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hillbilly enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

    
        if (!enemy.IsGroundDetected())
            enemy.Flip();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected(false) && Time.time - enemy.primary.lastTimeAttacked > enemy.attackCooldown)
            stateMachine.ChangeState(enemy.primary);
    
        enemy.SetVelocity(enemy.movementSpeed * enemy.facingDir, rb.linearVelocityY);

        if (!enemy.IsGroundDetected() || enemy.IsWallDetected())
            stateMachine.ChangeState(enemy.idle);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
