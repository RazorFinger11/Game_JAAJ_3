using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class healthText : MonoBehaviour
{
    TextMeshProUGUI healthUI;
    public PlayerController playerScript;

    private void Start()
    {
        healthUI = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        healthUI.text = playerScript.CurHealth + "/" + playerScript.maxHealth;
    }
}
