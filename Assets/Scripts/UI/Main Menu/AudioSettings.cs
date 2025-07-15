using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class AudioSettings : MenuNavigation
{
    public int masterVolume { get; private set; }
    public int effectVolume { get; private set; }
    public int musicVolume { get; private set; }
    public int dialoguesVoulme { get; private set; }

    protected override void Start()
    {
        base.Start();

        HighlightCurrentPipSlider();

        AdjustSettings(true);
    }

    protected override void Update()
    {
        base.Update();
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
            case 4:
                screenToSwitch = Screens.SettingsScreen;
                break;

        }

        base.Remote();
    }

    protected override void OnUpPressed()
    {
        base.OnUpPressed();

        HighlightCurrentPipSlider();
    }

    protected override void OnDownPressed()
    {
        base.OnDownPressed();

        HighlightCurrentPipSlider();
    }

    protected override void OnLeftPressed()
    {
        base.OnLeftPressed();

        RemovePipFromCurrentList();

        AdjustSettings();
    }

    protected override void OnRightPressed()
    {
        base.OnRightPressed();

        AddPipToCurrentList();

        AdjustSettings();
    }

    private void AdjustSettings(bool start = false)
    {
        if (!start)
            switch (currentButtonIndex)
            {
                case 0:
                    masterVolume = pipSliders[0].value;
                    break;
                case 1:
                    effectVolume = pipSliders[1].value;
                    break;
                case 2:
                    musicVolume = pipSliders[2].value;
                    break;
                case 3:
                    dialoguesVoulme = pipSliders[3].value;
                    break;
            }
        else
        {
            masterVolume = pipSliders[0].value;
            effectVolume = pipSliders[1].value;
            musicVolume = pipSliders[2].value;
            dialoguesVoulme = pipSliders[3].value;
        }
    }
}
