using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem {
    public static void SaveLeaderboard(List<LeaderboardEntry> entries) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "zombie.lo");
        FileStream stream = new FileStream(path, FileMode.Create);

        LeaderboardData data = new LeaderboardData(entries);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static List<LeaderboardEntry> LoadLeaderboard() {
        string path = Path.Combine(Application.persistentDataPath, "zombie.lo");

        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LeaderboardData data = formatter.Deserialize(stream) as LeaderboardData;
            stream.Close();

            List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
            for (int i = 0; i < data.names.Length; i++) {
                entries.Add(new LeaderboardEntry(data.names[i], data.scores[i]));
            }

            return entries;
        }
        else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}