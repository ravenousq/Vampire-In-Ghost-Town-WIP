using UnityEngine;

public class DogGroundedState : EnemyState
{
    protected Dog enemy;
    public DogGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Dog enemy) : base(enemyBase, stateMachine, animBoolName)
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
