    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{

    public enum TrapType { Cooldown, Fueled, OneUse, DamageOverTime }

    // General
    public TrapType type;
    public int trapDamage;
    public AudioClip trapSound;
    Animator anim;
    GameObject player;

    // Cooldowns
    public float trapCooldown;
    public bool slowdown;
    public float slowdownAmount;
    public float slowdownTime;

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
    public ParticleSystem fireParticles;

    //One Use
    public ParticleSystem explosionParticle;

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

                var main = fireParticles.main;
                main.startSpeed = -DamagePerFuel * CurFuel;
                main.startSize = (DamagePerFuel * CurFuel)/15; 

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
                fireParticles.gameObject.SetActive(true);
                fireParticles.Play();
                Activate();
            }
        }

        // If your hands enter a One Use trap
        if (other.gameObject.tag == "playerAttack" && type == TrapType.OneUse)
        {
            Debug.Log("does this count");
            if (Input.GetButtonDown("Fire1"))
            {
                explosionParticle.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 3.44f, this.transform.position.z);
                explosionParticle.transform.parent = null;
                explosionParticle.transform.rotation = new Quaternion(0, 0, 0, 0);
                explosionParticle.gameObject.SetActive(true);
                explosionParticle.Play();
                Activate();
                Destroy(explosionParticle.gameObject, 4f);
            }
        }
    }

    void Activate()
    {
        anim.SetTrigger("Activate");

        if(trapSound != null)
        AudioManager.instance.PlayClipAtPoint(trapSound, this.gameObject);

        player.GetComponent<PlayerController>().SendMessage("Damage", trapDamage);

        if(type == TrapType.Cooldown)
        {
            readyToActivate = false;
            StartCoroutine(CooldownSequence());
            if(slowdown)
            {
                player.GetComponent<PlayerController>().moveSpeed -= slowdownAmount;
                StartCoroutine(SlowdownCountdown());
            }
        }
        if(type == TrapType.Fueled)
        {
            curFuel = 0;
            trapDamage = 0;
        }
        if(type == TrapType.OneUse)
        {
            this.gameObject.SetActive(false);
            this.gameObject.GetComponent<Renderer>().enabled = false;
            this.gameObject.GetComponent<Collider>().enabled = false;
            Destroy(this.gameObject, 4.1f);
        }
    }

    IEnumerator SlowdownCountdown()
    {
        yield return new WaitForSeconds(slowdownTime);
        player.GetComponent<PlayerController>().moveSpeed = player.GetComponent<PlayerController>().DefaultMSpeed;
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
