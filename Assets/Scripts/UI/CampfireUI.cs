using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampfireUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private TextMeshProUGUI[] buttons;
    [SerializeField] private Color highlightedColor;
    [SerializeField] private float HighlightedFontSize;
    [SerializeField] private ItemData manuscriptItem;

    [Space]

    [SerializeField] private BlessingsUI blessingsTab;
    private Color defaultColor;
    private float defaultFontSize;
    private Campfire campfire;
    public int index { get; private set; }
    private bool stoodUp = true;

    private void Awake()
    {
        defaultColor = buttons[index].color;
        defaultFontSize = buttons[index].fontSize;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            SwitchTo(index == 0 ? buttons.Length - 1 : index - 1);

        if (Input.GetKeyDown(KeyCode.S))
            SwitchTo(index == buttons.Length - 1 ? 0 : index + 1);

        if (Input.GetKeyDown(KeyCode.C))
            Remote();
    }

    private void SwitchTo(int newIndex)
    {
        buttons[index].color = defaultColor;
        buttons[index].fontSize = defaultFontSize;

        index = newIndex;

        buttons[index].color = highlightedColor;
        buttons[index].fontSize = HighlightedFontSize;
    }

    private void Remote()
    {
        switch (index)
        {
            case 0:
                Rest();
                break;
            case 1:
                Travel();
                break;
            case 2:
                Stargaze();
                break;
            case 3:
                Brew();
                break;
            case 4:
                StandUp();
                break;
        }
    }

    private void Rest()
    {
        UI.instance.SetUpConfirmationDialogue(
            "In current version, resting will erase all progress. Continue?",
            gameObject,
            "Yes",
            "No"
        );
        UI.instance.confirmationDialogue.onConfirm += ResetGame;
    }

    private void Travel()
    {
        UI.instance.SetUpConfirmationDialogue(
            "This feature will be available once the level structure is implemented.",
            gameObject,
            "Okay"
        );
    }

    private void Stargaze()
    {
        gameObject.SetActive(false);

        blessingsTab.transform.SetParent(transform.parent);
        blessingsTab.SetUpCampfire();
        blessingsTab.gameObject.SetActive(true);
    }

    private void Brew()
    {
        if (!SkillManager.instance.isSkillUnlocked("Astral Elixir"))
        {
            UI.instance.SetUpConfirmationDialogue(
                    "There is no concoction to brew.",
                    gameObject,
                    "Okay"
                );

            return;
        }

        if (Inventory.instance.HasItem(manuscriptItem))
        {
            UI.instance.SetUpConfirmationDialogue(
                "Use <color=#C07919>Alchemy Manuscript</color> and brew new concoctions?",
                gameObject,
                "Yes",
                "No"
            );

            UI.instance.confirmationDialogue.onConfirm += BrewConfirm;
        }
        else
        {
            UI.instance.SetUpConfirmationDialogue(
                "No <color=#C07919>Alchemy Manuscript</color> in inventory.",
                gameObject,
                "Okay"
            );
        }
    }

    private void BrewConfirm()
    {
        //Debug.Log("Brewing new concoctions...");
        SkillManager.instance.concoction.AddStack();
        Inventory.instance.RemoveItem(manuscriptItem);
        UI.instance.SetUpConfirmationDialogue(
                "Number of uses of <color=#C07919>Herbal Concoction</color> has increased.",
                gameObject,
                "Okay"
            );
    }

    private void StandUp()
    {
        campfire.StandUp();
        campfire = null;
        stoodUp = true;
    }

    public void SetUpCampfire(Campfire campfire) => this.campfire = campfire;


    private void OnEnable()
    {
        if (stoodUp)
        {
            stoodUp = false;
            SwitchTo(0);
        }
    }
    
    private void ResetGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

}
