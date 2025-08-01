using UnityEngine;

public class PlayerExitState : PlayerState
{
    private Transform objective;

    public PlayerExitState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.ResetVelocity();
        skills.ChangeLockOnAllSkills(true);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity((objective.position - player.transform.position).normalized * player.movementSpeed * .6f);

        if (Mathf.Abs(objective.position.y - player.transform.position.y) > 2)
        {
            player.anim.SetBool("move", false);
            player.anim.SetBool("jump", true);
        }
        else
        {
            player.anim.SetBool("jump", false);
            player.anim.SetBool("move", true);
        }

        if (Vector2.Distance(player.transform.position, objective.transform.position) < 2f)
            stateMachine.ChangeState(player.idle);
        
    }

    public override void Exit()
    {
        base.Exit();

        player.anim.SetBool("jump", false);
        skills.ChangeLockOnAllSkills(false);
    }

    public void SetObjective(Transform objective) => this.objective = objective;
}
