
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using  UnityEditor;

[CustomEditor(typeof(MeshAroundAreaBorder))]

public class PointOnAreaBorderEditor : Editor
{
    MeshAroundAreaBorder meshAroundAreaBorder;

    private void OnEnable()
    {
        meshAroundAreaBorder = (MeshAroundAreaBorder) target;
        
    }
}
