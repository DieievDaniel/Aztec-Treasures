using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FinancialController : MonoBehaviour
{
    [SerializeField] private AutoRolling autoRolling;
    [SerializeField] private int rate;
    [SerializeField] public float money = 10000;
    [SerializeField] public float prize;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI rateText;
    public TextMeshProUGUI prizeText;
    public TextMeshProUGUI rollText;

    private DateTime lastBonusTime;

    private void Start()
    {
        lastBonusTime = DateTime.Now;
       LoadMoney();
        InformationOutput();
    }

    public int Rate
    {
        get { return rate; }
        set { rate = value; }
    }

    public void InformationOutput()
    {
        moneyText.text = "" + money.ToString();
        rateText.text = "" + rate.ToString();
        prizeText.text = "" + prize.ToString();
        rollText.text = "" + autoRolling.numberOfRolling.ToString();
    }

    public void Rolling()
    {
        if(money >= rate)
        {
            money -= rate;
            InformationOutput();
        }
    }

    private void OnApplicationQuit()
    {
        SaveMoney();
    }

    public void SaveMoney()
    {
        PlayerPrefs.SetFloat("Money", money);
        PlayerPrefs.Save();
    }
    public void LoadMoney()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetFloat("Money");
        }
    }
    public void ResetMoney()
    {
       
    }

   

}
