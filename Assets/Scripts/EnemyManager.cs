using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    Enemy[] enemies;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
    }

    public void ActivateEnemies() {
        if (enemies == null) {
            enemies = FindObjectsOfType<Enemy>();
        }

        foreach (Enemy enemy in enemies) {
            enemy.Alive = true;
        }
    }

    public void DeactivateEnemies() {
        if (enemies == null) {
            enemies = FindObjectsOfType<Enemy>();
        }

        foreach (Enemy enemy in enemies) {
            enemy.Alive = false;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            ActivateEnemies();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            DeactivateEnemies();
        }
    }
}