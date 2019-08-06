using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizeTMPro : MonoBehaviour {
    [SerializeField] string key;

    void OnEnable() {
        LocalizationManager.UpdateText += Localize;
    }

    void OnDisable() {
        LocalizationManager.UpdateText -= Localize;
    }

    void Localize(bool value) {
        TextMeshProUGUI element = GetComponent<TextMeshProUGUI>();
        element.text = LocalizationManager.instance.GetLocalizedValue(key);
    }
}