using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VideoSettings : MenuNavigation
{
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
    public KeyValuePair<int, int> resolution { get; private set; } = new KeyValuePair<int, int>(1920, 1080);
    public FullScreenMode fullscreenMode { get; private set; } = FullScreenMode.ExclusiveFullScreen;
    private int chosenResolutionIndex = 0;

    protected override void Start()
    {
        base.Start();

        ActivateListByIndex(0);

        lists[0].GetComponentInChildren<TextMeshProUGUI>().text = MainMenu.AddSpacesToEnumName(fullscreenMode.ToString());
        lists[1].GetComponentInChildren<TextMeshProUGUI>().text = resolution.Key + " x " + resolution.Value;
    }

    protected override void Update()
    {
        base.Update();
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

    protected override void Remote()
    {
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

    protected override void ChangeOption(bool increment = true)
    {
        switch (currentButtonIndex)
        {
            case 0:
                int fullscreenCount = Enum.GetValues(typeof(FullScreenMode)).Length;
                TextMeshProUGUI windowText = lists[currentButtonIndex].GetComponentInChildren<TextMeshProUGUI>();
                fullscreenMode = (FullScreenMode)(((int)fullscreenMode + (increment ? 1 : -1) + fullscreenCount) % fullscreenCount);

                windowText.text = MainMenu.AddSpacesToEnumName(fullscreenMode.ToString());

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

}
