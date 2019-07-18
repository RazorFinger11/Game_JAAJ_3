using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Health
    public float maxHealth;
    float curHealth;
    public float CurHealth { get => curHealth; set => curHealth = value; }

    // Fuel
    int fuel;
    public int Fuel { get => fuel; set => fuel = value; }
    public int maxFuel = 3;
    public FuelSpawner fuelSpawnerScript;

    void Start() {
        rb = GetComponent<Rigidbody>();
        curHealth = maxHealth;
        defaultMoveSpeed = moveSpeed;
    }

    void Update() {
        horizontalInput = GetHorizontalInput();
        verticalInput = GetVerticalInput();
        Jump();
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("ATTACK");
        }
    }

        public void Damage(int value) {
        curHealth -= value;
        
        if (curHealth > maxHealth) {
            curHealth = maxHealth;
        }

        if (curHealth < 0) {
            curHealth = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Fuel" && fuel < maxFuel)
        {
            fuelSpawnerScript.transformVectors.Add(this.transform.position);
            fuelSpawnerScript.CurFuels--;
            fuelSpawnerScript.CurTime = 0;
            Destroy(other.gameObject);
            fuel++;
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

    void Jump() {
        if (isGrounded()) {
            if (verticalVelocity <= 0)
                verticalVelocity = 0;

            if (Input.GetKeyDown(KeyCode.Space)) {
                verticalVelocity = jumpHeight;
            }
        }
        else {
            verticalVelocity -= gravity * Time.deltaTime;
        }
    }


    #region Getting Player Input
    float GetHorizontalInput() {
        float r = 0;
        r += Input.GetAxis("Horizontal");
        return Mathf.Clamp(r, -1.0f, 1.0f);
    }

    float GetVerticalInput() {
        float r = 0;
        r += Input.GetAxis("Vertical");
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