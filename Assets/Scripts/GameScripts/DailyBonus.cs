using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DailyBonus : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    [SerializeField] private FinancialController financialController;
    private DateTime lastBonusTime;

    private void Start()
    {
        if(PlayerPrefs.HasKey("LastBonusTime"))
        {
            long ticks = Convert.ToInt64(PlayerPrefs.GetString("LastBonusTime"));
            lastBonusTime = new DateTime(ticks);
        }
        else
        {
            lastBonusTime = DateTime.Now;
            SaveLastBonus();
        }
    }
    private void Update()
    {
        TimeSpan timeSinceLastBonus = DateTime.Now - lastBonusTime;
        if (timeSinceLastBonus.TotalDays >= 1)
        {
            Debug.Log(financialController.money);
            financialController.money += 10000;
            lastBonusTime = DateTime.Now;
            SaveLastBonus();
            financialController.InformationOutput();
            financialController.SaveMoney();
            Debug.Log(financialController.money);
        }
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "" + financialController.money.ToString();
    }
    private void SaveLastBonus()
    {
        PlayerPrefs.SetString("LastBonusTime", lastBonusTime.Ticks.ToString());
        PlayerPrefs.Save();
    }
   
}
