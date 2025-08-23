using UnityEngine;

public class EvangelistDeathState : EvangelistGroundedState
{
    public EvangelistDeathState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Evangelist enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.ResetVelocity();

        if (trigger)
            enemy.DestroyMe(); 
    }

    public override void Exit()
    {
        base.Exit();
    }
}
