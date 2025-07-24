using UnityEngine;

public class PlayerPickUpState : PlayerState
{
    public ItemObject item { get; private set; }

    public PlayerPickUpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if(trigger && item)
        {
            AudioManager.instance.PlaySFX(22, false);
            Inventory.instance.AddItem(item.item);
            LevelManager.instance.ItemFound(item);
            item.DestroyMe();
            item = null;
            trigger = false;
        }
        else if(trigger)
            stateMachine.ChangeState(player.idle);
    }

    public override void Exit()
    {
        base.Exit();

        skills.ChangeLockOnAllSkills(false);
    }
    
    public void PassItem(ItemObject item) => this.item = item;
}
