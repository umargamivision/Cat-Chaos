using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Omar.Task))]
public class TaskEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Omar.Task task = (Omar.Task)target;

        // Show default fields
        DrawDefaultInspector();

        // Warnings for incorrect settings
        if (task.countsForCompletion <= 0)
        {
            EditorGUILayout.HelpBox("Counts for completion must be greater than 0.", MessageType.Warning);
        }

        if (string.IsNullOrEmpty(task.discription))
        {
            EditorGUILayout.HelpBox("Description is missing.", MessageType.Warning);
        }

        if (GUILayout.Button("Reset Values"))
        {
            task.ResetValues();
        }
    }
}