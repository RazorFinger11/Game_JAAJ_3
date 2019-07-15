using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    int damage = 10;
    int speed = 30;

    void Update() {
        transform.position += this.transform.TransformDirection(Vector3.forward) * Time.deltaTime * speed;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            other.GetComponent<PlayerController>().Damage(-damage);
        }
    }
}
