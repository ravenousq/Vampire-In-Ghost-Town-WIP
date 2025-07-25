using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int currency;
    public int lastScene;
    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> charmsID = new List<string>();
    public SerializableDictionary<int, string> levels;
    public SerializableDictionary<string, int> npcs;
    public SerializableDictionary<string, string> npcShops;


    public GameData()
    {
        currency = 99999;
        lastScene = 1;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        charmsID = new List<string>();
        levels = new SerializableDictionary<int, string>();
        npcs = new SerializableDictionary<string, int>();
        npcShops = new SerializableDictionary<string, string>();
    }
}
