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

    protected override void OnLeftPressed()
    {
        base.OnLeftPressed();

        AdjustSettings();
    }

    protected override void OnRightPressed()
    {
        base.OnRightPressed();

        AdjustSettings();
    }

    private void AdjustSettings(bool start = false)
    {
        if (!start)
            switch (currentButtonIndex)
            {
                case 0:
                    masterVolume = sliders[0].value;
                    break;
                case 1:
                    effectVolume = sliders[1].value;
                    break;
                case 2:
                    musicVolume = sliders[2].value;
                    break;
                case 3:
                    dialoguesVoulme = sliders[3].value;
                    break;
            }
        else
        {
            masterVolume = sliders[0].value;
            effectVolume = sliders[1].value;
            musicVolume = sliders[2].value;
            dialoguesVoulme = sliders[3].value;
        }
    }
}
