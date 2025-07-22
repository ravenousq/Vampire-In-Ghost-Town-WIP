using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField] private string fileName;
    [SerializeField] private string settingsDataFileName;
    [SerializeField] private bool encryptData;
    private List<ISaveManager> saveManagers;
    private List<ISaveManagerSettings> settingsSaveManagers;

    [SerializeField] private GameData gameData;
    [SerializeField] private SettingsData settingsData;
    private FileDataHandler dataHandler;

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData, settingsDataFileName);
        saveManagers = FindAllSaveManagers();
        settingsSaveManagers = FindAllSettingsSaveManagers();

        LoadGame();
        LoadSettings();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            Debug.Log("No saved data found");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
            saveManager.LoadData(gameData);
    }

    public void LoadSettings()
    {
        settingsData = dataHandler.LoadSettings();

        if (settingsData == null)
        {
             Debug.Log("No settings save found");
            settingsData = new SettingsData();
        }

        foreach (ISaveManagerSettings saveManagerSettings in settingsSaveManagers)
            saveManagerSettings.LoadData(settingsData);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            SaveGame();
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
            saveManager.SaveData(ref gameData);
        
        dataHandler.Save(gameData);
    }

    public void SaveSettings()
    {
        foreach (ISaveManagerSettings saveManager in settingsSaveManagers)
            saveManager.SaveData(ref settingsData);

        dataHandler.SaveSettings(settingsData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
        SaveSettings();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    private List<ISaveManagerSettings> FindAllSettingsSaveManagers()
    {
        IEnumerable<ISaveManagerSettings> settingsSaveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<ISaveManagerSettings>();
        return new List<ISaveManagerSettings>(settingsSaveManagers);
    }

    [ContextMenu("Delete Saved File")]
    public void DeleteSavedData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData, settingsDataFileName);
        dataHandler.Delete();
    }

    [ContextMenu("Delete Saved Settings Files")]
    public void DeleteSettingsData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, false, settingsDataFileName);
        dataHandler.DeleteSettings();
    }

    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
            return true;

        return false;
    }

    public GameData GiveDataToButtons() => gameData;
}
