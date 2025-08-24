using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathScreen : MonoBehaviour, ISaveManager
{
    [SerializeField] private Image deathScreen;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private float fadeOutSpeed;
    private int sceneToReload;
    private bool isFading;


    private void Update()
    {
        if (isFading)
        {
            float alpha = Mathf.MoveTowards(deathScreen.color.a, 1, fadeOutSpeed * Time.unscaledDeltaTime);

            deathScreen.color = new Color(deathScreen.color.r, deathScreen.color.g, deathScreen.color.b, alpha);
            deathText.color = new Color(deathText.color.r, deathText.color.g, deathText.color.b, alpha);

            if (alpha == 1)
            {
                isFading = false;
                StartCoroutine(ResetGameRoutine());
            }
        }
    }

    private void OnEnable() => isFading = true;

    private IEnumerator ResetGameRoutine()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        UI.instance.fadeScreen.FadeIn();

        yield return new WaitForSecondsRealtime(1.5f);

        ResetGame();
    }

    private void ResetGame()
    {
        SaveManager.instance.SaveGame();
        SaveManager.instance.SaveSettings(); 
        SceneManager.LoadScene(sceneToReload);
    }

    public void LoadData(GameData data)
    {
        sceneToReload = data.lastCampfireScene != 0 ? data.lastCampfireScene : 1;
    }

    public void SaveData(ref GameData data)
    {
        //throw new System.NotImplementedException();
    }
}
