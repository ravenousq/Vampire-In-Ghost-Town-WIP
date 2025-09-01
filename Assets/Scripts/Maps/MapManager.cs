using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.AI;

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
    private MapCover[] mapCoversSimple;
    private List<bool> saveMapCoversSimple = new List<bool>();

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

        mapCoversSimple = maps.GetComponentsInChildren<MapCover>(true).OrderBy(item => item.gameObject.name).ToArray();

        for (int j = 0; j < mapCoversSimple.Length; j++)
                    saveMapCoversSimple.Add(false);
    }

    public void CoverDispelled(MapCover dispelled)
    {
        for (int i = 0; i < mapCoversSimple.Length; i++)
            if (mapCoversSimple[i] == dispelled)
            {
                saveMapCoversSimple[i] = true;
                Destroy(dispelled.gameObject);
            }
    }

    private void CleanUp()
    {
        for (int i = 0; i < saveMapCoversSimple.Count; i++)
            if (saveMapCoversSimple[i])
                mapCoversSimple[i].Dispell();
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

        if (data.mapCoversSimple == null)
            return;

        for (int i = 0; i < data.mapCoversSimple.Length; i++)
            saveMapCoversSimple[i] = data.mapCoversSimple[i] == 'T';

        CleanUp();
    }

    public void SaveData(ref GameData data)
    {
        string simpleHelper = string.Empty;

        foreach (bool boolean in saveMapCoversSimple)
            simpleHelper += boolean ? 'T' : 'F';

        data.mapCoversSimple = simpleHelper;

        foreach (string map in data.maps)
            if (currentMap.name == map)
                return;

        data.maps.Add(currentMap.gameObject.name);
    }
}
