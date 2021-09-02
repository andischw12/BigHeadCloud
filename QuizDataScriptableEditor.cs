using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(QuizDataScriptable))]
public class QuizDataScriptableEditor : Editor
{
    SerializedObject targetObject;

    void OnEnable()
    {
        if (targetObject == null)
            targetObject = serializedObject;
    }

    public override void OnInspectorGUI()
    {
        targetObject.Update();

        for (int i = 0; i < targetObject.FindProperty("groups").arraySize; i++)
        {
            EditorGUILayout.LabelField("Group " + i);

            EditorGUILayout.BeginHorizontal();
            EditorGUI.indentLevel+=2;

            EditorGUILayout.PropertyField(targetObject.FindProperty("groups").GetArrayElementAtIndex(i).FindPropertyRelative("questions"));

            if (GUILayout.Button("Import", GUILayout.MaxWidth(60)))
            {
                string path = EditorUtility.OpenFilePanel("Import", Application.dataPath, "txt");

                if (path != null && path.Length > 0)
                {
                    targetObject.FindProperty("groups").GetArrayElementAtIndex(i).FindPropertyRelative("questions").arraySize = 0;

                    string[] lines = File.ReadAllLines(path);

                    for (int j = 0, k = 0; j < lines.Length; j+=6, k++)
                    {
                        targetObject.FindProperty("groups").GetArrayElementAtIndex(i).FindPropertyRelative("questions").arraySize++;
                        targetObject.FindProperty("groups").GetArrayElementAtIndex(i).FindPropertyRelative("questions").GetArrayElementAtIndex(k).FindPropertyRelative("questionInfo").stringValue = lines[j];
                        targetObject.FindProperty("groups").GetArrayElementAtIndex(i).FindPropertyRelative("questions").GetArrayElementAtIndex(k).FindPropertyRelative("questionType").enumValueIndex = 0;
                        targetObject.FindProperty("groups").GetArrayElementAtIndex(i).FindPropertyRelative("questions").GetArrayElementAtIndex(k).FindPropertyRelative("options").arraySize = 4;
                        targetObject.FindProperty("groups").GetArrayElementAtIndex(i).FindPropertyRelative("questions").GetArrayElementAtIndex(k).FindPropertyRelative("options").GetArrayElementAtIndex(0).stringValue = lines[j + 1];
                        targetObject.FindProperty("groups").GetArrayElementAtIndex(i).FindPropertyRelative("questions").GetArrayElementAtIndex(k).FindPropertyRelative("options").GetArrayElementAtIndex(1).stringValue = lines[j + 2];
                        targetObject.FindProperty("groups").GetArrayElementAtIndex(i).FindPropertyRelative("questions").GetArrayElementAtIndex(k).FindPropertyRelative("options").GetArrayElementAtIndex(2).stringValue = lines[j + 3];
                        targetObject.FindProperty("groups").GetArrayElementAtIndex(i).FindPropertyRelative("questions").GetArrayElementAtIndex(k).FindPropertyRelative("options").GetArrayElementAtIndex(3).stringValue = lines[j + 4];
                        targetObject.FindProperty("groups").GetArrayElementAtIndex(i).FindPropertyRelative("questions").GetArrayElementAtIndex(k).FindPropertyRelative("correctAns").stringValue = lines[j + 5];
                    }
                }
            }

            if (GUILayout.Button("Remove", GUILayout.MaxWidth(60)))
            {
                targetObject.FindProperty("groups").DeleteArrayElementAtIndex(i);
            }

            EditorGUI.indentLevel-=2;
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Question Group"))
        {
            targetObject.FindProperty("groups").arraySize++;
        }

        targetObject.ApplyModifiedProperties();
    }
}
