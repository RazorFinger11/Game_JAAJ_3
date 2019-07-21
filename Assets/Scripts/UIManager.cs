using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    public PlayerController playerScript;

    [Space]

    public TextMeshProUGUI healthUI;
    public Image healthBar, healthBorder;
    public Sprite highHealth, lowHealth;
    public Image crossHealth;
    public Sprite highHealthCross, lowHealthCross, deadCross;

    [Space]
    public Image[] fuelBars; //from lowest to highest
    public TextMeshProUGUI fuelUI;

    public TextMeshProUGUI timerUI;

    [SerializeField] GameObject victoryPanel;
    [SerializeField] GameObject defeatPanel;

    [Space]
    [SerializeField] GameObject signPanel;
    [SerializeField] TextMeshProUGUI signUI;

    float minutes;
    float seconds;
    string niceTime;

    bool gameFinished;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
    }

    private void OnGUI() {
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
            timerUI.text = niceTime;
        }
    }

    public void UpdateHealth(float health) {
        float borderAmount = health > 0 ? 0.01f : 0f;

        healthBar.fillAmount = health / 100;
        healthBorder.fillAmount = (health / 100) + borderAmount;
        healthUI.text = playerScript.CurHealth.ToString();

        if (health > 40) {
            healthBar.sprite = highHealth;
            crossHealth.sprite = highHealthCross;
        }
        else if (health > 0) {
            healthBar.sprite = lowHealth;
            crossHealth.sprite = lowHealthCross;
        }
        else {
            crossHealth.sprite = deadCross;
            healthUI.text = "Morto";
        }
    }

    public void UpdateFuel(int fuel) {
        //turn off all fuels
        for (int i = 0; i < fuelBars.Length; i++) {
            fuelBars[i].gameObject.SetActive(false);
        }

        //turn on correspondent amount
        for (int i = 0; i < fuel; i++) {
            fuelBars[i].gameObject.SetActive(true);
        }
    }

    public void UpdateSign(bool state, string signText) {
        signPanel.SetActive(state);
        signUI.text = signText;
    }
}