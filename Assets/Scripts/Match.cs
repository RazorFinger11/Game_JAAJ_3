using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour {
    public static Match instance;

    [SerializeField] int matchTime;
    int currentMatchTime;
    public int CurrentMatchTime { get => currentMatchTime; }
    float time;

    bool gameFinished;
    public bool GameFinished { get { return gameFinished; } }

    [SerializeField] PlayerController playerScript;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
    }

    void Start() {        
        currentMatchTime = matchTime;

        if (matchTime > 0) {
            EnemyManager.instance.ActivateEnemies();
        }
    }

    void Update() {
        //arena
        if (matchTime > 0) {
            if (!gameFinished && Time.time > time) {
                time = Time.time + 1f;
                currentMatchTime--;

                if (currentMatchTime == 0) {
                    FinishGame();
                }
            }
        }
    }

    public void FinishGame() {        
        gameFinished = true;
        UIManager.instance.FinishGame(playerScript.CurHealth);
        EnemyManager.instance.DeactivateEnemies();
    }
}