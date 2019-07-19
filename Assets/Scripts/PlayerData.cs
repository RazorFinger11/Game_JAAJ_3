using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public int highScore;

    public PlayerData(int _highScore) {
        highScore = _highScore;
    }
}
