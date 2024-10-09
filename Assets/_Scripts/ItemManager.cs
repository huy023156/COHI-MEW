using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    cafe,
    orangeJuice,
    milkTea,
    cake,
    iceCream
}

[Serializable]
public struct ItemTypeItemSO
{
    public ItemType itemType;
    public ItemSO itemSO;
}

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] private List<ItemTypeItemSO> itemTypeItemSOList;
    public ItemSO GetItemSO(ItemType type)
    {
        foreach (ItemTypeItemSO itemTypeItemSO in itemTypeItemSOList)
        {
            if (itemTypeItemSO.itemType == type)
            {
                return itemTypeItemSO.itemSO;
            }
        }

        return null;
    }
}
