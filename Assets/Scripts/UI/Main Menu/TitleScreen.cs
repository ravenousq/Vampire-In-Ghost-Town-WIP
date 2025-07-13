using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TitleScreen : MenuNavigation
{
    private TextMeshProUGUI continueButton;

    protected override void Start()
    {
        continueButton = buttons[0];

        HideContinueButton();

        base.Start();
    }

    private void HideContinueButton()
    {
        buttons.RemoveAt(0);
        continueButton.transform.SetParent(transform);
        continueButton.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.C))
        {
            Remote();
            MainMenu.instance.fadeScreen.FadeIn();
        }
    }

    protected override void Remote()
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

        base.Remote();
    }

}
