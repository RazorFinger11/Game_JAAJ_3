using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour {
    [SerializeField] string signTitle, signText;
    [SerializeField] PlayerController target;

    bool isActive;

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            Ray ray = new Ray(this.transform.position, target.transform.position - this.transform.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2f)) {
                if (hit.collider.gameObject.tag == target.tag) {
                    isActive = !isActive;
                    UIManager.instance.UpdateSign(isActive, signTitle, signText);
                    target.LockPlayer(isActive);
                }
            }
        }
    }
}