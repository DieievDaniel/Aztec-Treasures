using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private AutoRolling autoRolling;
    [SerializeField] private FinancialController financialController;


    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button rollButton;
    [SerializeField] private Button decreaseRollButton;
    


   public void PlayGame()
    {
        financialController.InformationOutput();
        financialController.Rolling();
        
    }
    private void Update()
    {
        TurnOffButton();
    }

    public void TurnOffButton()
    {
        int totalCount = financialController.Rate * autoRolling.numberOfRolling;
        var buttons = FindObjectsOfType<Button>();
        if (financialController.money < financialController.Rate || ObjectGeneration.isRolling || autoRolling.isAutoRolling)
        {
            foreach (var button in buttons)
            {
                button.interactable = false;
            }
        }
        else if (financialController.Rate < 50 )
        {
            foreach (var button in buttons)
            {
                if (button!=increaseButton)
                {
                    button.interactable = false;
                }
            }
        }
        else if(financialController.money < totalCount)
        {
            foreach(var button in buttons)
            {
                if(button != autoRolling.autoRollButton)
                {
                    button.interactable = true;
                }
            }
        }
        else if(autoRolling.numberOfRolling <= 0)
        {
            decreaseRollButton.interactable = false;
            autoRolling.autoRollButton.interactable = false;
        }
        else
        {
            foreach(var button in buttons)
            {
                button.interactable = true;
            }
        }
    }

    public void IncreaseRate()
    {
        financialController.Rate += 50;
        financialController.InformationOutput();
    }
    public void DecreaseRate()
    {
        if(financialController.Rate >0)
        {
            financialController.Rate -= 50;
            financialController.InformationOutput();
        }
    }
    public void IncreaseRollButton()
    {
        autoRolling.numberOfRolling += 1;
        financialController.InformationOutput();
    }
    public void DecreaseRollButton()
    {
        if (autoRolling.numberOfRolling > 0)
        {
            autoRolling.numberOfRolling -= 1;
            financialController.InformationOutput();
        }
    }
}
