using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Intro : MonoBehaviour {
    [SerializeField] VideoPlayer player;
    [SerializeField] SubbedClip[] clips;
    [SerializeField] LoadManager loadManager;

    float time;

    void Start() {
        foreach (SubbedClip clip in clips) {
            if (clip.fileName == LocalizationManager.instance.FileName) {
                player.clip = clip.clip;
            }
        }

        time = Time.time + 3f;
    }

    void Update() {
        if (Input.anyKeyDown || (Time.time > time && !player.isPlaying)) {
            loadManager.LoadScene("Menu");
        }
    }
}

[System.Serializable]
public class SubbedClip {
    public string fileName;
    public VideoClip clip;
}