using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    #region Components
    public EnemyStateMachine stateMachine { get; private set; }
    public EnemyStats stats { get; private set; }
    public GameObject mark;
    #endregion

    [SerializeField] private bool isAMiniBoss;

    [Header("Movement")]
    public float movementSpeed;
    public float idleTime;
    public float aggroTime;

    [Header("Detection")]
    [SerializeField] protected float aggroRange;
    public LayerMask whatIsPlayer;
    public float executionRange;

    [Header("Combat")]
    public int poiseDamage;
    public float attackDistance;
    public float attackCooldown;
    public Vector2 attackKnockback;
    public Transform attackPoint;
    public bool canBeStunned { get; private set; }
    public bool canBeExecuted { get; private set; }

    [Header("Drop")]
    [SerializeField] private ItemObject itemPrefab;
    [SerializeField] private ItemData drop;
    [SerializeField] private Transform itemDropPosition;

    [Header("FX")]
    [SerializeField] protected GameObject bloodFX;
    [SerializeField] protected GameObject stunFX;
    public int attackFXIndex;
    private bool isBeingDestroyed;
    public float ambientRange;
    protected bool cdIsTrigger;

    Player player;

    public System.Action onDamaged;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        cdIsTrigger = cd.isTrigger;
    }

    protected override void Start()
    {
        base.Start();

        player = PlayerManager.instance.player;

        stats = GetComponent<EnemyStats>();
        stats.OnDamaged += BloodFX;
        SwitchKnockability();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.current.Update();

        if (canBeExecuted)
        {
            if (Physics2D.OverlapCircle(transform.position, executionRange, whatIsPlayer))
                ControlExecution();
            else
                player.enemyToExecute = player.enemyToExecute == this ? null : player.enemyToExecute;
        }

        if (isBeingDestroyed)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.MoveTowards(sr.color.a, 0, Time.deltaTime));

            if (sr.color.a <= 0)
                Destroy(gameObject);
        }
    }

    public virtual void DoDamage()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(attackPoint.position, attackDistance);

        foreach(var hit in targets)
        {
            if(hit.GetComponent<Player>())
            {
                PlayerStats playerTarget = hit.GetComponent<PlayerStats>();
                stats.DoDamage(playerTarget, attackKnockback, .5f, poiseDamage);
            }

            if(hit.GetComponent<PerfectDashChecker>())
            {
                //Debug.Log("Perfect Dash");

                player.SlowDownTime();

                Destroy(hit.gameObject);

                int currentBullets = player.skills.shoot.currentAmmo;

                int bulletsToRefill = Mathf.RoundToInt((12 - currentBullets)/ 2);
                player.skills.shoot.ModifyBullets(bulletsToRefill);
            }
        }
    }

    public override void Die()
    {
        base.Die();

        cd.isTrigger = true;
        rb.bodyType = RigidbodyType2D.Static;

        stats.OnDamaged -= BloodFX;

        if(drop)
            Instantiate(itemPrefab, itemDropPosition.position, Quaternion.identity).SetUpItem(drop, true);

        if (isAMiniBoss)
            LevelManager.instance.MiniBossDefeated(this);
    }

    public override void Flip()
    {
        StartCoroutine(StopMovingFor(.6f));

        base.Flip();
    }

    public override void Stun()
    {
        base.Stun();

        cd.isTrigger = true;
        rb.bodyType = RigidbodyType2D.Static;
    }

    public void EnableStunFX(bool enable) => stunFX.SetActive(enable);

    public virtual void Recover()
    {
        if (stats.HP <= 0)
            return;

        cd.isTrigger = cdIsTrigger;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    protected IEnumerator StopMovingFor(float seconds)
    {
        canMove = false;

        yield return new WaitForSeconds(seconds);

        canMove = true;
    }

    #region Parry
    public void OpenParryWindow() => canBeStunned = true;

    public virtual void Parried() => CloseParryWindow();

    public void CloseParryWindow() => canBeStunned = false;
    #endregion

    #region Execution
    public virtual void GetExecuted()
    {
        AllowExecution(false);
        FlipController(PlayerManager.instance.player.transform.position.x - transform.position.x);
    }

    public virtual void AllowExecution(bool allow) 
    {
        canBeExecuted = allow;
        if(player.enemyToExecute == this && !allow)
            player.AssignExecutionTarget(null);
    }

    protected virtual void ControlExecution()
    {
        if(player.enemyToExecute != null && player.stateMachine.current != player.dash)
            return;
        
        if(player.stateMachine.current == player.dash && SkillManager.instance.isSkillUnlocked("Dance Macabre"))
        {
            player.enemyToExecute = null;
            player.NoCollisionsFor(.5f);
            AllowExecution(false);

            if(stats.HP <= stats.health.GetValue() * .3f)
            {
                stats.Die();
                return;
            }

            stats.TakeDamage(player.executionDamage);
            stats.Invoke(nameof(Recover), .5f);
        }
        else
            player.AssignExecutionTarget(this);
    }

    #endregion

    #region Detection 
    public virtual bool IsPlayerDetected(bool detectBack = true)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right * facingDir, aggroRange);
        RaycastHit2D[] hitsBack = Physics2D.RaycastAll(transform.position, Vector2.left * facingDir, aggroRange/2);

        for(int i = 0; i < hits.Length; i++)
        {
            if(((1<<hits[i].collider.gameObject.layer) & whatIsGround) != 0)
                return false;

            if(((1<<hits[i].collider.gameObject.layer) & whatIsPlayer) != 0)
                return true;
        }

        if (detectBack)
            for (int i = 0; i < hitsBack.Length; i++)
            {
                if (((1 << hitsBack[i].collider.gameObject.layer) & whatIsGround) != 0)
                    return false;

                if (((1 << hitsBack[i].collider.gameObject.layer) & whatIsPlayer) != 0)
                    return true;
            }
        

        return false;
    } 

    public virtual void BecomeAggresive()
    {
        if(IsAggresive() || canBeExecuted)
            return;
    }

    public virtual bool  IsAggresive()
    {
        return false;
    }

    #endregion

    private void BloodFX()
    {
        float yOffset = cd.size.y/3;
        AudioManager.instance.PlaySFX(Random.Range(19, 21));
        Instantiate(bloodFX, new Vector3(
        transform.position.x - (player.transform.position.x > transform.position.x ? cd.size.x/8 : -cd.size.x/8),
        transform.position.y + Random.Range(-yOffset, yOffset)),
        Quaternion.Euler(0, player.transform.position.x > transform.position.x && facingDir != player.facingDir ? 180 : 0, Random.Range(-30, 30))).transform.parent = transform;
    }

    public bool IsAMiniBoss() => isAMiniBoss;

    public void DestroyMe()
    {
        isBeingDestroyed = true;
    }
        

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        //Gizmos.color = Color.black;
        //Gizmos.DrawLine(transform.position - new Vector3(0, .1f), new Vector3(transform.position.x + (aggroRange * facingDir), transform.position.y - .1f));
        //Gizmos.DrawLine(transform.position - new Vector3(0, .1f), new Vector3(transform.position.x - (aggroRange/2 * facingDir), transform.position.y - .1f));

        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
    }
}
