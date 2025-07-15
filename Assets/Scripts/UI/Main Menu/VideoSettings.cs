using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VideoSettings : MenuNavigation
{
    public KeyValuePair<int, int> resolution { get; private set; } = new KeyValuePair<int, int>(1920, 1080);
    public FullScreenMode fullscreenMode { get; private set; } = FullScreenMode.ExclusiveFullScreen;
    private int chosenResolutionIndex = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        lists[0].SetUp(screenModes);
        lists[1].SetUp(resolutions);
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
                break;
            case 1:
                break;
            case 2:
                screenToSwitch = Screens.SettingsScreen;
                break;
            default:
                break;
        }

        base.Remote();
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
                fullscreenMode = (FullScreenMode)(((int)fullscreenMode + (increment ? 1 : -1) + fullscreenCount) % fullscreenCount);

                SetResolution();

                break;

            case 1:
                chosenResolutionIndex = (chosenResolutionIndex + (increment ? 1 : -1) + possibleResolutions.Length) % possibleResolutions.Length;

                resolution = new KeyValuePair<int, int>(possibleResolutions[chosenResolutionIndex][0], possibleResolutions[chosenResolutionIndex][1]);

                SetResolution();

                break;

            default:
                break;
        }
    }

}
