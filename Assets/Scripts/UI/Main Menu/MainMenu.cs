using UnityEngine;
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
    private Screens currentScreen = Screens.TitleScreen;

    private void Start()
    {
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
}
