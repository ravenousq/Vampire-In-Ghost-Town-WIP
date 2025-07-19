using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum Screens
{
    TitleScreen = 0,
    SettingsScreen = 1,
    GameSettingsScreen = 2,
    VideoSettingsScreen = 3,
    AudioSettingsScreen = 4,
    CreditsScreen = 5,
    NullScreen = 8
}
public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public FadeScreen fadeScreen { get; private set; }
    private Image menuImage;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        fadeScreen = GetComponentInChildren<FadeScreen>();
        menuImage = GetComponent<Image>();
    }

    [SerializeField] private GameObject[] screens;
    [SerializeField] private Sprite[] menuImages;
    [SerializeField] private AudioMixer mixer;
    private Screens currentScreen = Screens.TitleScreen;

    private void Start()
    {
        SetVolume("Master", 10);
        SetVolume("SFX", 10);
        SetVolume("Music", 10);
        SetVolume("Dialogues", 10);

        screens[3].GetComponent<VideoSettings>().SetResolution();

        foreach (var screen in screens)
            screen.SetActive(false);

        SwitchTo(currentScreen, .5f);
    }

    public void SwitchTo(Screens screen, float fadeSpeed = 0)
    {
        //Debug.Log(screen);

        screens[(int)currentScreen].SetActive(false);

        currentScreen = screen;

        screens[(int)currentScreen].SetActive(true);

        //menuImage.sprite = menuImages[(int)currentScreen];

        fadeScreen.FadeOut(fadeSpeed);
    }

    public void SwitchTo(int screen, float fadeSpeed = 0)
    {
        screens[(int)currentScreen].SetActive(false);

        currentScreen = (Screens)screen;

        screens[(int)currentScreen].SetActive(true);

        //menuImage.sprite = menuImages[(int)currentScreen];

        fadeScreen.FadeOut(fadeSpeed);
    }

    public static string AddSpacesToEnumName(string enumName) => System.Text.RegularExpressions.Regex.Replace(enumName, "(\\B[A-Z])", " $1");

    public void SetSFXVolume(float volume)
    {
        mixer.SetFloat("SFXV", Mathf.Log10(volume) * 20);
    }

    public float GetSFXVolume(string name)
    {
        if (mixer.GetFloat(name, out float value))
            return Mathf.Pow(10, value / 20);


        Debug.LogWarning("SFXVolume not found");
        return 1f;
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
}
