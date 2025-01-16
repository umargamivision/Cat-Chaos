using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using System.Collections.Generic;
namespace Ommy.Attribute
{

    [CustomEditor(typeof(MonoBehaviour), true)]
    public class InspectorButtonDrawer : Editor
    {
        private class MethodData
        {
            public MethodInfo Method;
            public object[] Parameters;
        }

        private Dictionary<MethodInfo, object[]> _methodParameters = new Dictionary<MethodInfo, object[]>();

        public override void OnInspectorGUI()
        {
            // Draw default Inspector UI
            DrawDefaultInspector();

            // Get the target MonoBehaviour
            MonoBehaviour targetMonoBehaviour = (MonoBehaviour)target;

            // Get all methods of the target script
            MethodInfo[] methods = targetMonoBehaviour.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                // Check if the method has the InspectorButtonAttribute
                var attribute = method.GetCustomAttribute<InspectorButtonAttribute>();
                if (attribute == null) continue;

                string buttonLabel = attribute.ButtonLabel ?? method.Name;

                // Check and display parameters
                ParameterInfo[] parameters = method.GetParameters();
                if (!_methodParameters.ContainsKey(method))
                {
                    _methodParameters[method] = new object[parameters.Length];
                }

                for (int i = 0; i < parameters.Length; i++)
                {
                    ParameterInfo param = parameters[i];
                    _methodParameters[method][i] = DrawParameterField(param, _methodParameters[method][i]);
                }

                // Draw the button
                if (GUILayout.Button(buttonLabel))
                {
                    try
                    {
                        method.Invoke(targetMonoBehaviour, _methodParameters[method]);
                    }
                    catch (TargetParameterCountException)
                    {
                        Debug.LogError($"Parameter mismatch when invoking method '{method.Name}' on {targetMonoBehaviour.name}.");
                    }
                }
            }
        }

        private object DrawParameterField(ParameterInfo parameter, object currentValue)
        {
            object value = currentValue;
            System.Type paramType = parameter.ParameterType;

            GUILayout.BeginHorizontal();
            GUILayout.Label(parameter.Name);

            if (paramType == typeof(int))
            {
                value = EditorGUILayout.IntField(value != null ? (int)value : 0);
            }
            else if (paramType == typeof(float))
            {
                value = EditorGUILayout.FloatField(value != null ? (float)value : 0f);
            }
            else if (paramType == typeof(string))
            {
                value = EditorGUILayout.TextField(value != null ? (string)value : "");
            }
            else if (paramType == typeof(bool))
            {
                value = EditorGUILayout.Toggle(value != null && (bool)value);
            }
            else if (paramType.IsEnum)
            {
                value = EditorGUILayout.EnumPopup(value != null ? (System.Enum)value : (System.Enum)System.Activator.CreateInstance(paramType));
            }
            else
            {
                EditorGUILayout.LabelField($"Unsupported: {paramType.Name}");
            }

            GUILayout.EndHorizontal();

            return value;
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class InspectorButtonAttribute : PropertyAttribute
    {
        public string ButtonLabel { get; }

        public InspectorButtonAttribute(string buttonLabel = null)
        {
            ButtonLabel = buttonLabel;
        }
    }
}