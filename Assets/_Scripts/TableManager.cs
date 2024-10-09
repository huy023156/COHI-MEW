using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : Singleton<TableManager>
{
    [SerializeField] private Table[] tableArray;
    private List<Table> unlockedTableList;

    protected override void Awake()
    {
        base.Awake();

        unlockedTableList = new List<Table>();
    }

    private void Start()
    {
        Table.OnAnyTalbeUnlocked += Table_OnAnyTalbeUnlocked;
    }

    public bool IsAnyTableUnlocked() => unlockedTableList.Count > 0;

    private void Table_OnAnyTalbeUnlocked(Table table)
    {
        unlockedTableList.Add(table);
    }

    public bool TryFindEmtyTable(out Table foundTable)
    {
        foreach (Table table in unlockedTableList)
        {
            if (table.IsEmpty())
            {
                foundTable = table;
                return true;
            }
        }

        foundTable = null;
        return false;
    }
}
