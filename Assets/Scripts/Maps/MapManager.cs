using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour, ISaveManager
{
    public static MapManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField] private GameObject currentMap;
    [SerializeField] private GameObject[] maps;

    private void Start()
    {
        currentMap.SetActive(true);
    }

    public void LoadData(GameData data)
    {
        foreach (string map in data.maps)
        {
            foreach (GameObject tilemap in maps)
                if (tilemap.gameObject.name == map)
                {
                    tilemap.SetActive(true);
                    break;
                }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (!data.maps.Contains(currentMap.gameObject.name))
            data.maps.Append(currentMap.gameObject.name);
    }
}
