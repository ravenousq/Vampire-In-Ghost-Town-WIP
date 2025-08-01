using UnityEngine;

public class DogRunState : DogGroundedState
{
    Player player;
    private float randomizedDistance;

    public DogRunState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Dog enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {

    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player;
        stateTimer = enemy.aggroTime;
    }

    public override void Update()
    {
        base.Update();

        if (!enemy.IsGroundDetected() || stateTimer < 0 || enemy.IsWallDetected())
            stateMachine.ChangeState(enemy.idle);

        if (Mathf.Abs(player.transform.position.x - enemy.attackPoint.transform.position.x) > enemy.attackDistance)
            enemy.SetVelocity(enemy.movementSpeed * playerOnRight(), rb.linearVelocityY);
        else
            stateMachine.ChangeState(enemy.attack);
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    private int playerOnRight() => player.transform.position.x > enemy.transform.position.x ? 1 : -1;

}

