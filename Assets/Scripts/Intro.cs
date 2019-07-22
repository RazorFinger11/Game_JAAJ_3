using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Intro : MonoBehaviour {
    [SerializeField] VideoPlayer player;
    [SerializeField] LoadManager loadManager;

    float time;

    void Start() {
        time = Time.time + 3f;
    }

    void Update() {
        if (Input.anyKeyDown || (Time.time > time && !player.isPlaying)) {
            loadManager.LoadScene("Menu");
        }
    }
}
