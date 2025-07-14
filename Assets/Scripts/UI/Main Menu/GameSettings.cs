using TMPro;
using UnityEngine;

public enum Language
{
    English,
    Polski,
    日本語
}
public class GameSettings : MenuNavigation
{
    [SerializeField] private GameObject AudioLanguageList;
    [SerializeField] private GameObject TextlanguageList;
    [SerializeField] private GameObject ShowTutorialsList;

    protected override void Start()
    {
        base.Start();

        ActivateList(AudioLanguageList);
        ActivateList(TextlanguageList, false);
        ActivateList(ShowTutorialsList, false);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.C))
        {
            Remote();
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            switch (currentButtonIndex)
            {
                case 0:
                    ActivateList(AudioLanguageList);
                    ActivateList(TextlanguageList, false);
                    ActivateList(ShowTutorialsList, false);
                    break;
                case 1:
                    ActivateList(AudioLanguageList, false);
                    ActivateList(TextlanguageList);
                    ActivateList(ShowTutorialsList, false);
                    break;
                case 2:
                    ActivateList(AudioLanguageList, false);
                    ActivateList(TextlanguageList, false);
                    ActivateList(ShowTutorialsList);
                    break;
                default:
                    ActivateList(AudioLanguageList, false);
                    ActivateList(TextlanguageList, false);
                    ActivateList(ShowTutorialsList, false);
                    break;
            }
    }

    protected override void Remote()
    {
        switch (currentButtonIndex)
        {
            case 0:
                ActivateList(AudioLanguageList);
                ActivateList(TextlanguageList, false);
                ActivateList(ShowTutorialsList, false);
                break;
            case 1:
                ActivateList(AudioLanguageList, false);
                ActivateList(TextlanguageList);
                ActivateList(ShowTutorialsList, false);
                break;
            case 2:
                ActivateList(AudioLanguageList, false);
                ActivateList(TextlanguageList, false);
                ActivateList(ShowTutorialsList);
                break;
            case 3:
                screenToSwitch = Screens.ControlsScreen;
                MainMenu.instance.fadeScreen.FadeIn();
                break;
            case 4:
                screenToSwitch = Screens.SettingsScreen;
                MainMenu.instance.fadeScreen.FadeIn();
                break;
            default:
                break;
        }

        base.Remote();
    }

    private void ActivateList(GameObject List, bool activate = true)
    {
        TextMeshProUGUI listText = List.GetComponentInChildren<TextMeshProUGUI>();
        listText.fontSize = activate ? highlightedFontSize : defaultFontSize;
        listText.color = activate ? highlightedColor : defaultColor;
    }
}
