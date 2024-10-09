using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WorkingItemSO
{
    public bool isWorking;
    public ItemSO itemSO;
}

public class ItemGeneratorManager : Singleton<ItemGeneratorManager>
{
    private List<WorkingItemSO> workingItemSOList;

    protected override void Awake()
    {
        base.Awake();

        foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
        {
            WorkingItemSO workingItemSO = new WorkingItemSO { isWorking = false, itemSO = ItemManager.Instance.GetItemSO(itemType) };
            workingItemSOList.Add(workingItemSO);
        }

        workingItemSOList
            .Where(u => u.itemSO == ItemManager.Instance.GetItemSO(ItemType.cafe))
            .FirstOrDefault()
            .isWorking = true;
        workingItemSOList
            .Where(u => u.itemSO == ItemManager.Instance.GetItemSO(ItemType.orangeJuice))
            .FirstOrDefault()
            .isWorking = true;
    }
}
