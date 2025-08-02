using UnityEngine;

public class HillbillyGroundedState : EnemyState
{
    protected Hillbilly enemy;
    public HillbillyGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hillbilly enemy) : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
