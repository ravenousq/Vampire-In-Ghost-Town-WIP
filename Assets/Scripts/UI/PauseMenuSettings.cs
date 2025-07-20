using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenuSettings : MenuNavigation
{
    [SerializeField] private AudioMixer mixer;
    public int masterVolume { get; private set; }
    public int effectVolume { get; private set; }
    public int musicVolume { get; private set; }
    public int dialoguesVoulme { get; private set; }
    public KeyValuePair<int, int> resolution { get; private set; } = new KeyValuePair<int, int>(1920, 1080);
    public FullScreenMode fullscreenMode { get; private set; } = FullScreenMode.ExclusiveFullScreen;
    private int chosenResolutionIndex = 0;



    protected override void Start()
    {
        base.Start();

        fullscreenMode = Screen.fullScreenMode;
        resolution = new KeyValuePair<int, int>(Screen.width, Screen.height);

        lists[0].SetUp(screenModes);
        lists[1].SetUp(resolutions);

        masterVolume = GetVolume("Master");
        effectVolume = GetVolume("SFX");
        musicVolume = GetVolume("Music");
        dialoguesVoulme = GetVolume("Dialogues");

        AdjustSettings(true);
    }

    private void OnEnable()
    {
        UI.instance.fadeScreen.FadeOut();
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
                break;
            case 3:
                break;
            case 5:
                break;
            case 6:
                StartCoroutine(GoBackToSettings());
                UI.instance.fadeScreen.FadeIn();
                break;
        }

        base.Remote();
    }

    protected override void OnLeftPressed()
    {
        if (!CanNavigate())
            return;

        if (IsListActive())
        {
            lists[currentButtonIndex - 4].Retract();
            ChangeOption(false);
        }

        if (IsSliderActive())
        {
            sliders[currentButtonIndex].RemovePip();
            AdjustSettings();
        }
    }

    protected override void OnRightPressed()
    {
        if (!CanNavigate())
            return;

        if (IsListActive())
        {
            lists[currentButtonIndex - 4].Proceed();
            ChangeOption(true);
        }

        if (IsSliderActive())
        {
            sliders[currentButtonIndex].AddPip();
            AdjustSettings();
        }
    }

    protected override bool IsListActive() => lists.Length - 1 >= currentButtonIndex - 4 && currentButtonIndex >= 4;

    protected override void SwitchTo(int index)
    {
        if (buttons.Count == 0)
            return;

        buttons[currentButtonIndex].color = defaultColor;
        buttons[currentButtonIndex].fontSize = defaultFontSize;

        if (IsListActive())
            lists[currentButtonIndex - 4].Highlight(false);

        if (IsSliderActive())
            sliders[currentButtonIndex].Highlight(false);

        currentButtonIndex = index;

        buttons[currentButtonIndex].color = highlightedColor;
        buttons[currentButtonIndex].fontSize = highlightedFontSize;

        if (IsListActive())
            lists[currentButtonIndex - 4].Highlight();

        if (IsSliderActive())
            sliders[currentButtonIndex].Highlight();
    }

    private IEnumerator GoBackToSettings()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        UI.instance.fadeScreen.FadeOut();
        gameObject.SetActive(false);
    }

    public override bool CanNavigate() => !(UI.instance.fadeScreen.isFadingIn || UI.instance.fadeScreen.isFadingOut);

    private void AdjustSettings(bool start = false)
    {
        if (!start)
            switch (currentButtonIndex)
            {
                case 0:
                    masterVolume = sliders[0].value;
                    SetVolume("Master", masterVolume);
                    break;
                case 1:
                    effectVolume = sliders[1].value;
                    SetVolume("SFX", effectVolume);
                    break;
                case 2:
                    musicVolume = sliders[2].value;
                    SetVolume("Music", musicVolume);
                    break;
                case 3:
                    dialoguesVoulme = sliders[3].value;
                    SetVolume("Dialogues", dialoguesVoulme);
                    break;
            }
        else
        {
            sliders[0].SetTo(masterVolume);
            sliders[1].SetTo(effectVolume);
            sliders[2].SetTo(musicVolume);
            sliders[3].SetTo(dialoguesVoulme);
        }
    }

    public void SetVolume(string name, int level)
    {
        level = Mathf.Clamp(level, 0, 10);
        float linear = level / 10f;

        float db = (linear > 0.0001f) ? Mathf.Log10(linear) * 20f : -80f;

        mixer.SetFloat(name, db);
    }

    public int GetVolume(string name)
    {
        if (mixer.GetFloat(name, out float db))
        {
            float linear = Mathf.Pow(10f, db / 20f);

            return Mathf.RoundToInt(linear * 10f);
        }

        Debug.LogWarning($"Parameter '{name}' not found!");
        return 10;
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
    
    protected override void ChangeOption(bool increment = true)
    {
        switch (currentButtonIndex - 4)
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
