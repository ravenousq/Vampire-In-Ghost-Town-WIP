
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCrouchState : PlayerState
{
    public PlayerCrouchState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.cd.size = new Vector2(player.cd.size.x, player.cd.size.y / 2);
        player.cd.offset = new Vector2(0, -.75f);
        LevelManager.instance.CrouchOffset(true, .5f);
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
            LevelManager.instance.CrouchOffset(false);

        player.ResetVelocity();

        if (!player.IsGroundDetected())
            player.stateMachine.ChangeState(player.airborne);

        player.FlipController(xInput);

        if (Input.GetKeyUp(KeyCode.S))
            stateMachine.ChangeState(player.idle);
    }

    public override void Exit()
    {
        base.Exit();

        player.cd.size = new Vector2(player.cd.size.x, player.cd.size.y * 2);
        player.cd.offset = Vector2.zero;
        LevelManager.instance.CrouchOffset(false, .25f);
    }
}
