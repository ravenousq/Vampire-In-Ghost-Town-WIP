using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> charmsID = new List<string>();

    public SerializableDictionary<string, bool> campfires;
    public string lastCampfire;

    public GameData()
    {
        currency = 99999;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        charmsID = new List<string>();

        campfires = new SerializableDictionary<string, bool>();
        lastCampfire = string.Empty;
    }
}
