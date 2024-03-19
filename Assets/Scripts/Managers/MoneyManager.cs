using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;
    
    public float totalMoney;

    private void Awake()
    {
        Instance = this;
    }

    public void EarnMoney(float value)
    {
        UpdateMoneyCount(totalMoney + value);
    }

    public void SpendMoney(float value)
    {
        UpdateMoneyCount(totalMoney - value);

        if (totalMoney < 0) totalMoney = 0;
    }

    private void UpdateMoneyCount(float currentAmount)
    {
        totalMoney = currentAmount;
        ActionManager.Instance.onMoneyAmountChanged?.Invoke(totalMoney);
    }
}
