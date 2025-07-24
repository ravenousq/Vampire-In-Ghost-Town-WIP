using TMPro;
using UnityEngine;


public class TitleScreen : MenuNavigation, ISaveManager
{
    [SerializeField] private string newGameSceneName;
    private TextMeshProUGUI continueButton;
    private int lastScene;

    protected override void Start()
    {
        continueButton = buttons[0];

        if (!SaveManager.instance.HasSavedData())
            HideContinueButton();

        base.Start();
    }

    private void HideContinueButton()
    {
        buttons.RemoveAt(0);
        continueButton.transform.SetParent(transform);
        continueButton.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Remote()
    {
        switch (continueButton.gameObject.activeSelf ? currentButtonIndex : currentButtonIndex + 1)
        {
            case 0:
                MainMenu.instance.fadeScreen.FadeIn();
                Invoke(nameof(ContinueGame), 1.5f);
                break;
            case 1:
                MainMenu.instance.fadeScreen.FadeIn();
                Invoke(nameof(StartNewGame), 1.5f);
                break;
            case 2:
                screenToSwitch = Screens.SettingsScreen;
                break;
            case 3:
                screenToSwitch = Screens.CreditsScreen;
                break;
            case 4:
                Application.Quit();
                break;
            default:
                Debug.LogWarning("Unknown button index: " + currentButtonIndex);
                break;
        }

        base.Remote();
    }

    private void StartNewGame()
    {
        SaveManager.instance.DeleteSavedData();
        SaveManager.instance.SaveSettings();
        UnityEngine.SceneManagement.SceneManager.LoadScene(newGameSceneName);
    }

    private void ContinueGame()
    {
        SaveManager.instance.SaveSettings();
        UnityEngine.SceneManagement.SceneManager.LoadScene(lastScene);
    }

    public void LoadData(GameData data)
    {
        lastScene = data.lastScene;
    }

    public void SaveData(ref GameData data)
    {
    
    }
}
