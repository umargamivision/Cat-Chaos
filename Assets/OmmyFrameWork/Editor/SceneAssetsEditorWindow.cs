#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Ommy.Editor.SceneManagement
{
    [InitializeOnLoad]
    public class SceneIconsInSceneView
    {
        static SceneIconsInSceneView()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        static bool showScenes = true;

        private static void OnSceneGUI(SceneView sceneView)
        {
            Handles.BeginGUI();
            var scenes = EditorBuildSettings.scenes;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Scenes", EditorStyles.boldLabel, GUILayout.Width(50), GUILayout.Height(30)))
            {
                showScenes = !showScenes;
            }

            foreach (var scene in scenes)
            {
                if (showScenes && scene.enabled)
                {
                    var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
                    if (sceneAsset != null)
                    {
                        GUILayout.BeginVertical(GUILayout.Width(30)); // Adjusted width for better layout
                        if (GUILayout.Button(EditorGUIUtility.IconContent("d_SceneAsset Icon"), GUILayout.Width(30), GUILayout.Height(30)))
                        {
                            HandleButtonClick(sceneAsset);
                        }
                        GUILayout.Label(sceneAsset.name, GUILayout.Width(30), GUILayout.Height(20));

                        GUILayout.EndVertical();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
            Handles.EndGUI();
        }

        private static void HandleButtonClick(SceneAsset sceneAsset)
        {
            Event currentEvent = Event.current;
            if (currentEvent != null)
            {
                if (currentEvent.button == 0) // Left-click
                {
                    LoadSceneNormally(sceneAsset);
                }
                else if (currentEvent.button == 1) // Right-click
                {
                    LoadSceneAdditively(sceneAsset);
                }
            }
        }

        private static void LoadSceneNormally(SceneAsset sceneAsset)
        {
            if (sceneAsset != null)
            {
                string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                if (!string.IsNullOrEmpty(scenePath))
                {
                    EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                }
            }
        }

        private static void LoadSceneAdditively(SceneAsset sceneAsset)
        {
            if (sceneAsset != null)
            {
                string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                if (!string.IsNullOrEmpty(scenePath))
                {
                    EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                }
            }
        }
    }
}
#endif