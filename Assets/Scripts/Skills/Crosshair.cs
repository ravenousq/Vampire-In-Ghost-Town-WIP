using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    Player player;
    private float crosshairSpeed;
    private float crosshairResistance;

    private Vector2 initialPosition;

    private Vector2 bounds;

    private float aimTimer;
    private CinemachineCamera cinemachine;
    private int ammo;
    private int physicalDamage;
    private int poiseDamage;
    private int defaultReward;
    private int reward;
    private int finalTargets;
    private float rewardMultiplier = 1.5f;
    private bool canIncreaseRewards;

    private List<Enemy> enemiesToAdd = new List<Enemy>();
    private List<Enemy> targets = new List<Enemy>();


    public void SetUp(float maxAimDuration, float crosshairSpeed, float crosshairResistance, CinemachineCamera cinemachine, int ammo, int physicalDamage, int poiseDamage, int defaultReward)
    {
        aimTimer = maxAimDuration;
        this.crosshairSpeed = crosshairSpeed;
        this.crosshairResistance = crosshairResistance;
        this.cinemachine = cinemachine;
        this.ammo = ammo;
        this.physicalDamage = physicalDamage;
        this.poiseDamage = poiseDamage;
        this.defaultReward = defaultReward;

        player = PlayerManager.instance.player;

        initialPosition = transform.position;
        
        player.stats.OnDamaged += StopAiming;

        Camera cam = Camera.main;

        Vector3 screenCenter = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 screenTopRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
;

        bounds = new Vector2(screenTopRight.x - screenCenter.x,screenTopRight.y - screenCenter.y);
        this.cinemachine.Follow = gameObject.transform;
    }

    private void Update()
    {
        aimTimer -= Time.deltaTime;

        if (aimTimer <= 0 || Input.GetKeyDown(KeyCode.F))
            Execute();

        MovementLogic();

        GatherTargets();
    }

    private void Execute()
    {
        cinemachine.Follow = PlayerManager.instance.player.transform;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;

        finalTargets = targets.Count;
        Debug.Log(finalTargets); 

        StartCoroutine(DamageTargets());
    }

    private IEnumerator DamageTargets()
    {
        if(targets.Count > 0)
        {
            Enemy enemy = targets[Random.Range(0, targets.Count)];
            enemy.stats.OnDie += IncreaseRewards;

            enemy.stats.TakeDamage(Mathf.RoundToInt(physicalDamage/finalTargets));
            enemy.stats.LosePoise(poiseDamage);
            enemy.Knockback(new Vector2(2, 2), enemy.gameObject.transform.position.x + (Random.Range(1, 10) > 5 ? -1 : 1), 2);
            enemy.mark.SetActive(false);
            enemy.stats.OnDie -= IncreaseRewards;
            targets.Remove(enemy);

            if(!SkillManager.instance.isSkillUnlocked("Amen & Attack"))
                SkillManager.instance.shoot.ModifyBullets(-1);
        }

        yield return new WaitForSeconds(.2f);

        if(targets.Count > 0)
            StartCoroutine(DamageTargets());
        else
        {
            AddCurrency();
            player.stats.OnDamaged -= StopAiming;
            Destroy(gameObject);
        }
    }

    private void IncreaseRewards() 
    {
        if(!canIncreaseRewards)
        {
            canIncreaseRewards = true;
            reward = defaultReward;
            return;
        }
        reward += Mathf.RoundToInt(reward * rewardMultiplier);
        rewardMultiplier += .5f;
    }

    public void AddCurrency()
    {
        PlayerManager.instance.AddCurrency(reward);
        UI.instance.ModifySouls(reward);
    }

    private void GatherTargets()
    {

        if (enemiesToAdd.Count > 0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(ammo == targets.Count && !SkillManager.instance.isSkillUnlocked("Amen & Attack"))
                return;
            
            targets.Add(enemiesToAdd[0]);
            enemiesToAdd[0].mark.SetActive(true);

            enemiesToAdd.RemoveAt(0);

            if (!SkillManager.instance.isSkillUnlocked("Ashen Rain"))
            {
                Execute();
                return;
            }
        }
    }

    private void MovementLogic()
    {
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        Vector2 distance = new Vector2(Mathf.Abs(initialPosition.x - transform.position.x) - 5, Mathf.Abs(initialPosition.y - transform.position.y) -5);

        Vector2 clamped = new Vector2(Mathf.InverseLerp(0, bounds.x, distance.x), Mathf.InverseLerp(0, bounds.y, distance.y));

        Vector2 finalSpeed = new Vector2(Mathf.Lerp(0, crosshairSpeed, 1 - clamped.x), Mathf.Lerp(0, crosshairSpeed, 1 - clamped.y));

        Vector2 adjustedSpeed = new Vector2(finalSpeed.x, finalSpeed.y);

        transform.Translate(mouseMovement * adjustedSpeed * Time.deltaTime);

        if(mouseMovement == Vector2.zero)
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, crosshairResistance * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Enemy enemy; 

        if (!other.GetComponent<Enemy>())
            return;
        else 
            enemy = other.GetComponent<Enemy>();   
        
        if (!targets.Contains(enemy) && !enemiesToAdd.Contains(enemy))
            enemiesToAdd.Add(enemy);
    }
    
    private void OnTriggerExit2D(Collider2D other) 
    {
        Enemy enemy;
        
        if (!other.GetComponent<Enemy>())
            return;
        else
            enemy = other.GetComponent<Enemy>();      

        if(enemiesToAdd.Contains(enemy))
            enemiesToAdd.Remove(enemy);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);    
    }

    public void StopAiming()
    {
        player.AssignCrosshair(null);
        StopAllCoroutines();
        cinemachine.Follow = player.transform;
        Destroy(gameObject);
    }
}
