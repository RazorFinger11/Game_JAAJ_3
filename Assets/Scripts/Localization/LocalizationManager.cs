using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LocalizationManager : MonoBehaviour {
    public static LocalizationManager instance;

    public static event Action<bool> UpdateText = delegate { };

    Dictionary<string, string> localizedText;
    string missingValue = "Localized string not found!";
    bool ready;

    public bool Ready { get { return ready; } }

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        LoadLocalizedText("en.json");
    }

    public void LoadLocalizedText(string fileName) {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath)) {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++) {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            UpdateText(true);
            Debug.Log("Data loaded! Dictionary contains " + localizedText.Count + " entries!");
        }
        else {
            Debug.LogError("Cannot find localization file!!");
        }

        ready = true;
    }

    public string GetLocalizedValue(string key) {
        string result = missingValue;
        localizedText.TryGetValue(key, out result);
        return result;
    }
}