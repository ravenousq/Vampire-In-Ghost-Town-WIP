using System.Collections.Generic;
using Libs;
using UnityEngine;

[CreateAssetMenu(fileName = "Open Shop and Item", menuName = "Data/Dialogue Effect/Open Shop and Item Effect")]
public class OpenShopAndItem : DialogueEffect
{
    [SerializeField] private NPC npc;
    [SerializeField] private OldSerializableDictionary<string, int> possibleChoices;
    [SerializeField] private ItemData requiredItem;
    public Dictionary<string, int> choices;

    public override void Effect()
    {
        base.Effect();

        choices = possibleChoices.ToDictionary();

        manager.EnableDialogueUI(false);
        manager.EnableChoiceUI(true);
        manager.SetUpNPC(DialogueManager.instance.currentDialogue.GetComponent<NPC>());
        manager.SetUpChoices(choices, true, requiredItem);
        
    }
}
