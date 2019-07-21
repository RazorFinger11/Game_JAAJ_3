using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Sign : MonoBehaviour {
    [SerializeField] string signText;
    [SerializeField] AudioSource signsSource;
    [SerializeField] AudioClip signNarration;

    [SerializeField] PlayerController target;

    bool isActive;

    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "playerAttack" && Input.GetButtonDown("Fire1")) {
            target.LockPlayer(false);
            signsSource.Stop();

            isActive = !isActive;
            UIManager.instance.UpdateSign(isActive, signText);

            if (isActive) {
                target.LockPlayer(true);
                AudioManager.instance.PlayClipWithSource(signsSource, signNarration);
            }
        }
    }
}