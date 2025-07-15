using System;
using TMPro;
using UnityEngine;

public enum Language
{
    English = 0,
    Polish = 1,
    Japanese = 2
}
public class GameSettings : MenuNavigation
{
    public Language audioLanguage { get; private set; }
    public Language textLanguage { get; private set; }
    public bool showTutorials { get; private set; } = true;

    protected override void Start()
    {
        base.Start();

        ActivateListByIndex(currentButtonIndex);
    }


    protected override void Update()
    {
        base.Update();
    }

    protected override void Remote()
    {
        switch (currentButtonIndex)
        {
            case 3:
                screenToSwitch = Screens.ControlsScreen;
                break;
            case 4:
                screenToSwitch = Screens.SettingsScreen;
                break;
            default:
                break;
        }

        base.Remote();
    }

    protected override void OnUpPressed()
    {
        base.OnUpPressed();

        ActivateListByIndex(currentButtonIndex);
    }

    protected override void OnDownPressed()
    {
        base.OnDownPressed();

        ActivateListByIndex(currentButtonIndex);
    }

    protected override void ChangeOption(bool increment = true)
    {
        int languageCount = Enum.GetValues(typeof(Language)).Length;

        switch (currentButtonIndex)
        {
            case 0:
                TextMeshProUGUI audioListText = lists[currentButtonIndex].GetComponentInChildren<TextMeshProUGUI>();

                audioLanguage = (Language)(((int)audioLanguage + (increment ? 1 : -1) + languageCount) % languageCount);
                audioListText.text = audioLanguage.ToString();
                break;
            case 1:
                TextMeshProUGUI textListText = lists[currentButtonIndex].GetComponentInChildren<TextMeshProUGUI>();

                textLanguage = (Language)(((int)textLanguage + (increment ? 1 : -1) + languageCount) % languageCount);
                textListText.text = textLanguage.ToString();
                break;
            case 2:
                TextMeshProUGUI tutorialsListText = lists[currentButtonIndex].GetComponentInChildren<TextMeshProUGUI>();

                showTutorials = !showTutorials;
                tutorialsListText.text = showTutorials ? "Enabled" : "Disabled";
                break;
            default:
                break;
        }
    }

    protected override void OnLeftPressed()
    {
        base.OnUpPressed();

        ChangeOption(false);
    }

    protected override void OnRightPressed()
    {
        base.OnRightPressed();

        ChangeOption();
    }

    protected override void OnConfirmation()
    {
        base.OnConfirmation();

        ChangeOption();
    }
}
