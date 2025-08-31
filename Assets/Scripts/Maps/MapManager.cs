using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    private int sceneNumber;
    private Dictionary<int, MapCover[]> mapCovers = new Dictionary<int, MapCover[]>();
    private Dictionary<int, List<bool>> saveMapCovers = new Dictionary<int, List<bool>>();

    private void Start()
    {
        currentMap.SetActive(true);

        sceneNumber = int.Parse(currentMap.gameObject.name[^1].ToString());
    }

    private void FetchMaps()
    {
        if (areas.Count != 0)
            return;

        for (int i = 0; i < maps.transform.childCount; i++)
            if (maps.transform.GetChild(i).gameObject.name.Contains("Area"))
            {
                areas.Add(maps.transform.GetChild(i).gameObject);

                int areaNumber = int.Parse(maps.transform.GetChild(i).gameObject.name[^1].ToString());

                MapCover[] areaCovers = maps.transform.GetChild(i)
                .GetComponentsInChildren<MapCover>(true)
                .OrderBy(item => item.gameObject.name).ToArray();

                List<bool> saveData = new List<bool>();

                for (int j = 0; j < areaCovers.Length; j++)
                    saveData.Add(false);

                mapCovers.Add(areaNumber, areaCovers);
                saveMapCovers.Add(areaNumber, saveData);
            }
    }

    public void CoverDispelled(MapCover dispelled)
    {

        for (int i = 0; i < mapCovers[sceneNumber].Length; i++)
            if (mapCovers[sceneNumber][i] == dispelled)
            {
                saveMapCovers[sceneNumber][i] = true;
                Destroy(dispelled.gameObject);
            }
    }

    private void CleanUp()
    {
        for (int i = 1; i <= saveMapCovers.Count; i++)
            for (int j = 0; j < saveMapCovers[i].Count; j++)
                if (saveMapCovers[i][j])
                    mapCovers[i][j].Dispell();
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

        if (data.mapCovers == null)
            return;

        for (int i = 0; i < data.mapCovers.Length; i++)
            if(data.mapCovers[i]  != string.Empty)
            for (int j = 0; j < data.mapCovers[i].Length; j++)
                saveMapCovers[i + 1][j] = data.mapCovers[i][j] == 'T';

        CleanUp();
    }

    public void SaveData(ref GameData data)
    {
        data.mapCovers = new string[saveMapCovers.Count];

        foreach (KeyValuePair<int, List<bool>> pair in saveMapCovers)
        {
            string helper = string.Empty;

            foreach (bool boolean in pair.Value)
                helper += boolean ? 'T' : 'F';

            data.mapCovers[pair.Key - 1] = helper;
        }

        foreach (string map in data.maps)
            if (currentMap.name == map)
                return;

        data.maps.Add(currentMap.gameObject.name);
    }
}
