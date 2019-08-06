using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LocalizedTextEditor : EditorWindow {
    public LocalizationData localizationData;

    Vector2 scrollPos = Vector2.zero;

    [MenuItem("Window/Localized Text Editor")]
    static void Init() {
        EditorWindow.GetWindow(typeof(LocalizedTextEditor)).Show();
    }

    void OnGUI() {
        //begin scrolling
        scrollPos = GUILayout.BeginScrollView(scrollPos, true, true, GUILayout.Width(position.width), GUILayout.Height(position.height));

        //code
        if (localizationData != null) {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("localizationData");

            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save File!")) {
                SaveData();
            }
        }

        if (GUILayout.Button("Load File!")) {
            LoadData();
        }

        if (GUILayout.Button("Create New File!")) {
            CreateNewData();
        }

        //end scrolling
        GUILayout.EndScrollView();
    }

    void CreateNewData() {
        localizationData = new LocalizationData();
    }

    void SaveData() {
        string filePath = EditorUtility.SaveFilePanel("Save localization data file", Application.streamingAssetsPath, "language", "json");

        if (!string.IsNullOrEmpty(filePath)) {
            string dataAsJson = JsonUtility.ToJson(localizationData);
            File.WriteAllText(filePath, dataAsJson);
        }
    }

    void LoadData() {
        string filePath = EditorUtility.OpenFilePanel("Load localization data file", Application.streamingAssetsPath, "json");

        if (!string.IsNullOrEmpty(filePath)) {
            string dataAsJson = File.ReadAllText(filePath);
            localizationData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        }
    }
}