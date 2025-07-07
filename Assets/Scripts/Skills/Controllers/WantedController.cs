using Unity.Cinemachine;

using UnityEngine;

public class WantedController : SkillController
{
    [Header("Wanted Dead")]
    [SerializeField] private float maxAimDuration;
    [SerializeField] private float ashenRainDuration;
    [SerializeField] private float crosshairSpeed;
    [SerializeField] private float crosshairResistance;

    [SerializeField] private int physicalDamage;
    [SerializeField] private int poiseDamage;
    [SerializeField] private int defaultReward;

    [SerializeField] private Crosshair crosshairPrefab;
    [SerializeField] private CinemachineCamera cinemachine;
    private int currentAmmo;

    private Crosshair currentCrosshair = null;

    public float GetMaxDuration() 
    {
        if(SkillManager.instance.isSkillUnlocked("Ashen Rain"))
            return ashenRainDuration;
        else    
            return maxAimDuration;
    }

    public override bool CanUseSkill()
    {
        if(player.stateMachine.current == player.wallSlide && !SkillManager.instance.isSkillUnlocked("Heretic Hunter"))
            return false;

        if(player.skills.shoot.currentAmmo == 0 && !SkillManager.instance.isSkillUnlocked("Amen & Attack"))
            return false;

        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        currentAmmo = player.skills.shoot.currentAmmo;
        
        if(currentCrosshair)
            Destroy(currentCrosshair);

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 10f); 
        Vector3 center = Camera.main.ScreenToWorldPoint(screenCenter);

        Crosshair newCrosshair = Instantiate(crosshairPrefab, center, Quaternion.identity);

        currentCrosshair = newCrosshair;


        newCrosshair.SetUp(SkillManager.instance.isSkillUnlocked("Ashen Rain") ? ashenRainDuration : maxAimDuration, crosshairSpeed, crosshairResistance, cinemachine, currentAmmo, physicalDamage, poiseDamage, defaultReward);

        PlayerManager.instance.player.AssignCrosshair(newCrosshair);
    }
}
