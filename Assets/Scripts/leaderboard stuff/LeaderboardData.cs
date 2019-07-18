using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardData {
    public string[] names;
    public int[] scores;

    public LeaderboardData(List<LeaderboardEntry> entries) {
        int length = entries.Count;
        names = new string[length];
        scores = new int[length];

        for (int i = 0; i < length; i++) {
            names[i] = entries[i].name;
            scores[i] = entries[i].score;
        }
    }
}