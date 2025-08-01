
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


[SelectionBase]
public class Player : Entity
{
    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idle { get; private set; }
    public PlayerMoveState move { get; private set; }
    public PlayerJumpState jump { get; private set; }
    public PlayerAirborneState airborne { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public  PlayerClimbState climb { get; private set; }
    public PlayerDashState dash { get; private set; }
    public PlayerPrimaryAttackState attack { get; private set; }
    public PlayerReloadState reload { get; private set; }
    public PlayerQuickstepState quickstep { get; private set; }
    public PlayerCrouchState crouch { get; private set; }
    public PlayerDiveState dive { get; private set; }
    public PlayerAimGunState aimGun { get; private set; }
    public PlayerParryState parry { get; private set; }
    public PlayerExecutionState execute { get; private set; }
    public PlayerHealState heal { get; private set; }
    public PlayerDialogueState dialogue { get; private set; }
    public PlayerImpactState impact { get; private set; }
    public PlayerEdgeState edge { get; private set; }
    public PlayerPickUpState pickup { get; private set; }
    public PlayerRestState rest { get; private set;  }
    public PlayerExitState exit { get; private set; }
    #endregion

    [Header("Movement")]
    public float movementSpeed;
    public float jumpForce;
    public float gravityScale;
    public float coyoteJumpWindow;
    public float bufferJumpWindow;
    public float wallSlideTime;
    public float wallSlideSpeed;
    public float climbSpeed;
    public float dashSpeed;
    public float dashDuration;
    public float quickstepSpeed;
    public float diveSpeed;
    
    [Header("Collision")]
    [SerializeField] public Transform edgeCheck;
    public float edgeCheckDistance;
    public Transform dialoguePoint { get; private set; } = null;
    public int dialogueFacingDir { get; private set; }

    [Header("Combat")]
    public LayerMask whatIsEnemy;
    public GameObject reloadTorso;
    public GameObject healTorso;


    [Header("Abilities & Stats")]
    public SkillManager skills;
    public PlayerStats stats;
    public Crosshair crosshair { get; private set; }
    public ReapersHalo halo { get; private set; }
    public Enemy enemyToExecute;
    public int executionDamage;
    public bool canSlowTime;
    public bool slowMotion { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private AfterImage afterImage;
    [SerializeField] private PerfectDashChecker dashCheckerPrefab; 
    [SerializeField] private DeathScreen deathScreen;

    [Header("FX")]
    public GameObject intoTheAbyssFX;
    public GameObject dashFX;
    public GameObject wallSlideFX;
    public GameObject airDashFX;
    public GameObject shootFX;
    public GameObject landFX;
    public GameObject jumpFX;
 
    #region Flags
    [HideInInspector] public bool playStartAnim = true;
    [HideInInspector] public bool allowCoyote;
    [HideInInspector] public bool executeBuffer;
    [HideInInspector] public bool canWallSlide = true;
    [HideInInspector] public bool attackTrigger;
    [HideInInspector] public bool floorParry;
    [HideInInspector] public bool isAimingHalo;
    [HideInInspector] public bool thirdAttack;
    private bool creatingAfterImage;

    [Header("Debug")]
    [SerializeField] private Garry garry;
    private float haloTimer;
    #endregion

    public BoxCollider2D ladderToClimb { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        stats = GetComponent<PlayerStats>();

        #region States Initialization
        stateMachine = new PlayerStateMachine();
        idle = new PlayerIdleState(this, stateMachine, "idle");
        move = new PlayerMoveState(this, stateMachine, "move");
        jump = new PlayerJumpState(this, stateMachine, "jump");
        airborne = new PlayerAirborneState(this, stateMachine, "jump");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "wallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "jump");
        climb = new PlayerClimbState(this, stateMachine, "climb");
        dash = new PlayerDashState(this, stateMachine, "dash");
        attack = new PlayerPrimaryAttackState(this, stateMachine, "attack");
        reload = new PlayerReloadState(this, stateMachine, "reload");
        quickstep = new PlayerQuickstepState(this, stateMachine, "quickstep");
        crouch = new PlayerCrouchState(this, stateMachine, "crouch");
        dive = new PlayerDiveState(this, stateMachine, "jump");
        aimGun = new PlayerAimGunState(this, stateMachine, "idle");
        parry = new PlayerParryState(this, stateMachine, "parry");
        execute = new PlayerExecutionState(this, stateMachine, "execution");
        heal = new PlayerHealState(this, stateMachine, "reload");
        dialogue = new PlayerDialogueState(this, stateMachine, "idle");
        impact = new PlayerImpactState(this, stateMachine, "impact");
        edge = new PlayerEdgeState(this, stateMachine, "edge");
        pickup = new PlayerPickUpState(this, stateMachine, "pickup");
        rest = new PlayerRestState(this, stateMachine, "rest");
        exit = new PlayerExitState(this, stateMachine, "move");
        #endregion
    }

    protected override void Start()
    {
        Time.timeScale = 1;

        base.Start();

        rb.gravityScale = gravityScale;

        skills = SkillManager.instance;

        stateMachine.Initialize(idle);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        skills.shoot.Reload();
    }

    protected override void Update()
    {
        if (stats.HP == 0)
            return;

        stateMachine.current.Update();

        if(Input.GetKeyDown(KeyCode.I))
            Instantiate(garry, transform.position + new Vector3(10f * facingDir, 0f, 0f), Quaternion.identity);


        if(!slowMotion && Time.timeScale < 1 && Time.timeScale != 0)
        {
            Time.timeScale += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
        }
    }

    private void LateUpdate() 
    {
        CheckForDashInput();
        CheckForHaloInput();
    }

    //private void ResetMoveStart() => playStartAnim = true;

    public void SlowDownTime()
    {
        if(canSlowTime)
            StartCoroutine(SlowDownTimeRoutine());
    }

    private IEnumerator SlowDownTimeRoutine()
    {
        slowMotion = true;
        Time.timeScale = 0.2f; 
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 

        yield return new WaitForSecondsRealtime(1f);

        slowMotion = false;
    }

    private void CheckForDashInput()
    {
        if(isKnocked || Time.timeScale == 0)
            return;

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(SkillManager.instance.dash.CanUseSkill())
            {
                stateMachine.ChangeState(dash);
                
                if (IsGroundDetected())
                    InstantiateFX(dashFX, groundCheck, new Vector3(0, .5f), new Vector3(0, facingDir == -1 ? 0 : 180, 0));
                else
                    Instantiate(airDashFX, transform.position, Quaternion.identity);

                AudioManager.instance.PlaySFX(11); 
                creatingAfterImage = true;
                InvokeRepeating(nameof(CreateAfterImage), 0, .01f);
                if(SkillManager.instance.isSkillUnlocked("Incense & Iron"))
                    Instantiate(dashCheckerPrefab, transform.position, Quaternion.identity);
            }
        }
    }

    private void CheckForHaloInput()
    {
        if(Time.timeScale == 0)
            return;

        haloTimer -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Mouse1) && !halo)
            haloTimer = 0.2f;

        if(Input.GetKeyUp(KeyCode.Mouse1) && haloTimer > 0 && skills.halo.CanUseSkill() && !halo)
        {
            skills.halo.SkipAiming();
            return;
        }

        if(haloTimer < 0 && Input.GetKey(KeyCode.Mouse1) && skills.halo.CanUseSkill() && !halo)
        {
            skills.halo.DotsActive(true);
            isAimingHalo = true;
        }

        if(!halo && Input.GetKey(KeyCode.Mouse1) && Input.GetKeyDown(KeyCode.Mouse0) && skills.isSkillUnlocked("Bless 'em With The Blade") && skills.isSkillUnlocked("Die By The Blade"))
        {
            skills.halo.EnableOrbiting();
            skills.halo.DotsActive(false);
            isAimingHalo = false;
        }

        if(isAimingHalo && Input.GetKeyUp(KeyCode.Mouse1) && !halo)
        {
            isAimingHalo = false;
            SkillManager.instance.halo.CreateHalo();
        }

        if(halo && Input.GetKeyDown(KeyCode.Mouse1))
            halo.GetComponent<ReapersHalo>().StopHalo();
    }

    public void PlayMiss() => AudioManager.instance.PlaySFX(Random.Range(27, 29));

    private void CreateAfterImage()
    {
        if (!creatingAfterImage)
        {
            CancelInvoke(nameof(CreateAfterImage));
            return;
        }

        AfterImage newAfterImage = Instantiate(afterImage, transform.position, Quaternion.identity);
        newAfterImage.SetUpSprite(sr.sprite, facingRight);
    }

    public void CancelAfterImage() => creatingAfterImage = false;

    public void ThirdAttack() => StartCoroutine(ThirdAttackRoutine());

    private IEnumerator ThirdAttackRoutine()
    {
        thirdAttack = true;

        yield return new WaitForSeconds(skills.shoot.attackWindow);

        thirdAttack = false;
    }

    public override void Stun()
    {

    }

    public void RestAtCampfire() => stateMachine.ChangeState(rest);
    

    public override void Die()
    {
        skills.ChangeLockOnAllSkills(true);

        deathScreen.gameObject.SetActive(true);
        deathScreen.ActivateDeathScreen();

        base.Die();

        canMove = false;
        canBeKnocked = false;
        stats.InvincibleFor(5f);

        Time.timeScale = 0;
    }

    public void DialogueStarted(Transform dialoguePoint, int dialogueFacingDir)
    {
        this.dialoguePoint = dialoguePoint;
        this.dialogueFacingDir = dialogueFacingDir;

        stateMachine.ChangeState(dialogue);
    }

    public void DialogueEnded()
    {
        dialoguePoint = null;
        stateMachine.ChangeState(idle);
    }

    #region Assigners    
    public void AssignNewHalo(ReapersHalo newHalo) => halo = newHalo;
    public void AssignCrosshair(Crosshair crosshair) => this.crosshair = crosshair;
    public void AssignExecutionTarget(Enemy enemyToExecute) => this.enemyToExecute = enemyToExecute;
    public void AssignItemToPickUp(ItemObject itemData) => pickup.PassItem(itemData);
    public void AssignLadder(BoxCollider2D ladder) => ladderToClimb = ladder;
    #endregion

    #region MoveThroughDoors
    public void MoveTowardsObjective(Transform objective)
    {
        exit.SetObjective(objective);
        stateMachine.ChangeState(exit);
    }
    #endregion

    #region Collisions
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Enemy>())
        {
            if (!isKnocked && stateMachine.current != dive)
            {
                stats.SwitchInvincibility(true);
                stats.TakeDamage(5);
                stats.LosePoise(10);
                Knockback(new Vector2(10, 5), other.gameObject.transform.position.x, .35f);
                stats.SwitchInvincibility(false);
            }
        }
    }
    
    public bool IsLadderDetected()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, groundCheckDistance, LayerMask.GetMask("Ladder"));

        if(colliders.Length > 0)
            return true;

        return false;
    }

    public bool CloseToEdge()
    {
        Vector2 edgeCheck = groundCheck.transform.position;
        int edgeToCheck = rb.linearVelocityX > 0 ? 1 : -1;

        edgeCheck += new Vector2(cd.size.x / 2 * edgeToCheck, 0);

        if (Physics2D.OverlapCircle(edgeCheck, .3f, whatIsGround))
            return false;

        return true;
    }

    public void NoCollisionsFor(float seconds) => StartCoroutine(NoCollisionsRoutine(seconds));

    private IEnumerator NoCollisionsRoutine(float seconds)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        yield return new WaitForSeconds(seconds);

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
    }
    #endregion

    public void WallSlideFX(bool enable)
    {
        if(enable)
            InvokeRepeating(nameof(CreateWallSlideFX), 0, .6f);
        else
            CancelInvoke(nameof(CreateWallSlideFX));
    }

    private void CreateWallSlideFX()
    {
        GameObject newFX = Instantiate(wallSlideFX, wallCheck.position + new Vector3(-.5f * facingDir,.2f), Quaternion.identity);
        AudioManager.instance.PlaySFX(8);
        newFX.transform.Rotate(0, facingDir == -1 ? 0 : 180, -270);
    }

    public void InstantiateFX(GameObject fx, Transform pos, Vector3 offset, Vector3 rotation) => Instantiate(fx, pos.position + offset, Quaternion.identity).transform.Rotate(rotation.x, rotation.y, rotation.z);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 3);

        if(SkillManager.instance)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector2(transform.position.x + 1.2f / 1.2f * facingDir, transform.position.y + 2.7f / 5), new Vector2(transform.position.x + (1.2f / 1.2f) + (skills.shoot.effectiveAttackRange * facingDir), transform.position.y + 2.7f / 5));
        }
        
        if(Application.isPlaying)
            Gizmos.DrawWireSphere(airborne.edge, .1f);
    }
}
