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
        enemy.anim.GetComponent<EnemyAnimationTriggers>().PlayFootstep();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(0, rb.linearVelocityY);

        if(stateTimer < 0)
            enemy.stats.Recover();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.anim.GetComponent<EnemyAnimationTriggers>().StopFootstep();
        enemy.AllowExecution(false);
        enemy.EnableStunFX(false);
    }
}
