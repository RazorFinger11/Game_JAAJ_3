﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    // Input
    float horizontalInput;
    float verticalInput;

    // Components
    Rigidbody rb;

    // Movement
    public float moveSpeed;

    float defaultMoveSpeed;
    public float DefaultMSpeed { get => defaultMoveSpeed; }

    public float jumpHeight;
    public float gravity;
    float verticalVelocity;
    Vector3 movement;
    public Animator anim;
    AudioSource footstepSource;

    bool locked;
    public bool Locked { get => locked; }

    // Health
    [Space]
    public float maxHealth;
    float curHealth;
    public float CurHealth { get => curHealth; set => curHealth = value; }
    public GameObject healthIndicator;
    public GameObject healthPos;
    public GameObject healthScreenBlur;

    public AudioSource screamSource;
    public AudioClip[] zombieScreams;

    // Fuel
    [Space]
    int fuel;
    public int Fuel { get => fuel; set => fuel = value; }
    public AudioClip fuelPickup;

    public int maxFuel = 3;
    public FuelSpawner fuelSpawnerScript;
    public GameObject fuelPickupIndicator;

    void Start() {
        rb = GetComponent<Rigidbody>();
        footstepSource = GetComponent<AudioSource>();
        curHealth = maxHealth;
        defaultMoveSpeed = moveSpeed;

        //make sure it's not paused
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    void Update() {
        horizontalInput = GetHorizontalInput();
        verticalInput = GetVerticalInput();

        //make footstep sound
        footstepSource.volume = (verticalVelocity == 0) ? Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)) / 2 : 0;

        //animate movement
        anim.SetFloat("Velocity", Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)));

        Jump();

        if (fuelPickupIndicator.GetComponent<TextMeshProUGUI>().color.a <= 0) {
            fuelPickupIndicator.SetActive(false);

            fuelPickupIndicator.GetComponent<TextMeshProUGUI>().color = new Vector4(fuelPickupIndicator.GetComponent<TextMeshProUGUI>().color.r, fuelPickupIndicator.GetComponent<TextMeshProUGUI>().color.g,
                                                                                    fuelPickupIndicator.GetComponent<TextMeshProUGUI>().color.b, 10);

        }

        if (healthScreenBlur.GetComponent<Image>().color.a <= 0) {
            healthScreenBlur.SetActive(false);

            healthScreenBlur.GetComponent<Image>().color = new Vector4(healthScreenBlur.GetComponent<Image>().color.r, healthScreenBlur.GetComponent<Image>().color.g,
                                                                                    healthScreenBlur.GetComponent<Image>().color.b, 10);

        }

        //pause or unpause
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }
    }

    private void FixedUpdate() {
        MovePlayer();

        if (Input.GetButtonDown("Fire1")) {
            anim.SetTrigger("ATTACK");
        }
    }

    public void Pause() {
        bool paused = UIManager.instance.UpdatePause();

        if (paused) {
            locked = true;
        }
        else {
            locked = false;
        }
    }

    public void Damage(int value) {
        if (!Match.instance.GameFinished) {
            if (value > 0) {
                AudioManager.instance.PlayClipWithSource(screamSource, zombieScreams[Random.Range(0, zombieScreams.Length)]);
            }

            var indicator = Instantiate(healthIndicator, healthPos.transform);
            indicator.GetComponent<TextMeshPro>().text = (-value).ToString();

            healthScreenBlur.gameObject.SetActive(true);

            if (value < 0) {
                indicator.GetComponent<TextMeshPro>().color = Color.green;
                healthScreenBlur.GetComponent<Image>().color = Color.green;
            }
            else {
                healthScreenBlur.GetComponent<Image>().color = Color.red;
            }

            Destroy(indicator, 1f);

            curHealth -= value;

            if (curHealth > maxHealth) {
                curHealth = maxHealth;
            }
            else if (curHealth <= 0) {
                curHealth = 0;
                anim.SetTrigger("DEATH");
                LockPlayer(true);
                Match.instance.FinishGame();
            }

            //update ui
            UIManager.instance.UpdateHealth(curHealth);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Fuel" && fuel < maxFuel) {
            fuelPickupIndicator.SetActive(true);

            fuelSpawnerScript.transformVectors.Add(this.transform.position);
            fuelSpawnerScript.CurFuels--;
            fuelSpawnerScript.CurTime = 0;
            Destroy(other.gameObject);
            fuel++;
            AudioManager.instance.PlayClip(fuelPickup);
            UIManager.instance.UpdateFuel(fuel);
        }
    }

    void MovePlayer() {
        // getting movement value
        movement = (transform.forward * verticalInput) + (transform.right * horizontalInput);
        // normalizing movement
        movement = movement.normalized * moveSpeed;
        // getting jumps
        movement.y = verticalVelocity;

        // applying movement
        rb.AddForce(movement, ForceMode.VelocityChange);
        rb.velocity = new Vector3(rb.velocity.x, verticalVelocity, rb.velocity.z);
    }

    public void LockPlayer(bool _locked) {
        locked = _locked;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void Jump() {
        if (isGrounded()) {
            if (verticalVelocity <= 0)
                verticalVelocity = 0;

            if (!locked && Input.GetKeyDown(KeyCode.Space)) {
                verticalVelocity = jumpHeight;
            }

            anim.SetBool("Grounded", true);
        }
        else {
            verticalVelocity -= gravity * Time.deltaTime;
            anim.SetBool("Grounded", false);
        }
    }


    #region Getting Player Input
    float GetHorizontalInput() {
        float r = 0;

        if (!locked) {
            r += Input.GetAxis("Horizontal");
        }

        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    float GetVerticalInput() {
        float r = 0;

        if (!locked) {
            r += Input.GetAxis("Vertical");
        }

        return Mathf.Clamp(r, -1.0f, 1.0f);
    }


    #endregion

    bool isGrounded() {
        Ray rayCenter = new Ray(transform.position, transform.up);
        Ray rayLeft = new Ray(new Vector3(transform.position.x + 0, transform.position.y, transform.position.z), -transform.up);
        Ray rayRight = new Ray(new Vector3(transform.position.x - 0, transform.position.y, transform.position.z), -transform.up);
        Ray rayUp = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0), -transform.up);
        Ray rayDown = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0), -transform.up);

        Ray diagonalUpLeft = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), -transform.up);
        Ray diagonalUpRight = new Ray(new Vector3(transform.position.x - 0, transform.position.y, transform.position.z + 0), -transform.up);
        Ray diagonalDownLeft = new Ray(new Vector3(transform.position.x + 0, transform.position.y, transform.position.z - 0), -transform.up);
        Ray diagonalDownRight = new Ray(new Vector3(transform.position.x - 0, transform.position.y, transform.position.z - 0), -transform.up);

        Ray SmalldiagonalUpLeft = new Ray(new Vector3(transform.position.x + 0, transform.position.y, transform.position.z + 0), -transform.up);
        Ray SmalldiagonalUpRight = new Ray(new Vector3(transform.position.x - 0, transform.position.y, transform.position.z + 0), -transform.up);
        Ray SmalldiagonalDownLeft = new Ray(new Vector3(transform.position.x + 0, transform.position.y, transform.position.z - 0), -transform.up);
        Ray SmalldiagonalDownRight = new Ray(new Vector3(transform.position.x - 0, transform.position.y, transform.position.z - 0), -transform.up);

        RaycastHit hit;

        float rayDistance = 1.05f;

        // ------------- SIDE RAYCASTS ------------------
        Debug.DrawLine(transform.position, transform.position + transform.up * rayDistance, Color.green);
        Debug.DrawLine(new Vector3(transform.position.x + 0, transform.position.y, transform.position.z), new Vector3(transform.position.x + 0, transform.position.y, transform.position.z) - transform.up * rayDistance, Color.green);
        Debug.DrawLine(new Vector3(transform.position.x - 0, transform.position.y, transform.position.z), new Vector3(transform.position.x - 0, transform.position.y, transform.position.z) - transform.up * rayDistance, Color.green);
        // --------------------- UP RAYCASTS ------------------
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0), new Vector3(transform.position.x, transform.position.y, transform.position.z + 0) - transform.up * rayDistance, Color.green);
        Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0), new Vector3(transform.position.x, transform.position.y, transform.position.z - 0) - transform.up * rayDistance, Color.green);
        // --------------- DIAGONAL RAYCASTS --------------------
        Debug.DrawLine(new Vector3(transform.position.x + 0, transform.position.y, transform.position.z + 0), new Vector3(transform.position.x + 0, transform.position.y, transform.position.z + 0) - transform.up * rayDistance, Color.green);
        Debug.DrawLine(new Vector3(transform.position.x - 0, transform.position.y, transform.position.z + 0), new Vector3(transform.position.x - 0, transform.position.y, transform.position.z + 0) - transform.up * rayDistance, Color.green);
        Debug.DrawLine(new Vector3(transform.position.x + 0, transform.position.y, transform.position.z - 0), new Vector3(transform.position.x + 0, transform.position.y, transform.position.z - 0) - transform.up * rayDistance, Color.green);
        Debug.DrawLine(new Vector3(transform.position.x - 0, transform.position.y, transform.position.z - 0), new Vector3(transform.position.x - 0, transform.position.y, transform.position.z - 0) - transform.up * rayDistance, Color.green);
        // ----------------- SMALLER DIAGONAL RAYCASTS ------------
        Debug.DrawLine(new Vector3(transform.position.x + 0, transform.position.y, transform.position.z + 0), new Vector3(transform.position.x + 0, transform.position.y, transform.position.z + 0) - transform.up * rayDistance, Color.green);
        Debug.DrawLine(new Vector3(transform.position.x - 0, transform.position.y, transform.position.z + 0), new Vector3(transform.position.x - 0, transform.position.y, transform.position.z + 0) - transform.up * rayDistance, Color.green);
        Debug.DrawLine(new Vector3(transform.position.x + 0, transform.position.y, transform.position.z - 0), new Vector3(transform.position.x + 0, transform.position.y, transform.position.z - 0) - transform.up * rayDistance, Color.green);
        Debug.DrawLine(new Vector3(transform.position.x - 0, transform.position.y, transform.position.z - 0), new Vector3(transform.position.x - 0, transform.position.y, transform.position.z - 0) - transform.up * rayDistance, Color.green);

        if (Physics.Raycast(rayCenter, out hit, rayDistance) || Physics.Raycast(rayLeft, out hit, rayDistance) || Physics.Raycast(rayRight, out hit, rayDistance) || Physics.Raycast(rayUp, out hit, rayDistance) || Physics.Raycast(rayDown, out hit, rayDistance)
             || Physics.Raycast(diagonalDownLeft, out hit, rayDistance) || Physics.Raycast(diagonalDownRight, out hit, rayDistance) || Physics.Raycast(diagonalUpLeft, out hit, rayDistance) || Physics.Raycast(diagonalUpRight, out hit, rayDistance)
             || Physics.Raycast(SmalldiagonalDownLeft, out hit, rayDistance) || Physics.Raycast(SmalldiagonalDownRight, out hit, rayDistance) || Physics.Raycast(SmalldiagonalUpLeft, out hit, rayDistance) || Physics.Raycast(SmalldiagonalUpRight, out hit, rayDistance)) {
            return true;
        }
        else {
            return false;
        }
    }
}