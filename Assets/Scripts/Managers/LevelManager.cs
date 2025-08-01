using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, ISaveManager
{
    public static LevelManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        items = FindObjectsByType<ItemObject>(FindObjectsInactive.Include, FindObjectsSortMode.None)
        .OrderBy(item => item.gameObject.name)
        .ToArray();

        illusoryWalls = FindObjectsByType<IllusoryWall>(FindObjectsInactive.Include, FindObjectsSortMode.None)
        .OrderBy(item => item.gameObject.name)
        .ToArray();

        miniBosses = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
        .Where(enemy => enemy.IsAMiniBoss())
        .OrderBy(item => item.gameObject.name)
        .ToArray();

        doors = FindObjectsByType<LevelExit>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToArray();

        cam = GameObject.Find("CinemachineCamera").GetComponent<CinemachineFollow>();

        // npcs = FindObjectsByType<NPC>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
        // .OrderBy(npc => name)
        // .ToArray();

        for (int i = 0; i < items.Length; i++)
            levelItems.Add(false);

        for (int i = 0; i < illusoryWalls.Length; i++)
            levelIllusoryWalls.Add(false);

        for (int i = 0; i < miniBosses.Length; i++)
            levleMiniBosses.Add(false);
    }

    [SerializeField] private ItemObject[] items;
    [SerializeField] private IllusoryWall[] illusoryWalls;
    [SerializeField] private Enemy[] miniBosses;
    [SerializeField] private LevelExit[] doors;
    [Space]
    [SerializeField] private CinemachineFollow cam;
    private List<bool> levelItems = new List<bool>();
    private List<bool> levelIllusoryWalls = new List<bool>();
    private List<bool> levleMiniBosses = new List<bool>();

    private void Start()
    {
        //CleanUp();    
    }

    public void ItemFound(ItemObject item)
    {
        for (int i = 0; i < items.Length; i++)
            if (items[i] != null && items[i] == item)
                levelItems[i] = true;
    }

    public void IllusoryWallFound(IllusoryWall illusoryWall)
    {
        for (int i = 0; i < illusoryWalls.Length; i++)
            if (illusoryWalls[i] != null && illusoryWalls[i] == illusoryWall)
                levelIllusoryWalls[i] = true;
    }

    public void MiniBossDefeated(Enemy miniBoss)
    {
        for (int i = 0; i < miniBosses.Length; i++)
            if (miniBosses[i] != null && miniBosses[i] == miniBoss)
                levleMiniBosses[i] = true;
    }

    private void CleanUp()
    {
        for (int i = 0; i < items.Length; i++)
            if (levelItems[i] && items[i] != null)
                Destroy(items[i].gameObject);

        for (int i = 0; i < illusoryWalls.Length; i++)
            if (levelIllusoryWalls[i] && illusoryWalls[i] != null)
                Destroy(illusoryWalls[i].gameObject);

        for (int i = 0; i < miniBosses.Length; i++)
            if (levleMiniBosses[i] && miniBosses[i] != null)
                Destroy(miniBosses[i].gameObject);
    }

    public void LoadData(GameData data)
    {
        if (data.usedDoor)
            foreach (LevelExit door in doors)
                if (door.index == data.doorIndex)
                {
                    PlayerManager.instance.player.transform.position = door.exitPoint.position;
                    cam.ForceCameraPosition(PlayerManager.instance.player.transform.position, quaternion.identity);
                    door.DisableCollider();
                    PlayerManager.instance.player.MoveTowardsObjective(door.enterPoint);
                    break;   
                }
            
        if (data.levels.TryGetValue(SceneManager.GetActiveScene().buildIndex, out string value))
            {
                for (int i = 0; i < levelItems.Count; i++)
                    levelItems[i] = value[i] == 'T';

                for (int i = 0; i < levelIllusoryWalls.Count; i++)
                    levelIllusoryWalls[i] = value[i + levelItems.Count] == 'T';

                for (int i = 0; i < levleMiniBosses.Count; i++)
                    levleMiniBosses[i] = value[i + levelItems.Count + levelIllusoryWalls.Count] == 'T';

                CleanUp();
            }

        #region Old Testing
        // foreach (var keyValuePair in data.levels)
        // {
        //     Debug.Log(keyValuePair.Key + ": ");
        //     foreach (bool boolean in keyValuePair.Value)
        //         Debug.Log(boolean);
        // }
        // foreach (var item in data.levels)
        // {
        //     if (item.Key == SceneManager.GetActiveScene().buildIndex)
        //     {
        //         List<bool> saveData = new List<bool>(item.Value);

        //         for (int i = 0; i < levelItems.Count; i++)
        //             levelItems[i] = saveData[i];

        //         for (int i = 0; i < levelIllusoryWalls.Count; i++)
        //             levelIllusoryWalls[i] = saveData[i + levelItems.Count];

        //         for (int i = 0; i < levleMiniBosses.Count; i++)
        //             levleMiniBosses[i] = saveData[i + levelItems.Count + levelIllusoryWalls.Count];

        //         return;
        //     }
        // }
        #endregion
    }

    public void SaveData(ref GameData data)
    {
        Dictionary<int, string> temporaryDictionary = new Dictionary<int, string>();

        foreach (KeyValuePair<int, string> keyValuePair in data.levels)
            if (keyValuePair.Key != SceneManager.GetActiveScene().buildIndex)
                temporaryDictionary.Add(keyValuePair.Key, keyValuePair.Value);

        data.levels.Clear();

        string helper = string.Empty;

        foreach (bool boolean in levelItems)
            helper += boolean ? 'T' : 'F';

        foreach (bool boolean in levelIllusoryWalls)
            helper += boolean ? 'T' : 'F';

        foreach (bool boolean in levleMiniBosses)
            helper += boolean ? 'T' : 'F';

        foreach (KeyValuePair<int, string> keyValuePair in temporaryDictionary)
            data.levels.Add(keyValuePair.Key, keyValuePair.Value);

        data.levels.Add(SceneManager.GetActiveScene().buildIndex, helper);

        #region OldTesting
            // Dictionary<int, List<bool>> temporaryDictionary = new Dictionary<int, List<bool>>();

        // foreach (KeyValuePair<int, List<bool>> keyValuePair in data.levels)
        //     if (keyValuePair.Key != SceneManager.GetActiveScene().buildIndex)
        //         temporaryDictionary.Add(keyValuePair.Key, keyValuePair.Value);

        // data.levels.Clear();

        // List<bool> saveData = new List<bool>();

        // saveData.AddRange(levelItems);
        // saveData.AddRange(levelIllusoryWalls);
        // saveData.AddRange(levleMiniBosses);

        // data.levels.Add(SceneManager.GetActiveScene().buildIndex, saveData);

        // foreach (var keyValuePair in data.levels)
        // {
        //     Debug.Log(keyValuePair.Key + ": ");
        //     foreach (bool boolean in keyValuePair.Value)
        //         Debug.Log(boolean);

        // }

        // foreach (KeyValuePair<int, List<bool>> keyValuePair in temporaryDictionary)
        //         data.levels.Add(keyValuePair.Key, keyValuePair.Value);
        #endregion
    }
}
