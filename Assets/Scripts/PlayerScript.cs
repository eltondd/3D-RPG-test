using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour {

    // initialize public variables
    public bool movable = true;

    // player's movement and rotation speed
    public float movementSpeed = 1.0f;
    public float rotationSpeed = 1.0f;

    // player hp
    public int maxHP = 20;
    public int hp;

    // pointer to events gameobject
    public GameObject eventSystem;

    // pointer to the hp gauage in the main canvas
    public RectTransform hpGauge;

    // bool for whether player is blocking or not 
    public bool blocking = false;

    private bool canAttack;

    // reference to rigid body
    private Rigidbody rigidBody;

    // reference to animator
    private Animator animator;

    private float invulernableTimer;

    // When the object is instantiated
    private void Awake() {
        // get rigid body component
        rigidBody = GetComponent<Rigidbody>();
        // lock the constraints of X rotaion, Y rotation, Z rotation and Y position movement (prevents tipping over)
        rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionY;
        // reference to the animator
        animator = transform.GetChild(1).GetComponent<Animator>();
        // set hp to the max HP so HP will be full
        hp = maxHP;
        canAttack = true;
    }

    private void Update() {
        invulernableTimer += Time.deltaTime;
    }

    // Update is called once per frame
    private void FixedUpdate () {
        if (movable) {
            Move();
            Rotate();
            Attack();
            Block();
            Run();
        }

        lowerHP();
    }

    private void OnTriggerEnter(Collider other) {
        if (blocking && other.gameObject.tag == "Fireball")
            Destroy(other.gameObject);

        if (other.gameObject.tag == "Fireball" && invulernableTimer > 2) {
            invulernableTimer = 0;
            if (blocking) {
                hp--;
            } else {
                hp -= 2;
            }

            if (hp <= 0)
                eventSystem.GetComponent<GlobalEvents>().openGameOverMenu();
        }
    }

    private void Move() {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            input += transform.forward;
        if (Input.GetKey(KeyCode.S))
            input -= transform.forward;
        if (Input.GetKey(KeyCode.E))
            input += transform.right;
        if (Input.GetKey(KeyCode.Q))
            input -= transform.right;

        input.Normalize();

        Vector3 movement = input * movementSpeed * Time.deltaTime;
        rigidBody.MovePosition(rigidBody.position + movement);
    }

    private void Rotate() {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
            input += Vector3.down;
        if (Input.GetKey(KeyCode.D))
            input += Vector3.up;

        input.Normalize();

        Quaternion deltaRotation = Quaternion.Euler(input * rotationSpeed * Time.deltaTime);
        rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
    }

    private void preventRotation() {
        rigidBody.rotation = Quaternion.Euler(0, rigidBody.rotation.y, 0);
    }

    private void Attack() {
        if (Input.GetMouseButtonDown(0) && canAttack) 
            animator.Play("Player_Attack");   
    }

    private void Block() { 
        if (Input.GetMouseButtonDown(1)) {
            blocking = true;
            canAttack = false;
            animator.Play("Player_BlockEnter");
        }
        if (Input.GetMouseButtonUp(1)) {
            blocking = false;
            canAttack = true;
            animator.Play("Player_BlockExit");
        }
    }

    private void Run() {
        if (Input.GetKey(KeyCode.LeftShift))
            movementSpeed = 30.0f;
        else
            movementSpeed = 15.0f;
    }

    private void lowerHP() {
        float hpPosition = (float)hp / maxHP;
        hpGauge.anchorMax = new Vector2(hpPosition, 1f);
    }
}