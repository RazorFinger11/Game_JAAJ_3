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
    GameObject player;

    // Cooldowns
    public float trapCooldown;

    float curTime;
    public float CurTime { get => curTime; set => curTime = value; }

    bool readyToActivate = true;

    //DoT
    public int interval = 10;

    //Fueled
    public int DamagePerFuel;
    public int maxFuel;
    int curFuel;
    public int CurFuel { get => curFuel; set => curFuel = value; }

    // Start is called before the first frame update
    void Start()
    {
        curTime = trapCooldown;
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if(type == TrapType.Fueled)
        trapDamage = DamagePerFuel * curFuel;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If you enter a cooldown type trap, activate it 
        if(other.gameObject.tag == "playerAttack" && type == TrapType.Cooldown)
        {
            if(readyToActivate)
            Activate();
        }
        // If you enter a Fuel Trap
        else if(other.gameObject.tag == "playerAttack" && type == TrapType.Fueled)
        {
            // Add the fuel in the machine
            if (curFuel <= maxFuel)
            {
                // if you have more than the capacity, only add the capacity
                if (player.GetComponent<PlayerController>().Fuel > (maxFuel - curFuel))
                {
                    player.GetComponent<PlayerController>().Fuel -= (maxFuel - curFuel);
                    curFuel += (maxFuel - curFuel);
                }
                else
                {
                    // If you dont, add everything
                    curFuel += player.GetComponent<PlayerController>().Fuel;
                    player.GetComponent<PlayerController>().Fuel = 0;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && type == TrapType.DamageOverTime)
        {
            if(Time.frameCount % interval == 0)
            player.GetComponent<PlayerController>().SendMessage("Damage", trapDamage);
        }

        // If he presses the attack
        if (other.gameObject.tag == "playerAttack" && type == TrapType.Fueled)
        {
            // If he presses the button inside the playing area
            if (Input.GetButtonDown("Fire1"))
            {
                Activate();
            }
        }

        // If your hands enter a One Use trap
        if (other.gameObject.tag == "playerAttack" && type == TrapType.OneUse)
        {
            Debug.Log("does this count");
            if (Input.GetButtonDown("Fire1"))
            {
                Activate();
            }
        }
    }

    void Activate()
    {
        anim.SetTrigger("Activate");
        player.GetComponent<PlayerController>().SendMessage("Damage", trapDamage);

        if(type == TrapType.Cooldown)
        {
            readyToActivate = false;
            StartCoroutine(CooldownSequence());
        }
        if(type == TrapType.Fueled)
        {
            curFuel = 0;
            trapDamage = 0;
        }
        if(type == TrapType.OneUse)
        {
            Destroy(this.gameObject);
        }
    }

    // wait for Cooldown
    IEnumerator CooldownSequence()
    {
        while (curTime > 0)
        {
            curTime -= 1;
            yield return new WaitForSeconds(1f);
        }
        readyToActivate = true;
        curTime = trapCooldown;
    }
}
