
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

    public void AnimationTrigger() => enemy.GetComponentInParent<Garry>().stateMachine.current.CallTrigger();

    public void OpenParryWindow() => enemy.OpenParryWindow();

    public void CloseParryWindow() => enemy.CloseParryWindow();

    public void PlayGarryAttack() => AudioManager.instance.PlaySFX(24);

    public void PlayFootstep()
    {
        au.pitch = Random.Range(.9f, 1.1f);

        float ambientRange = enemy.gameObject.GetComponent<Garry>().ambientRange;

        if (Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) < ambientRange)
        {
            au.volume = Mathf.Clamp(Mathf.InverseLerp(ambientRange, 0, Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position)), .2f, .9f);
            au.Play();
        }
        
    }
}
