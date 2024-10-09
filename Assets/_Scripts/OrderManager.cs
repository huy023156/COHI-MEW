using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OrderManager : Singleton<OrderManager>
{

    public Order CreateOrder(ItemSO itemSO, float timeLimit, int rewardAmount)
    {
        return new Order(itemSO, timeLimit, rewardAmount);
    }


}
