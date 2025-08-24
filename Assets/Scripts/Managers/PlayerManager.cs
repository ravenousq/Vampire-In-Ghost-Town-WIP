using System.Collections;
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
    [SerializeField] private Bloodstain bloodstainPrefab;
    [SerializeField] private GameObject bloodstainIconPrefab;
    [SerializeField] private Transform maps;
    public int currency { get; private set; } = 10000;
    public int lastSceneName { get; private set; }
    public bool usedDoor { get; private set; }
    private bool playerIsDead;
    private bool bloodstainExists;
    private int doorIndexToSave = -1;
    private GameObject bloodstainIcon;

    private void Start()
    {
        if(!AudioManager.instance.isPlayingBGM(10))
            AudioManager.instance.PlayBGM(10);
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
        bloodstainExists = data.bloodstainExists;

        if (data.bloodstainExists)
        {
            bloodstainIcon = Instantiate(bloodstainIconPrefab, new Vector3(maps.transform.position.x + data.bloodstainIcon[0], maps.transform.position.y + data.bloodstainIcon[1]), Quaternion.identity);

            if (SceneManager.GetActiveScene().buildIndex == data.bloodstainScene)
                Instantiate(bloodstainPrefab).SetUpBloodstain(new Vector3(data.bloodstainPosition[0], data.bloodstainPosition[1], 0), data.bloodstainCurrency);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.lastScene = SceneManager.GetActiveScene().buildIndex;

        data.doorIndex = doorIndexToSave == -1 ? data.doorIndex : doorIndexToSave;
        data.usedDoor = usedDoor;

        data.bloodstainExists = bloodstainExists;

        if (!playerIsDead)
            data.currency = currency;

        else
        {
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2 mapsPos = new Vector2(maps.transform.position.x, maps.transform.position.y);
            data.currency = 0;
            data.bloodstainCurrency = currency;
            data.bloodstainScene = SceneManager.GetActiveScene().buildIndex;
            data.bloodstainPosition = new float[] { playerPos.x, playerPos.y };
            data.bloodstainIcon = new float[] { playerPos.x - mapsPos.x, playerPos.y - mapsPos.y };
        }
    }

    public void Die()
    {
        UI.instance.ModifySouls(-currency);
        bloodstainExists = true;
        playerIsDead = true;
    }

    public void RecoveredSouls(int soulsToRecoever)
    {
        UI.instance.bloodstain.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX(32);
        AddCurrency(soulsToRecoever);
        bloodstainExists = false;
        Destroy(bloodstainIcon);
    }

    public void ExitLevel(Transform objective, string targetScene, int targetIndex)
    {
        player.MoveTowardsObjective(objective);

        doorIndexToSave = targetIndex;
        usedDoor = true;

        StartCoroutine(Leave(targetScene));
    }

    private IEnumerator Leave(string targetScene)
    {
        UI.instance.fadeScreen.FadeIn();
        SaveManager.instance.SaveGame();
        SaveManager.instance.SaveSettings();

        yield return new WaitForSecondsRealtime(1.5f);

        SceneManager.LoadScene(targetScene);
    }
}