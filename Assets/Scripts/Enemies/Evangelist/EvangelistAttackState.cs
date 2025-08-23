
public class EvangelistAttackState : EvangelistGroundedState
{
    public EvangelistAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Evangelist enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        enemy.CreateParryIndicator();
    }

    public override void Update()
    {
        base.Update();

        if(trigger)
            stateMachine.ChangeState(enemy.aggro);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
