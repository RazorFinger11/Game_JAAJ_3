using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapType { Cooldown, Fueled, OneUse }

public class TrapManager : MonoBehaviour {
    // General
    public TrapType type;
    public int trapDamage;
    Animator anim;

    // Cooldowns
    public float trapCooldown;
    bool readyToActivate = true;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other) {
        // If you enter a cooldown type trap, activate it 
        if (other.gameObject.tag == "Player" && type == TrapType.Cooldown) {
            if (readyToActivate)
                Activate(other.gameObject);
        }
    }

    void Activate(GameObject player) {
        anim.SetTrigger("Activate");
        player.GetComponent<PlayerController>().Damage(trapDamage);

        if (type == TrapType.Cooldown) {
            readyToActivate = false;
            StartCoroutine(CooldownSequence());
        }
    }

    // wait for Cooldown
    IEnumerator CooldownSequence() {
        yield return new WaitForSeconds(trapCooldown);
        readyToActivate = true;
    }
}