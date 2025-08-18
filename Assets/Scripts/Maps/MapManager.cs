using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour, ISaveManager
{
    public static MapManager instance { get; private set; }
    [SerializeField] private GameObject maps;
    private List<GameObject> areas = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        FetchMaps();
    }

    [SerializeField] private GameObject currentMap;

    private void Start()
    {
        currentMap.SetActive(true);
    }

    private void FetchMaps()
    {
        if (areas.Count != 0)
            return;

        for (int i = 0; i < maps.transform.childCount; i++)
            if (maps.transform.GetChild(i).gameObject.name.Contains("Area"))
                areas.Add(maps.transform.GetChild(i).gameObject);
    }

    public void LoadData(GameData data)
    {
        FetchMaps();

        foreach (string map in data.maps)
        {
            foreach (GameObject tilemap in areas)
                if (tilemap.gameObject.name == map)
                {
                    tilemap.SetActive(true);
                    break;
                }
        }
    }

    public void SaveData(ref GameData data)
    {
        foreach (string map in data.maps)
        {
            if (currentMap.name == map)
                return;
        }

        data.maps.Add(currentMap.gameObject.name);
    }
}
