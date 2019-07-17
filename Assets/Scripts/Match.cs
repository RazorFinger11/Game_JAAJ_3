using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour {
    [SerializeField] int matchTime;
    public static int currentMatchTime;
    public static bool gameFinished;
    public PlayerController playerScript; 

    float time;    

    void Start() {
        currentMatchTime = matchTime;
        gameFinished = false;
    }

    void Update() {
        if (!gameFinished && Time.time > time) {
            time = Time.time + 1f;
            currentMatchTime--;

            if (currentMatchTime == 0 || playerScript.CurHealth <= 0) {
                gameFinished = true;
            }
        }
    }
}