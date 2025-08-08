using UnityEngine;

public class DogTurnState : DogGroundedState
{
    public DogTurnState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Dog enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    private bool turned;

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
        turned = false;
    }

    public override void Update()
    {
        base.Update();

        if ((stateTimer < 0 || !enemy.IsGroundDetected() || enemy.IsWallDetected()) && !turned)
        {
            if (playerOnRight() && enemy.facingDir == -1 || !playerOnRight() && enemy.facingDir == 1)
            {
                turned = true;
                enemy.anim.SetBool("run", false);
                enemy.anim.SetTrigger("turn");
            }
            else
                stateMachine.ChangeState(enemy.idle);
        }

        if (trigger)
            stateMachine.ChangeState(enemy.idle);

        if (!turned)
            enemy.SetVelocity(enemy.movementSpeed * enemy.facingDir, rb.linearVelocityY);
        else
            enemy.ResetVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    
        if(turned)
            enemy.Flip();
    }

    private bool playerOnRight() => PlayerManager.instance.player.transform.position.x > enemy.transform.position.x;

}
