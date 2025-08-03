using UnityEngine;

public class HillbillyDeathState : HillbillyGroundedState
{
    public HillbillyDeathState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hillbilly enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        enemy.PlayClip(Random.Range(0, 3));
    }

    public override void Update()
    {
        base.Update();

        if (trigger && !enemy.au.isPlaying)
            enemy.DestroyMe();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
