using System.Data.Common;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item List", fileName = "Item Data Base")]
public class ItemDataBase : ScriptableObject
{
    public ItemData[] itemList;
}
