using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    public Player player;
    [SerializeField] private float multiplier = 1;
    public int currency { get; private set; } = 10000;

    private void Start() => AudioManager.instance.PlayBGM(10);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.instance.fadeScreen.FadeIn();
            AudioManager.instance.StopBGM();
            Invoke(nameof(GoToMainMenu), 1.5f);
            
        }
    }

    public void AddCurrency(int currencyToAdd)
    {
        int finalCurrency = Mathf.RoundToInt(currencyToAdd * multiplier);
        currency += finalCurrency;
        UI.instance.ModifySouls(finalCurrency);
    }
    public void RemoveCurrency(int currencyToRemove) => currency -= currencyToRemove;
    public bool CanAfford(int price) => currency >= price;
    public void AddMultiplier(float multiplier) => this.multiplier += multiplier;


    public void StartStopFootSteps(bool start = true)
    {
        if (start)
            InvokeRepeating(nameof(PlayFootseps), 0, 0.2f);
        else
            CancelInvoke(nameof(PlayFootseps));

    }

    private void PlayFootseps() => AudioManager.instance.PlaySFX(Random.Range(3, 6), true);

    private void GoToMainMenu() => UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
}
