using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        skills.ChangeLockOnAllSkills(true);
        rb.bodyType = RigidbodyType2D.Static;
    }

    public override void Update()
    {
        base.Update();

        if (trigger && !UI.instance.deathScreen.gameObject.activeSelf)
        {
            UI.instance.deathScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
