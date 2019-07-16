using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class healthText : MonoBehaviour
{
    public TextMeshProUGUI healthUI;
    public TextMeshProUGUI fuelUI;
    public PlayerController playerScript;

    void Update()
    {

        healthUI.text = playerScript.CurHealth + "/" + playerScript.maxHealth;
        fuelUI.text = playerScript.Fuel.ToString();
    }
}
