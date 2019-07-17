using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour {
    #region Attributes
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
    [SerializeField] GameObject firePoint;
    float timeToFire;
    [Range(0, 3)] [SerializeField] float minFireRate = .5f;
    [Range(0, 3)] [SerializeField] float maxFireRate = 1;

    Animator anim;

    bool attacking;
    #endregion

    void Start() {
        var hashPoints = new HashSet<Transform>(spawnPoints.GetComponentsInChildren<Transform>());
        hashPoints.Remove(spawnPoints.transform);
        points = hashPoints.ToArray();

        anim = GetComponent<Animator>();
    }

    void Update() {        
        if (!attacking) {
            //check if point is spawnable
            Transform[] viablePoints = CheckViablePoints();

            //teleport after attack time(random value between min and max attack time)
            if (viablePoints.Length > 0) {
                this.transform.position = viablePoints[Random.Range(0, viablePoints.Length)].position;
                attacking = true;
                StartCoroutine(WaitForAttack(Random.Range(minAttackTime, maxAttackTime)));
            }
        }
        else {
            //look at player
            this.transform.LookAt(target.transform);
            firePoint.transform.LookAt(target.transform);

            //begin shoot anim (that will trigger shoot event)
            if (Time.time > timeToFire) {
                anim.SetTrigger("Shoot");
                timeToFire = (Time.time + anim.GetCurrentAnimatorStateInfo(0).length) + 1 / Random.Range(minFireRate, maxFireRate);                
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

    public void Shoot() {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
    }

    IEnumerator WaitForAttack(float time) {
        yield return new WaitForSeconds(time);
        attacking = false;
    }
}