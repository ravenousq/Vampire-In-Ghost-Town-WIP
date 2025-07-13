using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> buttons;
    [SerializeField] private Color highlightColor;
    [SerializeField] private float highlightedFontSize;
    private TextMeshProUGUI continueButton;
    private Color defaultColor;
    private float defaultFontSize;
    private int currentButtonIndex = 0;
    private Screens screenToSwitch = Screens.NullScreen;

    void Start()
    {
        MainMenu.instance.fadeScreen.FadeOut(.5f);

        continueButton = buttons[0];

        HideContinueButton();

        defaultFontSize = buttons[currentButtonIndex].fontSize;
        defaultColor = buttons[currentButtonIndex].color;

        SwitchTo(currentButtonIndex);
    }

    private void HideContinueButton()
    {
        buttons.RemoveAt(0);
        continueButton.transform.SetParent(transform);
        continueButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            SwitchTo(currentButtonIndex == 0 ? buttons.Count - 1 : currentButtonIndex - 1);

        if (Input.GetKeyDown(KeyCode.S))
            SwitchTo((currentButtonIndex + 1) % buttons.Count);

        if (Input.GetKeyDown(KeyCode.C))
        {
            Remote();
            MainMenu.instance.fadeScreen.FadeIn();
        }
    }

    private void SwitchTo(int index)
    {

        buttons[currentButtonIndex].color = defaultColor;
        buttons[currentButtonIndex].fontSize = defaultFontSize;

        currentButtonIndex = index;

        buttons[currentButtonIndex].color = highlightColor;
        buttons[currentButtonIndex].fontSize = highlightedFontSize;
    }

    private void Remote()
    {
        switch (continueButton.gameObject.activeSelf ? currentButtonIndex : currentButtonIndex + 1)
        {
            case 0:
                // Continue game
                Debug.Log("Continue");
                break;
            case 1:
                // Start New Game
                Debug.Log("New Game");
                break;
            case 2:
                screenToSwitch = Screens.SettingsScreen;
                break;
            case 3:
                screenToSwitch = Screens.CreditsScreen;
                break;
            case 4:
                Application.Quit();
                break;
            default:
                Debug.LogWarning("Unknown button index: " + currentButtonIndex);
                break;
        }

        Invoke(nameof(ChangeScreen), 1.5f);
    }

    private void ChangeScreen()
    {

        if (screenToSwitch != Screens.NullScreen)
            MainMenu.instance.SwitchTo(screenToSwitch);
        
    }
}
