using UnityEngine;

[CreateAssetMenu(fileName = "Chiming Items Effect", menuName = "Data/Charm Effect/Chiming Items Effect")]
public class ChimingItems : ItemEffect
{
    public override void Effect()
    {
        base.Effect();

        skills.AddChimingToItems();
    }

    public override void Countereffect() => skills.AddChimingToItems(false);
}
