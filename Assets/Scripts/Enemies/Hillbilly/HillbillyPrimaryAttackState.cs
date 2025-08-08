using UnityEngine;

public class HillbillyPrimaryAttackState : HillbillyGroundedState
{
    public HillbillyPrimaryAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Hillbilly enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public float lastTimeAttacked { get; private set; }

    private bool firstAttack = true;
    private int lastClip;
    private int clipToPlay = -1;

    public override void Enter()
    {
        base.Enter();

        enemy.stats.OnDamaged -= enemy.BecomeAggresive;

        if(clipToPlay == -1)
            clipToPlay = Random.Range(2, 7);

        while (clipToPlay == lastClip)
            clipToPlay = Random.Range(2, 7);

        lastClip = clipToPlay;

        //if (firstAttack)
        enemy.PlayClip(clipToPlay);
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
