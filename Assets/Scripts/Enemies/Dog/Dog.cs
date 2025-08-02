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

    [Space]
    private AudioSource au;

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

        au = GetComponent<AudioSource>();
    }

    [SerializeField] private float ambientRange;

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idle);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.current.Update();

        if (CanGrowl() && Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) < ambientRange)
            au.volume = Mathf.Clamp(Mathf.InverseLerp(ambientRange, 0, Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position)), 0, .9f);
        else
            au.volume = 0;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(death);
        AudioManager.instance.PlaySFX(35, false);
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
        if (stateMachine.current == run && stateMachine.current == attack)
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

        stateMachine.ChangeState(parried);
    }

    public override void GetExecuted()
    {
        base.GetExecuted();

        stateMachine.ChangeState(stun);
    }

    private bool CanGrowl() => stateMachine.current == idle;
}
