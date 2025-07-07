using Unity.VisualScripting;
using UnityEngine;

public class PlayerRestState : PlayerState
{
    public PlayerRestState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        skills.ChangeLockOnAllSkills(true);
    }

    public override void Update()
    {
        base.Update();

        player.ResetVelocity();
    }

    public override void Exit()
    {
        skills.ChangeLockOnAllSkills(true);

        base.Exit();
    }

    public void StandUp() => stateMachine.ChangeState(player.idle);
}
