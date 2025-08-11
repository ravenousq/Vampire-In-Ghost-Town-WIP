using UnityEngine;

[CreateAssetMenu(fileName = "Show Parry Indicator Effect", menuName = "Data/Charm Effect/Show Parry Indicator Effect")]
public class ShowParryIndicator : ItemEffect
{
    public override void Effect()
    {
        base.Effect();

        skills.ShowParryIndicator(true);
    }

    public override void Countereffect() => skills.ShowParryIndicator(false);
}
