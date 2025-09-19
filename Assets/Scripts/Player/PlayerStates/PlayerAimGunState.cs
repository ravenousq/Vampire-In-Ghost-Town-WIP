using System.Collections.Generic;
using UnityEngine;
public class PlayerAimGunState : PlayerState
{
    public PlayerAimGunState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    private bool aiming;
    private bool shooting;
    private bool stopping;
    private int triggerCount;
    private int targets;

    public override void Enter()
    {
        base.Enter();

        rb.bodyType = RigidbodyType2D.Kinematic;
        aiming = true;
        shooting = false;
        stopping = false;
        trigger = false;
        triggerCount = 0;

        player.skills.ChangeLockOnAllSkills(true);
    }

    public override void Update()
    {
        base.Update();

        player.anim.SetBool("shootWanted", shooting);
        player.anim.SetBool("stopWanted", stopping);

        if (!player.isKnocked)
            player.ResetVelocity();

        if (trigger)
        {
            trigger = false;
            triggerCount++;

            if (shooting)
            {
                if (triggerCount % 2 == 1)
                {
                    player.crosshair?.DamageTarget();
                    targets--;
                }
                else
                {
                    if(targets > 0)
                        player.anim.SetTrigger("shootTrigger");
                }
            }
            else if (stopping)
            {
                if(player.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_AimExit"))
                    stateMachine.ChangeState(player.idle); 
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        rb.bodyType = RigidbodyType2D.Dynamic;

        player.skills.ChangeLockOnAllSkills(false);
    }

    public void StopAiming(bool interrupted = false)
    {
        aiming = false;
        shooting = false;

        if (interrupted)
        {
            player.anim.SetTrigger("interruptWanted");
            stateMachine.ChangeState(player.airborne);
        }
        else
            stopping = true;
    }

    public void Execute(int targets)
    {
        this.targets = targets;

        if (targets == 0)
        {
            skills.wanted.currentCrosshair.StopAiming();
            return;
        }

        aiming = false;
        shooting = true;
    }
}
