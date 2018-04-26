using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Breach))]
[CanEditMultipleObjects]
public class BreachEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Breach myScript = (Breach)target;
        if (GUILayout.Button("Repositioning"))
        {
            myScript.NormalizePositions();
        }
    }
}
