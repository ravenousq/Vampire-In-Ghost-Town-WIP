using UnityEngine;

public class DogRunState : DogGroundedState
{
    private Player player;
    private const float MIN_RUN_TIME = 1f;
    private float runChecker;
    private float runDirection;

    public DogRunState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Dog enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player;
        stateTimer = enemy.aggroTime;
        runChecker = MIN_RUN_TIME;
        runDirection = PlayerOnRight();

        enemy.anim.GetComponent<EnemyAnimationTriggers>().PlayFootstep();
    }

    public override void Update()
    {
        base.Update();
        runChecker -= Time.deltaTime;

        //Debug.Log(runChecker);

        if (!enemy.IsGroundDetected() || stateTimer < 0 || enemy.IsWallDetected())
            stateMachine.ChangeState(enemy.idle);

        if (runChecker >= 0)
            enemy.SetVelocity(enemy.movementSpeed * runDirection, rb.linearVelocityY);
        else
        {
            if (PlayerOnRight() != runDirection)
                stateMachine.ChangeState(enemy.turn);

            if (Mathf.Abs(player.transform.position.x - enemy.attackPoint.transform.position.x) > enemy.attackDistance)
                enemy.SetVelocity(enemy.movementSpeed * runDirection, rb.linearVelocityY);
            else
                stateMachine.ChangeState(enemy.attack);
        }

    }

    public override void Exit()
    {
        base.Exit();

        enemy.anim.GetComponent<EnemyAnimationTriggers>().StopFootstep();

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
            enemy.Flip();
    }
    
    private int PlayerOnRight() => player.transform.position.x > enemy.transform.position.x ? 1 : -1;

}

