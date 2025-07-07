using TMPro;
using UnityEngine;

public class CampfireUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private TextMeshProUGUI[] buttons;
    [SerializeField] private Color highlightedColor;
    [SerializeField] private float HighlightedFontSize;
    [SerializeField] private Color defaultColor;
    private float defaultFontSize;
    private Campfire campfire;
    public int index { get; private set; }

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

    }

    private void Travel()
    {

    }

    private void Stargaze()
    {

    }

    private void Brew()
    {

    }

    private void StandUp()
    {
        campfire.StandUp();
        campfire = null;
    }

    public void SetUpCampfire(Campfire campfire) => this.campfire = campfire;
    

    private void OnEnable() => SwitchTo(0);
}
