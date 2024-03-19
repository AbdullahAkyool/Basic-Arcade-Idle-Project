using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   [SerializeField] private TMP_Text moneyCount;

   private void Start()
   {
      ActionManager.Instance.onMoneyAmountChanged += SetMoneyCount;
   }

   private void OnDisable()
   {
      ActionManager.Instance.onMoneyAmountChanged -= SetMoneyCount;
   }

   private void SetMoneyCount(float moneyAmount)
   {
      moneyCount.text = moneyAmount.ToString();
   }
}
