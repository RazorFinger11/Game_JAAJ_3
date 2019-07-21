using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Sign : MonoBehaviour {    
    [SerializeField] string signTitle, signText;
    [SerializeField] AudioSource signsSource;
    [SerializeField] AudioClip signNarration;

    [SerializeField] PlayerController target;

    bool isActive;

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            Ray ray = new Ray(this.transform.position, target.transform.position - this.transform.position);

            if(Physics.Raycast(ray, 2f)) {
                signsSource.Stop();

                isActive = !isActive;
                UIManager.instance.UpdateSign(isActive, signTitle, signText);
                target.LockPlayer(isActive);

                if (isActive) {
                    AudioManager.instance.PlayClipWithSource(signsSource, signNarration);
                }
            }
        }    
    }
}