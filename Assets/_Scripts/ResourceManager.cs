using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public event Action<int> OnMoneyAmountChanged;

    [SerializeField] private int initialMoneyAmount = 100;
    private int moneyAmount;

    protected override void Awake()
    {
        base.Awake();
        moneyAmount = initialMoneyAmount;
    }

    private IEnumerator Start()
    {
        yield return null;
        OnMoneyAmountChanged?.Invoke(initialMoneyAmount);
    }

    public bool IsAfford(int amount)
    {
        if (moneyAmount < amount)
        {
            return false;
        }

        return true;
    }

    public bool TrySpendMoney(int amount)
    {
        if (IsAfford(amount))
        {
            moneyAmount -= amount;
            OnMoneyAmountChanged?.Invoke(moneyAmount);
            return true;
        }

        return false;
    }

    public void AddMoney(int amount)
    {
        moneyAmount += amount;
        OnMoneyAmountChanged?.Invoke(moneyAmount);
    }
}
