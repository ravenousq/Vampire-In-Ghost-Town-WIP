using UnityEngine;

public class PlayerClimbState : PlayerState
{
    private int sideToExit;
    private BoxCollider2D ladderCollider;

    public PlayerClimbState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player.skills.dash.SwitchBlockade(true);
        player.skills.halo.SwitchBlockade(true);
        
        stateTimer = 1f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        sideToExit = player.ladderToClimb.GetComponent<Ladder>().sideToExit;
        ladderCollider = player.ladderToClimb.GetComponent<BoxCollider2D>();

        player.stats.OnDamaged += FallOfTheLadder;
    }

    public override void Update()
    {
        base.Update();

        if (!player.ladderToClimb)
        {
            stateMachine.ChangeState(player.airborne);
            return;
        }

        if (xInput != 0 && stateTimer < 0)
        {
            stateMachine.ChangeState(player.airborne);
            return;
        }
         

        player.ResetVelocity();

        player.anim.speed = yInput == 0 ? 0 : 1;

        float targetX = player.ladderToClimb.transform.position.x - ladderCollider.bounds.size.x / 2;
        if (Mathf.Abs(player.transform.position.x - targetX) > 0.01f)
        {
            player.transform.position = Vector2.MoveTowards(
                player.transform.position,
                new Vector3(targetX, player.transform.position.y),
                13 * Time.deltaTime
            );
            return;
        }
            
        if(Input.GetKey(KeyCode.LeftShift) && yInput == -1)
        {
            player.anim.SetBool("ladderSlide", true);
            player.transform.position += new Vector3(0, yInput * player.climbSpeed * 2 * Time.deltaTime, 0);
        }
        else
        {
            player.anim.SetBool("ladderSlide", false);    
            player.transform.position += new Vector3(0, yInput * player.climbSpeed * Time.deltaTime, 0);
        }
        
        if(player.IsGroundDetected() && yInput == -1 && !player.IsLadderDetected())
            stateMachine.ChangeState(player.idle);
    }

    public override void Exit()
    {
        base.Exit();

        player.skills.dash.SwitchBlockade(false);
        player.skills.halo.SwitchBlockade(false);
        rb.bodyType = RigidbodyType2D.Dynamic;
        sideToExit = sideToExit == 0 ? player.facingDir : sideToExit;
        player.SetVelocity(3 * sideToExit, 3);
        player.BusyFor(.4f);
        player.anim.speed = 1;
        player.stats.OnDamaged -= FallOfTheLadder;
        ladderCollider = null;
    }

    private void FallOfTheLadder() => player.AssignLadder(null);
}
