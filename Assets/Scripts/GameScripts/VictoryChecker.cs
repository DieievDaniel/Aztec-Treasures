using System.Collections.Generic;
using UnityEngine;

public class VictoryChecker
{
    public static bool CheckForWin(Dictionary<Sprite, int> elementCounts, Sprite[] objectSprite, int rows, int columns)
    {
        foreach (var key in elementCounts.Keys)
        {
            int elementIndex = GetElementIndex(key, objectSprite);
            int count = elementCounts[key];

            float coefficient = ElementMultiplier.GetMultiplier(elementIndex, count, rows, columns);

            if (coefficient > 0 && count >= GetWinningCount(rows, columns))
            {
                return true;
            }
        }

        return false;
    }

    private static int GetElementIndex(Sprite sprite, Sprite[] objectSprite)
    {
        for (int i = 0; i < objectSprite.Length; i++)
        {
            if (objectSprite[i] == sprite)
            {
                return i;
            }
        }
        return -1;
    }

    // Метод для определения выигрышного количества элементов в зависимости от размеров сетки
    private static int GetWinningCount(int rows, int columns)
    {
        // Возвращаем значение в зависимости от размера сетки
        if (rows == 3 && columns == 4)
        {
            return 5; // Минимальное количество элементов для выигрыша для сетки 3x4
        }
        else if (rows == 4 && columns == 5)
        {
            return 7; // Минимальное количество элементов для выигрыша для сетки 4x5
        }
        else if (rows == 5 && columns == 6)
        {
            return 8; // Минимальное количество элементов для выигрыша для сетки 5x6
        }
        else if (rows == 6 && columns == 7)
        {
            return 9; // Минимальное количество элементов для выигрыша для сетки 6x7
        }

        // Возвращаем значение по умолчанию
        return 0;
    }
}
