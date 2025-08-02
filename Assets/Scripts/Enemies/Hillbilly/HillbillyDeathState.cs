using UnityEngine;

public class HillbillyDeathState : HillbillyGroundedState
{
    public HillbillyDeathState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hillbilly enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
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
            enemy.DestroyMe();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
