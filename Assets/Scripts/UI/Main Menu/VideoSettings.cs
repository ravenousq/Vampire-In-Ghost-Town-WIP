using System;
using System.Collections.Generic;
using UnityEngine;

public class VideoSettings : MenuNavigation, ISaveManagerSettings
{
    public KeyValuePair<int, int> resolution { get; private set; } = new KeyValuePair<int, int>(1920, 1080);
    public FullScreenMode fullscreenMode { get; private set; } = FullScreenMode.ExclusiveFullScreen;
    private int chosenResolutionIndex = 0;
    private const int defaultResolutionIndex = 0;
    private const int defaultFullscreenMode = 0;

    protected override void Start()
    {
        base.Start();

        lists[0].SetUp(screenModes);
        lists[1].SetUp(resolutions);

        for (int i = 0; i < (int)fullscreenMode; i++)
            lists[0].Proceed();

        for (int i = 0; i < chosenResolutionIndex; i++)
            lists[1].Proceed();
    }

    public void SetResolution()
    {
        Screen.SetResolution(resolution.Key, resolution.Value, fullscreenMode);
    }

    protected override void Remote()
    {
        switch (currentButtonIndex)
        {
            case 2:
                RestoreDefaultSettings();
                break;
            case 3:
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

    public void LoadData(SettingsData data)
    {
        chosenResolutionIndex = data.resolution;
        fullscreenMode = (FullScreenMode)data.screenMode;

        resolution = new KeyValuePair<int, int>(possibleResolutions[chosenResolutionIndex][0], possibleResolutions[chosenResolutionIndex][1]);

        SetResolution();
    }

    public void SaveData(ref SettingsData data)
    {
        data.resolution = chosenResolutionIndex;
        data.screenMode = (int)fullscreenMode;
    }

    private void RestoreDefaultSettings()
    {
        for (int i = 0; i < (int)fullscreenMode; i++)
            lists[0].Retract();

        for (int i = 0; i < chosenResolutionIndex; i++)
            lists[1].Retract();

        chosenResolutionIndex = defaultResolutionIndex;
        fullscreenMode = (FullScreenMode)defaultFullscreenMode;

        resolution = new KeyValuePair<int, int>(possibleResolutions[chosenResolutionIndex][0], possibleResolutions[chosenResolutionIndex][1]);

        SetResolution();
    }
}
