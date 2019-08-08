using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour {
    [SerializeField] Language[] languages;

    [Space]

    [SerializeField] Image flagElement;
    [SerializeField] TextMeshProUGUI languageElement;

    Language currentLanguage;
    int currentIndex;

    void Start() {
        UpdateCanvas();
    }

    void UpdateCanvas() {
        currentLanguage = languages[currentIndex];
        flagElement.sprite = currentLanguage.flagSprite;
        languageElement.text = currentLanguage.languageName;
    }

    void AddToIndex(int index) {
        currentIndex += index;

        if (currentIndex > languages.Length - 1) {
            currentIndex = 0;
        }
        else if (currentIndex < 0) {
            currentIndex = languages.Length - 1;
        }
    }

    public void ChangeLanguage(int index) {
        AddToIndex(index);
        UpdateCanvas();
    }

    public void ConfirmLanguage(string sceneName) {
        LocalizationManager.instance.LoadLocalizedText(currentLanguage.fileName);
        SceneManager.LoadScene(sceneName);
    }
}

[System.Serializable]
public class Language {
    public string fileName;
    public string languageName;
    public Sprite flagSprite;
}