using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VideoSettings : MenuNavigation
{
    [SerializeField] private GameObject[] lists;
    private int[][] resolutions = new int[][]
    {
        new int[2] { 1920, 1080 },
        new int[2] { 1920, 1200 },
        new int[2] { 1920, 1440 },
        new int[2] { 1024, 768 },
        new int[2] { 1152, 864 },
        new int[2] { 1176, 664 },
        new int[2] { 1280, 1440 },
        new int[2] { 1280, 720 },
        new int[2] { 1280, 768 },
        new int[2] { 1280, 800 },
        new int[2] { 1280, 960 },
        new int[2] { 1280, 1024 },
        new int[2] { 1360, 768 },
        new int[2] { 1366, 768 },
        new int[2] { 1440, 900 },
        new int[2] { 1440, 1080 },
        new int[2] { 1600, 900 },
        new int[2] { 1600, 1024 },
        new int[2] { 1600, 1200 },
        new int[2] { 1680, 1050 },
        new int[2] { 800, 600 },
        new int[2] { 640, 480 },
        new int[2] { 720, 480 },
        new int[2] { 720, 576 },
        new int[2] { 800, 600 },
    };
    private KeyValuePair<int, int> resolution = new KeyValuePair<int, int>(1920, 1080);
    private int chosenResolutionIndex = 0;
    private FullScreenMode fullscreenMode = FullScreenMode.Windowed;

    protected override void Start()
    {
        base.Start();

        ActivateListByIndex(0);

        lists[0].GetComponentInChildren<TextMeshProUGUI>().text = AddSpacesToEnumName(fullscreenMode.ToString());
        lists[1].GetComponentInChildren<TextMeshProUGUI>().text = resolution.Key + " x " + resolution.Value;
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
                default:
                    ActivateListByIndex();
                    break;
            }

        if (Input.GetKeyDown(KeyCode.A))
            ChangeOption(false);

        if (Input.GetKeyDown(KeyCode.D))
            ChangeOption();

        if (MainMenu.instance.fadeScreen.isFadingIn || MainMenu.instance.fadeScreen.isFadingOut)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            Remote();
            MainMenu.instance.fadeScreen.FadeIn();
        }
    }

    public void SetResolution()
    {
        Screen.SetResolution(resolution.Key, resolution.Value, fullscreenMode);
    }

    public void ToggleFullscreen()
    {
        fullscreenMode = fullscreenMode == FullScreenMode.FullScreenWindow ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;
        SetResolution();
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

    protected override void Remote()
    {
        base.Remote();
        switch (currentButtonIndex)
        {
            case 0:
                ActivateListByIndex(0);
                break;
            case 1:
                ActivateListByIndex(1);
                break;
            case 2:
                screenToSwitch = Screens.SettingsScreen;
                break;
            default:
                ActivateListByIndex();
                break;
        }
    }

    private void ChangeOption(bool increment = true)
    {
        switch (currentButtonIndex)
        {
            case 0:
                int fullscreenCount = Enum.GetValues(typeof(FullScreenMode)).Length;
                TextMeshProUGUI windowText = lists[currentButtonIndex].GetComponentInChildren<TextMeshProUGUI>();
                fullscreenMode = (FullScreenMode)(((int)fullscreenMode + (increment ? 1 : -1) + fullscreenCount) % fullscreenCount);

                windowText.text = AddSpacesToEnumName(fullscreenMode.ToString());

                SetResolution();

                break;

            case 1:
                TextMeshProUGUI resolutionText = lists[currentButtonIndex].GetComponentInChildren<TextMeshProUGUI>();

                chosenResolutionIndex = (chosenResolutionIndex + (increment ? 1 : -1) + resolutions.Length) % resolutions.Length;

                resolution = new KeyValuePair<int, int>(resolutions[chosenResolutionIndex][0], resolutions[chosenResolutionIndex][1]);

                resolutionText.text = resolution.Key + " x " + resolution.Value;

                SetResolution();

                break;

            default:
                break;
        }
    }
    
    private string AddSpacesToEnumName(string enumName) => System.Text.RegularExpressions.Regex.Replace(enumName, "(\\B[A-Z])", " $1");

}
