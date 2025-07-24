using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, ISaveManager
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
    public int lastSceneName { get; private set; }

    private void Start() => AudioManager.instance.PlayBGM(10);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            Debug.Log(currency);    
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

    public void LoadData(GameData data)
    {
        currency = data.currency;
        lastSceneName = data.lastScene;
        UI.instance.ModifySouls();
    }

    public void SaveData(ref GameData data)
    {
        data.currency = currency;
        data.lastScene = SceneManager.GetActiveScene().buildIndex;
    }
}