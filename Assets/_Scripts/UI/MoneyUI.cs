using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Start()
    {
        ResourceManager.Instance.OnMoneyAmountChanged += Instance_OnMoneyAmountChanged;
    }

    private void Instance_OnMoneyAmountChanged(int amount)
    {
        moneyText.text = amount.ToString();
    }
}
