
using UnityEngine;

public class DogParriedState : DogGroundedState
{
    public DogParriedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Dog enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();

        if (trigger)
        {
            if (enemy.stats.isStunned)
                stateMachine.ChangeState(enemy.stun);
            else
                stateMachine.ChangeState(enemy.idle);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
