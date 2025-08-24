using UnityEngine;

public class DogIdleState : DogGroundedState
{
    private float lastAggro;

    public DogIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Dog enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        enemy.stats.OnDamaged += enemy.BecomeAggresive;
        stateTimer = enemy.attackCooldown;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() && enemy.IsGroundDetected() && stateTimer < 0 && PlayerManager.instance.player.IsGroundDetected())
            stateMachine.ChangeState(enemy.run);

        enemy.SetVelocity(0, rb.linearVelocityY);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.OnDamaged -= enemy.BecomeAggresive;

        if(Time.time - lastAggro > 10)
            AudioManager.instance.PlaySFX(33);

        lastAggro = Time.time;
    }
}
