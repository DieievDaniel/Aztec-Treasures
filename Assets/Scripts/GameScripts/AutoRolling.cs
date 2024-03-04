using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoRolling : MonoBehaviour
{
    private int rollCount;
    [SerializeField] public int numberOfRolling = 1;
    [SerializeField] private ObjectGeneration objectGeneration;
    [SerializeField] private FinancialController financialController;
    [SerializeField] private ButtonManager buttonManager;
    [SerializeField] public Button autoRollButton;
    private GridElementManager gridElementManager;
    
    public bool isAutoRolling = false;

    public void AutoRoll()
    {
        int totalCost = financialController.Rate * numberOfRolling;
        if (financialController.money >= totalCost)
        {
            StartCoroutine(StartGenerating());
        }
        else
        {
            autoRollButton.interactable = false;
        }
    }

    private IEnumerator StartGenerating()
    {
        isAutoRolling = true;
        for (rollCount = 0; rollCount < numberOfRolling; rollCount++)
        {
            financialController.money -= financialController.Rate;
            financialController.InformationOutput();
            objectGeneration.GenerateObjects();
            yield return new WaitForSeconds(3f);

            // Ждем, пока процесс генерации объектов завершится
            yield return StartCoroutine(WaitForObjectGeneration());

            // Проверяем наличие выигрышных комбинаций
            if (!VictoryChecker.CheckForWin(objectGeneration.elementCounts, objectGeneration.objectSprite, GridSizeManager.Rows, GridSizeManager.Columns))
            {
                Debug.Log("Roll count: " + (rollCount + 1));
            }
            else
            {
                // Ждем, пока выигрышные комбинации не исчезнут
                yield return new WaitUntil(() => !VictoryChecker.CheckForWin(objectGeneration.elementCounts, objectGeneration.objectSprite, GridSizeManager.Rows, GridSizeManager.Columns));
                yield return new WaitForSeconds(2f);
            }

            isAutoRolling = false;
        }

       
        
    }
    private IEnumerator WaitForObjectGeneration()
    {
        // Ждем, пока процесс генерации объектов не завершится
        while (ObjectGeneration.isRolling)
        {
            yield return null;
        }
    }

}
