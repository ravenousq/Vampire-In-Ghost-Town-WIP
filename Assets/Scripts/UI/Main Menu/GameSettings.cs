using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    [SerializeField] private GameObject[] lists;
    public Language audioLanguage { get; private set; }
    public Language textLanguage { get; private set; }
    public bool showTutorials { get; private set; } = true;

    protected override void Start()
    {
        base.Start();

        ActivateListByIndex(0);
    }


    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            switch (currentButtonIndex)
            {
                case 0:
                    ActivateListByIndex(0);
                    break;
                case 1:
                    ActivateListByIndex(1);
                    break;
                case 2:
                    ActivateListByIndex(2);
                    break;
                default:
                    ActivateListByIndex();
                    break;
            }

        if (Input.GetKeyDown(KeyCode.A))
            ChangeOption(false);

        if (Input.GetKeyDown(KeyCode.D))
            ChangeOption();   
        
        if(MainMenu.instance.fadeScreen.isFadingIn || MainMenu.instance.fadeScreen.isFadingOut)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            Remote();
            ChangeOption();
        }
    }

    protected override void Remote()
    {
        switch (currentButtonIndex)
        {
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

    private void ActivateList(GameObject list, bool activate = true)
    {
        TextMeshProUGUI listText = list.GetComponentInChildren<TextMeshProUGUI>();
        listText.fontSize = activate ? highlightedFontSize : defaultFontSize;
        listText.color = activate ? highlightedColor : defaultColor;

    }

    private void ActivateListByIndex(int index = -1)
    {
        for (int i = 0; i < lists.Length; i++)
            ActivateList(lists[i], index == i);
    }

    private void ChangeOption(bool increment = true)
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
}
