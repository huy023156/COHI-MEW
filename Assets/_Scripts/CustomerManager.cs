using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : Singleton<CustomerManager>
{
    [SerializeField] private Transform customerSpawnTransform;
    [SerializeField] private Transform customerPrefabs;

    private void Update()
    {
        if (GameManager.Instance.GetCurrentState() != GameState.playing)
        {
            return;
        }

        if (TableManager.Instance.TryFindEmtyTable(out Table table))
        {
            SpawnCustomer(new Order { item = ItemManager.Instance.GetItemSO(ItemType.cafe), rewardAmount = 100, timeLimit = 15 }, table);
        }
    }

    public void SpawnCustomer(Order order, Table table)
    {
        Transform customerTransform = Instantiate(customerPrefabs, customerSpawnTransform);
        Customer customer = customerTransform.GetComponent<Customer>();
        customer.SetUp(order, table); 
    }

    public Vector2 GetSpawnPosition() => customerSpawnTransform.position;
}
