
using UnityEngine;


public class PlayerAnimationTrigger : MonoBehaviour
{
    FX fx => GetComponent<FX>();

    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void AnimationTriggers() => player.stateMachine.current.CallTrigger();

    public void DealDamage()
    {
        player.attackTrigger = true;
        AudioManager.instance.PlaySFX(Random.Range(16, 18));
        
        if (PlayerManager.instance.player.thirdAttack)
            AudioManager.instance.PlaySFX(Random.Range(16, 18));
    }

    public void EnableParry() => player.parry.canParry = true;

    public void DisableParry() => player.parry.canParry = false;

    public void PlayFootstep() => AudioManager.instance.PlaySFX(Random.Range(3, 6));

    public void PlayCreak() => AudioManager.instance.PlaySFX(Random.Range(9, 10));

    public void PlayCocking() => AudioManager.instance.PlaySFX(15);

    public void PlayReload() => AudioManager.instance.PlaySFX(14);
}
