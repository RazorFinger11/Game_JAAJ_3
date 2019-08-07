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

    //localize text at start
    void Start() {
        Localize(true);
    }

    //if some elements needs to be updated
    void Localize(bool value) {
        TextMeshProUGUI element = GetComponent<TextMeshProUGUI>();
        element.text = LocalizationManager.instance.GetLocalizedValue(key);
    }
}