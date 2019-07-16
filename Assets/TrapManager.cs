    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{

    public enum TrapType { Cooldown, Fueled, OneUse, DamageOverTime }

    // General
    public TrapType type;
    public int trapDamage;
    Animator anim;

    // Cooldowns
    public float trapCooldown;
    bool readyToActivate = true;

    //DoT
    public int interval = 10;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // If you enter a cooldown type trap, activate it 
        if(other.gameObject.tag == "Player" && type == TrapType.Cooldown)
        {
            if(readyToActivate)
            Activate(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && type == TrapType.DamageOverTime)
        {
            if(Time.frameCount % interval == 0)
            other.GetComponent<PlayerController>().SendMessage("Damage", trapDamage);
        }
    }

    void Activate(GameObject player)
    {
        anim.SetTrigger("Activate");
        player.GetComponent<PlayerController>().SendMessage("Damage", trapDamage);

        if(type == TrapType.Cooldown)
        {
            readyToActivate = false;
            StartCoroutine(CooldownSequence());
        }
    }

    // wait for Cooldown
    IEnumerator CooldownSequence()
    {
        yield return new WaitForSeconds(trapCooldown);
        readyToActivate = true;
    }
}
