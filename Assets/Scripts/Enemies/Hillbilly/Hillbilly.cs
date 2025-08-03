using UnityEngine;

public class Hillbilly : Enemy
{
    #region States
    public HillbillyIdleState idle { get; private set; }
    public HillbillyMoveState move { get; private set; }
    public HillbillyDeathState death { get; private set; }
    public HillbillyPrimaryAttackState primary { get; private set; }
    public HillbillyComboState combo { get; private set; }
    public HillbillyParriedState parried { get; private set; }
    public HillbillyStunnedState stun { get; private set; }
    #endregion

    public AudioSource au { get; private set; }
    protected override void Awake()
    {
        base.Awake();

        idle = new HillbillyIdleState(this, stateMachine, "idle", this);
        move = new HillbillyMoveState(this, stateMachine, "move", this);
        death = new HillbillyDeathState(this, stateMachine, "death", this);
        primary = new HillbillyPrimaryAttackState(this, stateMachine, "attack1", this);
        combo = new HillbillyComboState(this, stateMachine, "attack2", this);
        parried = new HillbillyParriedState(this, stateMachine, "parried", this);
        stun = new HillbillyStunnedState(this, stateMachine, "stun", this);

        au = GetComponent<AudioSource>();
    }

    [SerializeField] private AudioClip[] clips;

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & whatIsPlayer) != 0)
            BecomeAggresive();
    }

    public override void BecomeAggresive()
    {
        if (stateMachine.current == combo || stateMachine.current == stun)
            return;

        float playerPosX = PlayerManager.instance.player.transform.position.x;

        if ((playerPosX > transform.position.x && !facingRight) || (playerPosX < transform.position.x && facingRight))
        {
            stateMachine.ChangeState(idle);
            Flip();
        }
    }

    public override void Recover()
    {
        base.Recover();

        if (stateMachine.current == stun)
            stateMachine.ChangeState(idle);
    }

    public override void GetExecuted()
    {
        base.GetExecuted();

        stateMachine.ChangeState(stun);
    }

    public override void Parried()
    {
        base.Parried();

        stateMachine.ChangeState(parried);
    }

    public void PlayClip(int clip)
    {
        au.clip = clips[clip];
        au.Play();
    }

}
