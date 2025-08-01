using UnityEngine;

public class Dog : Enemy
{
    #region States
    public DogIdleState idle { get; private set; }
    public DogRunState run { get; private set; }
    public DogAttackState attack { get; private set; }
    public DogTurnState turn { get; private set; }
    public DogParriedState parried { get; private set; }
    public DogStunnedState stun { get; private set; }
    public DogDeathState death { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idle = new DogIdleState(this, stateMachine, "idle", this);
        run = new DogRunState(this, stateMachine, "run", this);
        attack = new DogAttackState(this, stateMachine, "attack", this);
        turn = new DogTurnState(this, stateMachine, "run", this);
        parried = new DogParriedState(this, stateMachine, "parried", this);
        stun = new DogStunnedState(this, stateMachine, "idle", this);
        death = new DogDeathState(this, stateMachine, "death", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idle);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.current.Update();
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(death);
    }

    public override void Stun()
    {
        base.Stun();

        stateMachine.ChangeState(stun);
    }

    public override void Recover()
    {
        base.Recover();

        if (stateMachine.current == stun)
            stateMachine.ChangeState(turn);
    }

    public override bool IsAggresive()
    {
        if(stateMachine.current == run /*|| stateMachine.current == attack*/)
            return true;

        return false;
    }

    public override void BecomeAggresive()
    {
        base.BecomeAggresive();

        if (IsAggresive() || stateMachine.current == stun)
            return;

        stateMachine.ChangeState(run);
    }
    
     public override void Parried()
    {
        base.Parried();

        if (stats.isStunned)
            Stun();
        else
            stateMachine.ChangeState(parried);
    }

    public override void GetExecuted()
    {
        base.GetExecuted();

        stateMachine.ChangeState(stun);
    }
}
