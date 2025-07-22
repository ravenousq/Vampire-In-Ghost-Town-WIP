using System.Collections;
using UnityEngine;

public class PauseMenu : MenuNavigation
{
    [Space]
    [SerializeField] private PauseMenuSettings settings;

    private void OnEnable()
    {
        Time.timeScale = 0;

        if (UI.instance && UI.instance.canTurnOnGameMenu)
            UI.instance.LockGameMenu();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;

        if (!UI.instance.canTurnOnGameMenu)
            UI.instance.LockGameMenu();
    }

    protected override void Update()
    {
        if (settings.gameObject.activeSelf)
            return;

        base.Update();
    }

    protected override void Remote()
    {
        switch (currentButtonIndex)
        {
            case 0:
                gameObject.SetActive(false);
                break;
            case 1:
                UI.instance.fadeScreen.FadeIn();
                StartCoroutine(GoToSettings());
                break;
            case 2:
                UI.instance.fadeScreen.FadeIn();
                AudioManager.instance.StopBGM();
                StartCoroutine(GoToMainMenu());
                break;
        }

        base.Remote();
    }

    private IEnumerator GoToMainMenu()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        SaveManager.instance.SaveGame();
        SaveManager.instance.SaveSettings();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }

    private IEnumerator GoToSettings()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        settings.gameObject.SetActive(true);
    }

    public override bool CanNavigate() =>!(UI.instance.fadeScreen.isFadingIn || UI.instance.fadeScreen.isFadingOut);

}
