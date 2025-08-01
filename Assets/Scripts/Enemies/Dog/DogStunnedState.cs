using UnityEngine;

public class DogStunnedState : DogGroundedState
{
    public DogStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Dog enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stats.stunTime;
        enemy.AllowExecution(true);
        enemy.EnableStunFX(true);
    }

    public override void Update()
    {
        base.Update();

        enemy.ResetVelocity();

        if(stateTimer < 0)
            enemy.stats.Recover();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.AllowExecution(false);
        enemy.EnableStunFX(false);
    }
}
