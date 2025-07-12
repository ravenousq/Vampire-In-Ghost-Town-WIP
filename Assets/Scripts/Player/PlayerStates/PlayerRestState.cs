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
        UI.instance.LockGameMenu();
    }

    public override void Update()
    {
        base.Update();

        player.ResetVelocity();
    }

    public override void Exit()
    {
        skills.ChangeLockOnAllSkills(false);
        UI.instance.LockGameMenu();
        
        base.Exit();
    }

    public void StandUp() => stateMachine.ChangeState(player.idle);
}
