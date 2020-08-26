
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using  UnityEditor;

[CustomEditor(typeof(PointsOnAreaBorder))]

public class PointOnAreaBorderEditor : Editor
{
    PointsOnAreaBorder pointsOnAreaBorder;

    private void OnEnable()
    {
        pointsOnAreaBorder = (PointsOnAreaBorder) target;
        
    }
}
