using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridSize
{
    Size3x4,
    Size4x5,
    Size5x6,
    Size6x7
}


public static class GridSizeManager
{
    public static int Rows { get; set; }
    public static int Columns { get; set; }
    public static GridSize SelectedSize { get; set; }
}

