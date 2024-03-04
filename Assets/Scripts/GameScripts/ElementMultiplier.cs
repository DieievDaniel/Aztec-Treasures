using UnityEngine;
using System.Collections.Generic;

public class ElementMultiplier : MonoBehaviour
{
    public static float[][] elementMultiplier = new float[][]
    {
        new float[] {20, 50, 100},    // Элемент 1
        new float[] {5, 20, 50},       // Элемент 2
        new float[] {3, 4, 24},        // Элемент 3
        new float[] {4, 10, 30},       // Элемент 4
        new float[] {2, 3, 20},        // Элемент 5
        new float[] {1.6f, 2.40f, 50}, // Элемент 6
        new float[] {1, 2, 10},        // Элемент 7
        new float[] {0.5f, 1.5f, 4}    // Элемент 8
    };

    public static float GetMultiplier(int indexOfElement, int count, int rows, int columns)
    {
        if (rows == 3 && columns == 4) // для сетки 3x4
        {
            if (count == 5)
            {
                return elementMultiplier[indexOfElement][0];
            }
            else if (count >= 6 && count < 7)
            {
                return elementMultiplier[indexOfElement][1];
            }
            else if (count >= 8)
            {
                return elementMultiplier[indexOfElement][2];
            }
            else
            {
                return 0;
            }
        }
        else  if (rows == 4 && columns == 5) // для сетки 3x4
        {
            if (count == 7)
            {
                return elementMultiplier[indexOfElement][0];
            }
            else if (count >= 8 && count < 9)
            {
                return elementMultiplier[indexOfElement][1];
            }
            else if (count >= 9)
            {
                return elementMultiplier[indexOfElement][2];
            }
            else
            {
                return 0;
            }
        }
        else if (rows == 5 && columns == 6) // для сетки 3x4
        {
            if (count == 8)
            {
                return elementMultiplier[indexOfElement][0];
            }
            else if (count >= 9 && count < 10)
            {
                return elementMultiplier[indexOfElement][1];
            }
            else if (count >= 10)
            {
                return elementMultiplier[indexOfElement][2];
            }
            else
            {
                return 0;
            }
        }
        else
        {
            if (count >= 9 && count < 11)
            {
                return elementMultiplier[indexOfElement][0];
            }
            else if (count >= 11 && count < 12)
            {
                return elementMultiplier[indexOfElement][1];
            }
            else if (count >= 13)
            {
                return elementMultiplier[indexOfElement][2];
            }
            else
            {
                return 0;
            }
        }
    }
}