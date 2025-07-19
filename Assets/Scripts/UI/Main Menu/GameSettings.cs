using System;
using System.Collections.Generic;

public class GameSettings : MenuNavigation
{
    public Language audioLanguage { get; private set; }
    public Language textLanguage { get; private set; }
    public bool showTutorials { get; private set; } = true;

    protected override void Start()
    {
        base.Start();

        lists[0].SetUp(languages);
        lists[1].SetUp(languages);
        lists[2].SetUp(booleans);
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
                screenToSwitch = Screens.SettingsScreen;
                break;
            default:
                break;
        }

        base.Remote();
    }

    protected override void ChangeOption(bool increment = true)
    {
        int languageCount = Enum.GetValues(typeof(Language)).Length;

        switch (currentButtonIndex)
        {
            case 0:
                audioLanguage = (Language)(((int)audioLanguage + (increment ? 1 : -1) + languageCount) % languageCount);
                break;
            case 1:
                textLanguage = (Language)(((int)textLanguage + (increment ? 1 : -1) + languageCount) % languageCount);
                break;
            case 2:
                showTutorials = !showTutorials;
                break;
            default:
                break;
        }
    }

    protected override void OnLeftPressed()
    {
        base.OnLeftPressed();

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
