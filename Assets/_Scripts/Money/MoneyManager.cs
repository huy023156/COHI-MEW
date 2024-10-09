using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private Transform moneyPrefab;
    [SerializeField] private float moneySpreadAmount = .3f;
    [SerializeField] private int moneyPrefabDrop = 4;

    public void SpawnMoney(Transform positionTransform, out List<Transform> moneyTransformList) 
    {
        moneyTransformList = new List<Transform>();
        Vector3 randomDir;
    
        for (int i = 0; i < moneyPrefabDrop; i++)
        {
            randomDir = new Vector2(Random.Range(-moneySpreadAmount, moneySpreadAmount), Random.Range(-moneySpreadAmount, moneySpreadAmount));
            Transform money = Instantiate(moneyPrefab, positionTransform.position + randomDir, Quaternion.identity, positionTransform);
            moneyTransformList.Add(money);
        }
    }
}
