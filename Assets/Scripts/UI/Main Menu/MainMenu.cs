using UnityEngine;
using UnityEngine.UI;

public enum Screens
{
    TitleScreen = 0,
    SettingsScreen = 1,
    GameSettingsScreen = 2,
    VideoSettingsScreen = 3,
    AudioSettingsScreen = 4,
    ControlsScreen = 5,
    CreditsScreen = 6,
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
        foreach (var screen in screens)
            screen.SetActive(false);
        
        SwitchTo(currentScreen, .5f);
    }

    public void SwitchTo(Screens screen, float fadeSpeed = 0)
    {
        screens[(int)currentScreen].SetActive(false);

        currentScreen = screen;

        screens[(int)currentScreen].SetActive(true);

        //menuImage.sprite = menuImages[(int)currentScreen];

        fadeScreen.FadeOut(fadeSpeed);
    }

    public void Switchto(int screen, float fadeSpeed = 0)
    {
        screens[(int)currentScreen].SetActive(false);

        currentScreen = (Screens)screen;

        screens[(int)currentScreen].SetActive(true);

        //menuImage.sprite = menuImages[(int)currentScreen];

        fadeScreen.FadeOut(fadeSpeed);
    }

    
}
