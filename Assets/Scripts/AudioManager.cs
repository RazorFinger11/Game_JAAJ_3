using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {
    public static AudioManager instance; //static instance can be called any time
    [SerializeField] Camera mainCamera;
    AudioSource sourceFX;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
    }

    void Start() {
        sourceFX = GetComponent<AudioSource>();
    }

    //UI sounds
    public void PlayClip(AudioClip clip) {
        sourceFX.PlayOneShot(clip);
    }

    //ambient sounds
    public void PlayClipAtPoint(AudioClip clip, GameObject go) {
        //play as if sound was 10 times closer to the camera
        AudioSource.PlayClipAtPoint(clip, 0.9f * mainCamera.transform.position + 0.1f * go.transform.position);        
    }
}