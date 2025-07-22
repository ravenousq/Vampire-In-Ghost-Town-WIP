using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class AudioSettings : MenuNavigation, ISaveManagerSettings
{
    public int masterVolume { get; private set; }
    public int effectVolume { get; private set; }
    public int musicVolume { get; private set; }
    public int dialoguesVoulme { get; private set; }


    protected override void Start()
    {
        base.Start();

        sliders[0].SetTo(masterVolume);
        sliders[1].SetTo(effectVolume);
        sliders[2].SetTo(musicVolume);
        sliders[3].SetTo(dialoguesVoulme);

        AdjustSettings(true);
    }

    protected override void Remote()
    {
        switch (currentButtonIndex)
        {
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
                    masterVolume = Mathf.Clamp(sliders[0].value, 0, 9);
                    MainMenu.instance.SetVolume("Master", masterVolume);
                    break;
                case 1:
                    effectVolume = Mathf.Clamp(sliders[1].value, 0, 9);
                    MainMenu.instance.SetVolume("SFX", effectVolume);
                    break;
                case 2:
                    musicVolume = Mathf.Clamp(sliders[2].value, 0, 9);
                    MainMenu.instance.SetVolume("Music", musicVolume);
                    break;
                case 3:
                    dialoguesVoulme = Mathf.Clamp(sliders[3].value, 0, 9);
                    MainMenu.instance.SetVolume("Dialogues", dialoguesVoulme);
                    break;
            }
        else
        {
            MainMenu.instance.SetVolume("Master", masterVolume);
            MainMenu.instance.SetVolume("SFX", effectVolume);
            MainMenu.instance.SetVolume("Music", musicVolume);
            MainMenu.instance.SetVolume("Dialogues", dialoguesVoulme);
        }
    }

    public void LoadData(SettingsData data)
    {
        masterVolume = data.soundSettings[0];
        musicVolume = data.soundSettings[1];
        effectVolume = data.soundSettings[2];
        dialoguesVoulme = data.soundSettings[3];

        AdjustSettings(true);
    }

    public void SaveData(ref SettingsData data)
    {
        data.soundSettings[0] = masterVolume;
        data.soundSettings[1] = musicVolume;
        data.soundSettings[2] = effectVolume;
        data.soundSettings[3] = dialoguesVoulme;
    }
}
