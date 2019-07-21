﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    public TextMeshProUGUI healthUI;
    public Image healthBar, healthBorder;
    public Sprite highHealth, lowHealth;
    public Image crossHealth;
    public Sprite highHealthCross, lowHealthCross, deadCross;

    [Space]
    public Image[] fuelBars; //from lowest to highest

    [Space]
    public TextMeshProUGUI timerUI;

    [Space]
    [SerializeField] GameObject victoryPanel;
    [SerializeField] TextMeshProUGUI victoryScoreUI;
    [SerializeField] GameObject defeatPanel;

    [Space]
    [SerializeField] GameObject signPanel;
    [SerializeField] TextMeshProUGUI signTitleUI, signUI;

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

    void OnGUI() {
        minutes = Mathf.Floor(Match.instance.CurrentMatchTime / 60);
        seconds = Mathf.RoundToInt(Match.instance.CurrentMatchTime % 60);
        niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        timerUI.text = niceTime;
    }

    public void UpdateHealth(float health) {
        float borderAmount = health > 0 ? 0.01f : 0f;

        healthBar.fillAmount = health / 100;
        healthBorder.fillAmount = (health / 100) + borderAmount;
        healthUI.text = health.ToString();

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

    public void UpdateSign(bool state, string signTitle, string signText) {
        signPanel.SetActive(state);
        signTitleUI.text = signTitle;
        signUI.text = signText;
    }

    public void FinishGame(float health) {
        if (health <= 0) {
            int score = Convert.ToInt16(niceTime.Replace(":", ""));
            PlayerPrefs.SetInt("Score", score);
            victoryPanel.SetActive(true);
            victoryScoreUI.text = "Seu score foi de " + score;
        }
        else {
            defeatPanel.SetActive(true);
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}