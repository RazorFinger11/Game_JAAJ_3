using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Leaderboard : MonoBehaviour {
    [SerializeField] GameObject entryContainer; //empty gameObject to become parent
    [SerializeField] GameObject entryTemplate; //template for fixed highscores
    [SerializeField] GameObject newEntryTemplate; //template that can have name inputted in (i guess inputted doesn't exist but whatevs)

    int leaderboardSize = 10;

    List<LeaderboardEntry> entries;
    int score;

    void Start() {
        ShowLeaderboard();       
    }

    public void ShowLeaderboard() {
        //reset UI
        foreach (Transform child in entryContainer.transform) {
            Destroy(child.gameObject);
        }
        entryTemplate.SetActive(false);
        newEntryTemplate.SetActive(false);

        //carregar dados anteriores de highscores
        entries = SaveSystem.LoadLeaderboard();

        //pegar último score feito pelo player apenas se ele jogou
        LeaderboardEntry playerEntry = new LeaderboardEntry("", PlayerPrefs.GetInt("Score", -1));
        if (playerEntry.score >= 0) {
            entries.Add(playerEntry);
        }

        //checar se ele entra no top 10 scores
        for (int i = 0; i < entries.Count; i++) {
            for (int j = i + 1; j < entries.Count; j++) {
                if(entries[j].score > entries[i].score) {
                    //swap
                    LeaderboardEntry temp = entries[i];
                    entries[i] = entries[j];
                    entries[j] = temp;
                }
            }
        }
        entries = entries.GetRange(0, leaderboardSize);

        float templateHeight = 25f;
        for (int i = 0; i < entries.Count; i++) {
            GameObject newEntry = null;

            if (entries[i].Equals(playerEntry)) {
                //caso ele entre, preciso pedir para que player coloque nome
                newEntry = Instantiate(newEntryTemplate, entryContainer.transform);
                newEntry.name = "New Entry " + i;
            }
            else {
                //se não, apenas mostrar
                newEntry = Instantiate(entryTemplate, entryContainer.transform);
                newEntry.name = "Entry " + i;
                newEntry.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = entries[i].name;
            }

            //positioning new entry
            RectTransform newEntryTransform = newEntry.GetComponent<RectTransform>();
            newEntryTransform.anchoredPosition = new Vector2(0, -templateHeight * i);

            //change new entry values
            newEntry.transform.Find("Pos").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            newEntry.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = entries[i].score.ToString();

            newEntry.SetActive(true);
        }
    }

    public void SaveNewEntry(TextMeshProUGUI name) {
        if (name.text.Trim().Length > 1) {
            entries.Find(x => x.name == "").name = name.text;
            SaveSystem.SaveLeaderboard(entries);
            PlayerPrefs.DeleteKey("Score");
        }

        ShowLeaderboard();
    }
}