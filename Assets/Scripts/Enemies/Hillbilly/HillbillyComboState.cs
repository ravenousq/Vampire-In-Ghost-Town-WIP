using UnityEngine;

public class HillbillyComboState : HillbillyGroundedState
{
    public HillbillyComboState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hillbilly enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        enemy.CreateParryIndicator();

        enemy.stats.damage.AddModifier(3, enemy.stats);
    }

    public override void Update()
    {
        base.Update();

        enemy.ResetVelocity();

        if (trigger)
            enemy.stateMachine.ChangeState(enemy.idle);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.damage.RemoveModifier(3);
    }
}
