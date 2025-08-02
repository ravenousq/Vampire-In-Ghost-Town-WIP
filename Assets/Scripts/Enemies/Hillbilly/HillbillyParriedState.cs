using UnityEngine;

public class HillbillyParriedState : HillbillyGroundedState
{
    public HillbillyParriedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hillbilly enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if(trigger)
        {
            if(enemy.stats.isStunned)
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
