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

    [SerializeField] AudioClip teleportSound;
    [SerializeField] AudioClip bulletSound;

    Animator anim;
    float attackDuration;
    #endregion

    void Start() {
        var hashPoints = new HashSet<Transform>(spawnPoints.GetComponentsInChildren<Transform>());
        hashPoints.Remove(spawnPoints.transform);
        points = hashPoints.ToArray();

        anim = GetComponent<Animator>();
    }

    void Update() {
        //look at player
        this.transform.LookAt(target.transform);
        firePoint.transform.LookAt(target.transform);

        if (Time.time > attackDuration) {
            //begin teleport anim (that will trigger teleport event)
            anim.SetTrigger("Teleport");
            attackDuration = (Time.time + anim.GetCurrentAnimatorStateInfo(0).length) + Random.Range(minAttackTime, maxAttackTime);
        }
        else {
            //begin shoot anim (that will trigger shoot event)
            if (Time.time > timeToFire) {
                anim.SetTrigger("Shoot");
                timeToFire = (Time.time + anim.GetCurrentAnimatorStateInfo(0).length) + 1 / Random.Range(minFireRate, maxFireRate);                
            }
        }
    }

    public void Teleport() {
        //check if point is spawnable
        Transform[] viablePoints = CheckViablePoints();

        //teleport after attack time(random value between min and max attack time)
        if (viablePoints.Length > 0) {
            this.transform.position = viablePoints[Random.Range(0, viablePoints.Length)].position;
            AudioManager.instance.PlayClipAtPoint(teleportSound, this.gameObject);
        }
    }

    public void Shoot() {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        AudioManager.instance.PlayClipAtPoint(bulletSound, this.gameObject);
    }

    Transform[] CheckViablePoints() {
        var viablePoints = new HashSet<Transform>();

        foreach (Transform point in points) {
            if (!Physics.CheckSphere(point.position, .5f, checkSpawnMask) && point.position != transform.position) {
                viablePoints.Add(point);
            }
        }

        return viablePoints.ToArray();
    }
}