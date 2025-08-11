using UnityEngine;

[CreateAssetMenu(fileName = "Show Healthbars Effect", menuName = "Data/Charm Effect/Show Healthbars Effect")]
public class ShowHealthbars : ItemEffect
{
    public override void Effect()
    {
        base.Effect();

        skills.ShowHealthbars(true);
    }

    public override void Countereffect() => skills.ShowHealthbars(false);
}
