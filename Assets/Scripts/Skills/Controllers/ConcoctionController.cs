using UnityEngine;

public class ConcoctionController : SkillController, ISaveManager
{
    [SerializeField] private int maxConcoctionStacks = 3;
    public int currentConcoctionStacks;
    [Range(0f, 1f)]
    [SerializeField] private float percentage;

    protected override void Start()
    {
        base.Start();

        if(!SaveManager.instance.HasSavedData())
            currentConcoctionStacks = maxConcoctionStacks;
    }

    public override bool CanUseSkill()
    {
        if (base.CanUseSkill())
        {
            if (currentConcoctionStacks > 0)
            {
                currentConcoctionStacks--;
                return true;
            }
        }

        return false;
    }

    public int GetHeal() => Mathf.RoundToInt(player.stats.health.GetValue() * percentage);
    public void ModifyPercentage(float percentage) => this.percentage += percentage;
    public void AddStack(bool refill = true)
    {
        maxConcoctionStacks++;
        if(refill)
            currentConcoctionStacks = maxConcoctionStacks;

        if(UI.instance.concoctionUI.gameObject.activeSelf)
            UI.instance.concoctionUI.UpdateConcoctionStacks();
    }

    public void LoadData(GameData data)
    {
        if (!data.usedDoor)
            currentConcoctionStacks = maxConcoctionStacks;
        else
            currentConcoctionStacks = data.currentConcoctionStacks;

        for (int i = 0; i < data.concoctionStacks - maxConcoctionStacks; i++)
                AddStack(false);
    }

    public void SaveData(ref GameData data)
    {
        data.concoctionStacks = maxConcoctionStacks;
        data.currentConcoctionStacks = currentConcoctionStacks;
    }
}
