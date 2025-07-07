using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    [SerializeField] protected string primarySkillName;
    protected float cooldownTimer;
    protected Player player;
    public bool isBlocked { get; private set; }

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update() 
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if(cooldownTimer <= 0 && SkillManager.instance.isSkillUnlocked(primarySkillName) && !isBlocked && Time.timeScale > 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        if(cooldownTimer > 0 && SkillManager.instance.debugging)
            Debug.Log("Skill on cooldown");

        if(!SkillManager.instance.isSkillUnlocked(primarySkillName) && SkillManager.instance.debugging)
            Debug.Log("Skill not unlocked");

        if(isBlocked && SkillManager.instance.debugging)
            Debug.Log("Skill is blocked from usage");

        return false;
    }   

    public virtual void UseSkill()
    {
        //Set up 
    }

    protected virtual Transform FindClosestEnemy(Transform checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 500);

        float closest = float.MaxValue;

        Transform closestTarget = null;

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>())
            {
                float distance = Vector2.Distance(checkTransform.position, hit.transform.position);

                if(distance < closest)
                {
                    closest = distance;
                    closestTarget = hit.transform;
                }
            }
        }

        return closestTarget;
    }

    public virtual void RenableSkill()
    {
        cooldownTimer = 0;
        if(isBlocked)
            SwitchBlockade(false);
    }

    public virtual void AddCooldown(float cooldown) => cooldownTimer = cooldown;

    public  virtual void SwitchBlockade(bool blockade) => isBlocked = blockade;

    public virtual void ModifyCooldown(float seconds) => cooldown += seconds;
    public float GetCooldown() => cooldown;
    
}
