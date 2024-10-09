using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Order 
{
    public ItemSO item;
    public float timeLimit;
    public int rewardAmount;

    public Order(ItemSO item, float timeLimit, int rewardAmount)
    {
        this.item = item;
        this.timeLimit = timeLimit;
        this.rewardAmount = rewardAmount;
    }
}
