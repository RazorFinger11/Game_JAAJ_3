using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardEntry {
    public string name;
    public int score;

    public LeaderboardEntry(string _name, int _score) {
        name = _name;
        score = _score;
    }
}