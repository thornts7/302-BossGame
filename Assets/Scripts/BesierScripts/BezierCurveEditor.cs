using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveEditor : Editor
{

    int selectedIndex = -1;

    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Add Point"))
        {
            (target as BezierCurve).AddPoint();
            EditorUtility.SetDirty(target);
        }

        //GUI.Button(new Rect(0,0,100,30), "Add Point");

        // add more to it...
    }

    void OnSceneGUI()
    {
        BezierCurve curve = (BezierCurve)target;



        for (int i = 0; i < curve.worldPoints.Length; i++)
        {
            if (Handles.Button(curve.worldPoints[i], Quaternion.identity, .1f, .05f, Handles.CubeHandleCap))
            {
                selectedIndex = i;
            }
        }

        if (selectedIndex >= 0)
        {

            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(curve.worldPoints[selectedIndex], Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed path points");
                curve.points[selectedIndex] = curve.transform.InverseTransformDirection(newPos);
                curve.casheSplineData();
            }
        }

    }

}