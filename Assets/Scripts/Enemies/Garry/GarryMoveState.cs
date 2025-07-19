
using UnityEngine;

public class GarryMoveState : GarryGroundedState
{
    public GarryMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Garry enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public bool noZaryczNo;

    public override void Enter()
    {
        base.Enter();

        stateTimer = 2f;

        if (enemy.patrolRoute && !enemy.IsWallOnTheBackDetected())
            enemy.Flip();

        noZaryczNo = true;
    }

    public override void Update()
    {
        base.Update();

        if (noZaryczNo && Physics2D.OverlapCircle(enemy.transform.position, 10, enemy.whatIsPlayer))
        {
            noZaryczNo = false;
            enemy.InvokeName(nameof(enemy.NoZaryczNo), 4);
        }

        enemy.SetVelocity(enemy.movementSpeed * enemy.facingDir, rb.linearVelocityY);

        if (enemy.patrolRoute)
            if (Vector2.Distance(enemy.transform.position, enemy.patrolRoute.transform.position) > enemy.patrolRoute.bounds.size.x / 2 && stateTimer < 0)
                stateMachine.ChangeState(enemy.idle);

        if (!enemy.IsGroundDetected() || enemy.IsWallDetected())
            stateMachine.ChangeState(enemy.idle);

        if (enemy.IsPlayerDetected())
            stateMachine.ChangeState(enemy.aggro);
    }

    public override void Exit()
    {
        base.Exit();

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
            enemy.Flip();
    }

    
}
