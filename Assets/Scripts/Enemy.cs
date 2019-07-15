using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] GameObject spawnPoints;    
    Transform[] points;

    GameObject model;
    bool teleported;

    void Start() {
        var hashPoints = new HashSet<Transform>(spawnPoints.GetComponentsInChildren<Transform>());
        hashPoints.Remove(spawnPoints.transform);
        points = hashPoints.ToArray();
    }

    void Update() {
        if (!teleported) {
            this.transform.position = points[Random.Range(0, points.Length)].position;
            StartCoroutine("WaitToTeleport");
        }
    }

    IEnumerator WaitToTeleport() {
        teleported = true;
        yield return new WaitForSeconds(1);        
        teleported = false;
    }
}