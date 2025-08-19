using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        player.canWallSlide = true;
        player.skills.dash.SwitchBlockade(false);

        player.EndKnockback();
    }

    public override void Update()
    {
        base.Update();

        if (Time.timeScale == 0)
            return;

        if (Input.GetKeyDown(KeyCode.E) && skills.concoction.CanUseSkill())
                stateMachine.ChangeState(player.heal);

        if(Input.GetKeyDown(KeyCode.C) && player.pickup.item)
            stateMachine.ChangeState(player.pickup);

        if(Input.GetKeyDown(KeyCode.Q) && SkillManager.instance.parry.CanUseSkill())
            stateMachine.ChangeState(player.parry);

        if(Input.GetKey(KeyCode.S) && !player.isBusy)
            stateMachine.ChangeState(player.crouch);

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(!SkillManager.instance.isSkillUnlocked("Faster Than The Flame") && stateMachine.current != player.reload && !player.CloseToEdge()) 
                stateMachine.ChangeState(player.quickstep);
        }
        
        if(Input.GetKeyDown(KeyCode.F) && player.skills.wanted.CanUseSkill())
            stateMachine.ChangeState(player.aimGun);
        

        if(Input.GetKeyDown(KeyCode.R) && skills.shoot.CanReload())
            stateMachine.ChangeState(player.reload);

        if((Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected()) || player.executeBuffer)
            stateMachine.ChangeState(player.jump);

        if(player.facingDir == xInput && player.IsWallDetected())
            xInput = 0;

        if(!player.IsGroundDetected())
        {
            player.allowCoyote = true;
            stateMachine.ChangeState(player.airborne);
        }

        if(Mathf.Abs(yInput) == 1 && player.ladderToClimb && !player.isBusy && !(player.IsGroundDetected() && !player.IsLadderDetected() && yInput == -1))
            stateMachine.ChangeState(player.climb);

        if(CanExecuteEnemy() && SkillManager.instance.isSkillUnlocked("Sweet Vendetta") && Input.GetKeyDown(KeyCode.Mouse0) && player.stateMachine.current != player.execute)
        {
            player.execute.target = player.enemyToExecute;
            player.enemyToExecute = null;
            stateMachine.ChangeState(player.execute);
            
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
        {
            if (skills.shoot.CanUseSkill())
                stateMachine.ChangeState(player.attack);
            else if (skills.shoot.CanReload() && Time.timeScale != 0)
                stateMachine.ChangeState(player.reload);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.FlagPosition();
    }

    private bool CanExecuteEnemy()
    {
        if(!player.enemyToExecute)
            return false;

        RaycastHit2D executionCheck = Physics2D.Raycast(player.transform.position, Vector2.right * player.facingDir, player.enemyToExecute.executionRange, player.whatIsEnemy);
        if(!executionCheck)
            return false;

        return executionCheck.collider.gameObject == player.enemyToExecute.gameObject;
    }
}
