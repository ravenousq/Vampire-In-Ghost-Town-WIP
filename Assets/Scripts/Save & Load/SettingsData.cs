using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    [Header("Sound Settings")]
    public List<int> soundSettings;

    [Header("Video Settings")]
    public int resolution;
    public int screenMode;

    [Header("Game Settings")]
    public int audioLanguage;
    public int textLanguage;
    public bool showTutorials;

    public SettingsData()
    {
        soundSettings = new List<int> { 4, 4, 4, 4 };
        resolution = 0;
        screenMode = 0;
        audioLanguage = 0;
        textLanguage = 0;
        showTutorials = true;
    }
}
