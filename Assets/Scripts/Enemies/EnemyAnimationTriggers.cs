
using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    Enemy enemy;
    AudioSource au;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        au = GetComponent<AudioSource>();
    }

    public void DoDamage() => enemy.DoDamage();

    public void AnimationTrigger() => enemy.stateMachine.current.CallTrigger();

    public void OpenParryWindow() => enemy.OpenParryWindow();
    
    public void CloseParryWindow() => enemy.CloseParryWindow();

    public void PlayAttackSound() => AudioManager.instance.PlaySFX(enemy.attackFXIndex);

    public void PlayFootstep()
    {
        au.pitch = Random.Range(.9f, 1.1f);

        float ambientRange = enemy.ambientRange;

        AdjustDirectionalSound.Adjuster(au, PlayerManager.instance.player, ambientRange);

        au.Play();
    }

    public void StopFootstep() => au.Stop();
}
