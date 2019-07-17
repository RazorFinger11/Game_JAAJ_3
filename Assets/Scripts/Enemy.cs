using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] GameObject target;

    //this game object is just empty, its children are the actual points of teleport
    [SerializeField] GameObject spawnPoints;
    Transform[] points;
    //mark all objects that can block the spawn (currently including other enemies)
    [SerializeField] LayerMask checkSpawnMask;

    //time the enemy can be active
    [Range(0, 10)] [SerializeField] float minAttackTime = 3f;
    [Range(0, 10)] [SerializeField] float maxAttackTime = 10f;

    [SerializeField] Bullet bulletPrefab;
    float timeToFire;
    [SerializeField] float fireRate = 1;

    bool attacking;

    void Start() {
        var hashPoints = new HashSet<Transform>(spawnPoints.GetComponentsInChildren<Transform>());
        hashPoints.Remove(spawnPoints.transform);
        points = hashPoints.ToArray();
    }

    void Update() {
        //teleport enemy based on random value between min and max active time
        if (!attacking) {
            Transform[] viablePoints = CheckViablePoints();

            if (viablePoints.Length > 0) {
                this.transform.position = viablePoints[Random.Range(0, viablePoints.Length)].position;
                attacking = true;
                StartCoroutine(WaitForAttack(Random.Range(minAttackTime, maxAttackTime)));
            }
        }
        else {
            //look at player and shoot life at certain firerate
            this.transform.LookAt(target.transform);

            if (Time.time > timeToFire) {
                timeToFire = Time.time + 1 / fireRate;

                Bullet bullet = Instantiate(bulletPrefab, this.transform.position, this.transform.rotation);
            }
        }
    }

    Transform[] CheckViablePoints() {
        var viablePoints = new HashSet<Transform>();

        foreach (Transform point in points) {
            if (!Physics.CheckSphere(point.position, .5f, checkSpawnMask)) {
                viablePoints.Add(point);
            }
        }

        return viablePoints.ToArray();
    }

    IEnumerator WaitForAttack(float time) {
        yield return new WaitForSeconds(time);
        attacking = false;
    }

    //void OnDrawGizmos() {
    //    foreach (Transform point in points) {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireSphere(point.position, .5f);
    //    }
    //}
}