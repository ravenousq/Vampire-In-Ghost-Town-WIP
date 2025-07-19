using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettings : MenuNavigation
{
    public int masterVolume { get; private set; }
    public int effectVolume { get; private set; }
    public int musicVolume { get; private set; }
    public int dialoguesVoulme { get; private set; }


    protected override void Start()
    {
        base.Start();

        masterVolume = MainMenu.instance.GetVolume("Master");
        effectVolume = MainMenu.instance.GetVolume("SFX");
        musicVolume = MainMenu.instance.GetVolume("Music");
        dialoguesVoulme = MainMenu.instance.GetVolume("Dialogues");

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
                    MainMenu.instance.SetVolume("Master", masterVolume);
                    break;
                case 1:
                    effectVolume = sliders[1].value;
                    MainMenu.instance.SetVolume("SFX", effectVolume);
                    break;
                case 2:
                    musicVolume = sliders[2].value;
                    MainMenu.instance.SetVolume("Music", musicVolume);
                    break;
                case 3:
                    dialoguesVoulme = sliders[3].value;
                    MainMenu.instance.SetVolume("Dialogues", dialoguesVoulme);
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

    
}
