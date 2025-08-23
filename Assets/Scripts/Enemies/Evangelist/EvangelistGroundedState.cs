
public class EvangelistGroundedState : EnemyState
{
    protected Evangelist enemy;

    public EvangelistGroundedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Evangelist enemy) : base(enemyBase, stateMachine, animBoolName)
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
