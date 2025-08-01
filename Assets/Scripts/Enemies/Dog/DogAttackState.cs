using UnityEngine;

public class DogAttackState : DogGroundedState
{
    Vector2 defaultVelocity;

    public DogAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Dog enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        defaultVelocity = rb.linearVelocity;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(defaultVelocity * 1.5f);

        if (trigger)
            stateMachine.ChangeState(enemy.turn);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
