using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, ISaveManager, ISaveManagerSettings
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

        Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID).ToArray();

        miniBosses = allEnemies.Where(enemy => enemy.IsAMiniBoss()).OrderBy(item => item.gameObject.name).ToArray();
        enemies = allEnemies.Where(enemy => !enemy.IsAMiniBoss()).OrderBy(item => item.gameObject.name).ToArray();

        // miniBosses = FindObjectsByType<Enemy>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
        // .Where(enemy => enemy.IsAMiniBoss())
        // .OrderBy(item => item.gameObject.name)
        // .ToArray();

        oneSideDoors = FindObjectsByType<OneSideDoor>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
        .OrderBy(item => item.gameObject.name)
        .ToArray();

        doors = FindObjectsByType<LevelExit>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToArray();

        tutorials = FindObjectsByType<TutorialEnabler>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
        .OrderBy(item => item.gameObject.name)
        .ToArray();

        // npcs = FindObjectsByType<NPC>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)
        // .OrderBy(npc => name)
        // .ToArray();

        for (int i = 0; i < items.Length; i++)
            levelItems.Add(false);

        for (int i = 0; i < illusoryWalls.Length; i++)
            levelIllusoryWalls.Add(false);

        for (int i = 0; i < miniBosses.Length; i++)
            levelMiniBosses.Add(false);

        for (int i = 0; i < oneSideDoors.Length; i++)
            levelOneSideDoors.Add(false);

        for (int i = 0; i < tutorials.Length; i++)
            levelTutorials.Add(false);

        for (int i = 0; i < enemies.Length; i++)
            levelEnemies.Add(false);
    }

    [SerializeField] private ItemObject[] items;
    [SerializeField] private IllusoryWall[] illusoryWalls;
    [SerializeField] private Enemy[] miniBosses;
    [SerializeField] private LevelExit[] doors;
    [SerializeField] private OneSideDoor[] oneSideDoors;
    [SerializeField] private TutorialEnabler[] tutorials;
    [SerializeField] private Enemy[] enemies;
    public CinemachineFollow cam { get; private set; }
    private List<bool> levelItems = new List<bool>();
    private List<bool> levelIllusoryWalls = new List<bool>();
    private List<bool> levelMiniBosses = new List<bool>();
    private List<bool> levelOneSideDoors = new List<bool>();
    private List<bool> levelTutorials = new List<bool>();
    private List<bool> levelEnemies = new List<bool>();
    public bool showTutorials { get; private set; }
    public bool crouchOffset  { get; private set; }

    private void Start()
    {
        cam = GameObject.Find("CinemachineCamera").GetComponent<CinemachineFollow>();
    }

    private void Update()
    {
        cam.FollowOffset = new Vector3(0, Mathf.Lerp(cam.FollowOffset.y, crouchOffset ? -3 : 3, Time.unscaledDeltaTime * 10), -10);
    }

    private IEnumerator OffsetRoutine(bool value, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        crouchOffset = value;
    }

    public void CrouchOffset(bool value, float delay = 0)
    {
        StopAllCoroutines();
        StartCoroutine(OffsetRoutine(value, delay));
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

    public void EnemyDefeated(Enemy enemy)
    {
        if (enemy.IsAMiniBoss())
        {
            for (int i = 0; i < miniBosses.Length; i++)
                if (miniBosses[i] != null && miniBosses[i] == enemy)
                    levelMiniBosses[i] = true;
        }
        else
        {
            for (int i = 0; i < enemies.Length; i++)
                if (enemies[i] != null && enemies[i] == enemy)
                    levelEnemies[i] = true;
        }
    }

    public void OneSideDoorOpened(OneSideDoor oneSideDoor)
    {
        for (int i = 0; i < oneSideDoors.Length; i++)
            if (oneSideDoors[i] != null && oneSideDoors[i] == oneSideDoor)
                levelOneSideDoors[i] = true;
    }

    public void TutorialRead(TutorialEnabler tutorial)
    {
        for (int i = 0; i < tutorials.Length; i++)
            if (tutorials[i] != null && tutorials[i] == tutorial)
                levelTutorials[i] = true;
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
            if (levelMiniBosses[i] && miniBosses[i] != null)
                Destroy(miniBosses[i].gameObject);

        for (int i = 0; i < oneSideDoors.Length; i++)
            if (levelOneSideDoors[i] && oneSideDoors[i] != null)
                oneSideDoors[i].Open();

        for (int i = 0; i < tutorials.Length; i++)
            if (levelTutorials[i] && tutorials[i] != null)
                tutorials[i].DestroyTutorial();

        for (int i = 0; i < enemies.Length; i++)
            if (levelEnemies[i] && enemies[i] != null)
                Destroy(enemies[i].gameObject);
    }

    public void LoadData(GameData data)
    {
        if (data.usedDoor)
        {
            foreach (LevelExit door in doors)
                if (door.index == data.doorIndex)
                {
                    PlayerManager.instance.player.transform.position = door.exitPoint.position;
                    cam.ForceCameraPosition(PlayerManager.instance.player.transform.position, quaternion.identity);
                    door.DisableCollider();
                    PlayerManager.instance.player.MoveTowardsObjective(door.enterPoint);
                    break;
                }
        }
        else if (data.spawnPosition != null && data.spawnPosition.Length == 3)
        {
            PlayerManager.instance.player.transform.position = new Vector3(data.spawnPosition[0], data.spawnPosition[1], data.spawnPosition[2]);
            cam.ForceCameraPosition(PlayerManager.instance.player.transform.position, quaternion.identity);
        }


        if (data.levels.TryGetValue(SceneManager.GetActiveScene().buildIndex, out string value))
        {
            for (int i = 0; i < levelItems.Count; i++)
                levelItems[i] = value[i] == 'T';

            for (int i = 0; i < levelIllusoryWalls.Count; i++)
                levelIllusoryWalls[i] = value[i + levelItems.Count] == 'T';

            for (int i = 0; i < levelMiniBosses.Count; i++)
                levelMiniBosses[i] = value[i + levelItems.Count + levelIllusoryWalls.Count] == 'T';

            for (int i = 0; i < levelOneSideDoors.Count; i++)
                levelOneSideDoors[i] = value[i + levelItems.Count + levelIllusoryWalls.Count + levelMiniBosses.Count] == 'T';

            for (int i = 0; i < levelTutorials.Count; i++)
                levelTutorials[i] = value[i + levelItems.Count + levelIllusoryWalls.Count + levelMiniBosses.Count + levelOneSideDoors.Count] == 'T';

            if (data.defeatedEnemies.TryGetValue(SceneManager.GetActiveScene().buildIndex, out string list))
                for (int i = 0; i < levelEnemies.Count; i++)
                    levelEnemies[i] = list[i] == 'T';
                
            CleanUp();
        }
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

        foreach (bool boolean in levelMiniBosses)
            helper += boolean ? 'T' : 'F';

        foreach (bool boolean in levelOneSideDoors)
            helper += boolean ? 'T' : 'F';

        foreach (bool boolean in levelTutorials)
            helper += boolean ? 'T' : 'F';

        foreach (KeyValuePair<int, string> keyValuePair in temporaryDictionary)
            data.levels.Add(keyValuePair.Key, keyValuePair.Value);

        data.levels.Add(SceneManager.GetActiveScene().buildIndex, helper);

        Dictionary<int, string> temporaryEnemiesDictionary = new Dictionary<int, string>();

        foreach (KeyValuePair<int, string> keyValuePair in data.defeatedEnemies)
            if (keyValuePair.Key != SceneManager.GetActiveScene().buildIndex)
                temporaryEnemiesDictionary.Add(keyValuePair.Key, keyValuePair.Value);

        data.defeatedEnemies.Clear();

        if (PlayerManager.instance.usedDoor)
        {
            string enemiesHelper = string.Empty;

            foreach (bool boolean in levelEnemies)
                enemiesHelper += boolean ? 'T' : 'F';

            foreach (KeyValuePair<int, string> keyValuePair in temporaryEnemiesDictionary)
                data.defeatedEnemies.Add(keyValuePair.Key, keyValuePair.Value);

            data.defeatedEnemies.Add(SceneManager.GetActiveScene().buildIndex, enemiesHelper);
        }
    }

    public void LoadData(SettingsData data)
    {
        showTutorials = data.showTutorials;
    }

    public void SaveData(ref SettingsData data)
    {

    }
}
