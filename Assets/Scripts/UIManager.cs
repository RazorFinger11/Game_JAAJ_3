using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {
    public PlayerController playerScript;

    public TextMeshProUGUI healthUI;
    public TextMeshProUGUI fuelUI;
    public TextMeshProUGUI timerUI;

    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject defeatPanel;

    float minutes;
    float seconds;
    string niceTime;

    bool gameFinished;

    private void OnGUI()
    {
        minutes = Mathf.Floor(Match.currentMatchTime / 60);
        seconds = Mathf.RoundToInt(Match.currentMatchTime % 60);
        niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    void Update() {
        if (!gameFinished) {
            if (Match.gameFinished) {
                if (playerScript.CurHealth <= 0) { 
                    victoryPanel.SetActive(true);
                }
                else {
                    defeatPanel.SetActive(true);
                }

                gameFinished = true;
            }

            healthUI.text = playerScript.CurHealth + "/" + playerScript.maxHealth;
            fuelUI.text = playerScript.Fuel.ToString();
            timerUI.text = niceTime;
        }
    }
}
