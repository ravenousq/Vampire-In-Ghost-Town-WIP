using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealState : PlayerState
{
    public PlayerHealState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.anim.SetInteger("facingDir", player.facingDir);

        skills.ChangeLockOnAllSkills(true);
        
        //player.healTorso.SetActive(true);
        //stateTimer = 1.5f;
    }

    public override void Update()
    {
        base.Update();

        //player.anim.SetInteger("facingDir", player.facingDir);

        // if(!player.isKnocked)
        //     rb.linearVelocity = new Vector2(xInput * skills.shoot.reloadMovementSpeed, 0);

        player.ResetVelocity();

        if (player.isKnocked)
            stateMachine.ChangeState(player.airborne);

        if (trigger)
                stateMachine.ChangeState(player.idle);
    }

    public override void Exit()
    {
        base.Exit();

        // player.anim.SetInteger("facingDir", player.facingDir);
        // player.healTorso.GetComponent<FX>().ResetSprite();
        // player.healTorso.SetActive(false);
        player.stats.Heal(skills.concoction.GetHeal());
        
        skills.ChangeLockOnAllSkills(false);
    }
}
