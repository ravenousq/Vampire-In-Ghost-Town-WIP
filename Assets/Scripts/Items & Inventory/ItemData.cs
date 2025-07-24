using System;
using System.Text;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Note,
    KeyItem,
    Charm,
}

[CreateAssetMenu(fileName = "New Note", menuName = "Data/Note")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public int price;
    public Sprite icon;
    [TextArea(1, 10)]
    public string itemDescription;
    [TextArea]
    public string noteContent;
    public string itemID;
    protected StringBuilder sb = new StringBuilder();

    public virtual string GetDescription()
    {
        return "";
    }
        void OnValidate()
    {

    #if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
    #endif
    }
}
