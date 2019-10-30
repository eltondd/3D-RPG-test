using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WizardScript : MonoBehaviour {
    public bool canAttack;

    public int maxHP = 9;
    public GameObject fireballPrefab;
    public RectTransform hpGauge;
    // pointer to events gameobject
    public GameObject eventSystem;

    private GameObject player;
    private Rigidbody rigidBody;
    private Transform wand;

    private float timer;
    private float invulnerableTimer;
    private float rateOfFire;
    private int hp;

    // Use this for initialization
    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidBody = GetComponent<Rigidbody>();

        wand = transform.GetChild(0).GetChild(7);
        canAttack = true;

        hp = maxHP;
        invulnerableTimer = 2;
        rateOfFire = 3.0f;

        fireballPrefab.transform.localScale = new Vector3(1, 1, 1);
    }

    private void Update() {
        timer += Time.deltaTime;
        invulnerableTimer += Time.deltaTime;
        if (canAttack && timer > rateOfFire) {
            if (hp > 6)
                shootFireBall();
            if (hp > 2 && hp <= 6)
                shootGiantFireball();
            if (hp <= 2)
                shootMultiFireball();
           
            timer = 0;
        }
        lowerHP();
    }

    private void FixedUpdate() {
        FacePlayer();
        Teleport();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Weapon" && invulnerableTimer > 0.5f) {
            invulnerableTimer = 0;
            if (--hp == 0) {    
                Destroy(this.gameObject);
                eventSystem.GetComponent<GlobalEvents>().EndGameCutscene();
            }
        }
    }

    private void FacePlayer() {
        transform.LookAt(2 * transform.position - player.transform.position);
    }

    private void shootFireBall() {
        Instantiate(fireballPrefab, wand.position, new Quaternion(0, rigidBody.rotation.y, 0, transform.rotation.w));
    }

    private void shootMultiFireball() {
        fireballPrefab.transform.localScale = new Vector3(1, 1, 1);
        Vector3 leftPosition = new Vector3(wand.position.x - 3, wand.position.y, wand.position.z);
        Vector3 rightPosition = new Vector3(wand.position.x + 3, wand.position.y, wand.position.z);
        Instantiate(fireballPrefab, wand.position, new Quaternion(0, rigidBody.rotation.y, 0, transform.rotation.w));
        Instantiate(fireballPrefab, leftPosition, new Quaternion(0, rigidBody.rotation.y, 0, transform.rotation.w));
        Instantiate(fireballPrefab, rightPosition, new Quaternion(0, rigidBody.rotation.y, 0, transform.rotation.w));
    }

    private void shootGiantFireball() {
        fireballPrefab.transform.localScale = new Vector3(2, 2, 2);
        Instantiate(fireballPrefab, wand.position, new Quaternion(0, rigidBody.rotation.y, 0, transform.rotation.w));
    }

    private void Teleport() {
        if (hp == 6) {
            transform.position = new Vector3(-85f, transform.position.y, 50f);
            rateOfFire = 2.0f;
        }
            
        if (hp == 2)
            transform.position = new Vector3(-95f, transform.position.y, 90f);

    }

    private void lowerHP() {
        float hpPosition = (float)hp / maxHP;
        hpGauge.anchorMax = new Vector2(hpPosition, 1f);
    }
}
