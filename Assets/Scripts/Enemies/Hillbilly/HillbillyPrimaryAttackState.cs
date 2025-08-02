using UnityEngine;

public class HillbillyPrimaryAttackState : HillbillyGroundedState
{
    public HillbillyPrimaryAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hillbilly enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public float lastTimeAttacked { get; private set; }

    private bool firstAttack = true;

    public override void Enter()
    {
        base.Enter();

        enemy.stats.OnDamaged -= enemy.BecomeAggresive;
    }

    public override void Update()
    {
        base.Update();

        enemy.ResetVelocity();

        if (trigger && firstAttack)
            enemy.stateMachine.ChangeState(enemy.idle);
        else if (trigger && !firstAttack)
            enemy.stateMachine.ChangeState(enemy.combo);

    }

    public override void Exit()
    {
        base.Exit();

        lastTimeAttacked = Time.time;
        
        firstAttack = !firstAttack;
    }
}
