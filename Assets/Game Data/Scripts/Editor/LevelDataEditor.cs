using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelData levelData = (LevelData)target;

        // Show default fields
        DrawDefaultInspector();

        if (GUILayout.Button("Reset Level"))
        {
            levelData.hasCompleted = false;
            levelData.currentXP = 0;
            foreach (var task in levelData.tasks)
            {
                task.ResetValues();
            }
        }

        if (GUILayout.Button("Simulate Level Completion"))
        {
            foreach (var task in levelData.tasks)
            {
                while (!task.complete)
                {
                    task.Execute();
                }
            }
        }
    }
}