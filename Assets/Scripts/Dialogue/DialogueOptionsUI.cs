using System.Collections.Generic;
using UnityEngine;

public enum ChoiceType
{
    Simple,
    Item,
    Complex
}
public class DialogueOptionsUI : MonoBehaviour
{
    private Dictionary<string, int> possibleChoices;
    [SerializeField] private ChoiceButtonUI buttonPrefab;
    private List<ChoiceButtonUI> buttons;
    private int highlightedButton = 0;
    private bool shop = false;
    private NPC npc;
    
    private void Update()
    {
        NavigateOptions();

        if (Input.GetKeyDown(KeyCode.C))
        {
            int chosenIndex = -1;

            if (possibleChoices.TryGetValue(buttons[highlightedButton].myText.text, out int value))
                chosenIndex = value;

            if(!buttons[highlightedButton].goToShop)
                DialogueManager.instance.NextLine(chosenIndex);
            else
            {
                UI.instance.SwitchShop(npc, chosenIndex);
                //UI.instance.SwitchBlockadeOnGameMenu();
            }

            foreach (Transform child in transform)
                Destroy(child.gameObject);

            gameObject.SetActive(false);
        }
    }


    private void NavigateOptions()
    {
        int nextButton;
        if (Input.GetKeyDown(KeyCode.S))
        {   
            nextButton = highlightedButton - 1 < 0 ? buttons.Count - 1 : highlightedButton - 1;
            
            if(!buttons[nextButton].canBeSelected)
                return;

            buttons[highlightedButton].Highlight();

            highlightedButton = nextButton;

            buttons[highlightedButton].Highlight();
        }

        else if (Input.GetKeyDown(KeyCode.W))
        {
            nextButton = (highlightedButton + 1) % buttons.Count;
            
            if(!buttons[nextButton].canBeSelected)
                return;

            buttons[highlightedButton].Highlight();

            highlightedButton = nextButton;

            buttons[highlightedButton].Highlight();
        }
    }

    public void SetUpNPC(NPC npc) => this.npc = npc;
    public void SetUpChoices(Dictionary<string, int> choices, ItemData requiredItem, bool shop)
    {
        possibleChoices = new Dictionary<string, int>(choices);

        buttons = new List<ChoiceButtonUI>();

        this.shop = shop;

        if(requiredItem == null)
            DoSimpleChoice();
        else
            GiveItemChoice(requiredItem);
    }


    private void DoSimpleChoice()
    {
        List<string> keys = new List<string>(possibleChoices.Keys);
        
        for (int i = 0; i < keys.Count; i++)
        {
            string key = keys[i];

            ChoiceButtonUI newButton = Instantiate(buttonPrefab);
            newButton.gameObject.transform.SetParent(transform);
            newButton.SetUp(key, null, i == 0 && shop);
            buttons.Add(newButton);

            if (i == 0)
            {
                newButton.Highlight();
                highlightedButton = i;
            }
        }
    }

    private void GiveItemChoice(ItemData requiredItem)
    {
        List<string> keys = new List<string>(possibleChoices.Keys);
        
        for (int i = 0; i < keys.Count; i++)
        {
            string key = keys[i];
            ChoiceButtonUI newButton = Instantiate(buttonPrefab);
            newButton.gameObject.transform.SetParent(transform);
            if(i == 0)
                newButton.SetUp(key, requiredItem);
            else
                newButton.SetUp(key, null, i == 1 && shop);
            buttons.Add(newButton);

            if (i == keys.Count - 1)
            {
                newButton.Highlight();
                highlightedButton = i;
            }
        }
    }
}
