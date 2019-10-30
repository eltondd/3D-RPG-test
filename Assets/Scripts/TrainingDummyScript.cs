using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummyScript : MonoBehaviour {

    public int maxHP = 5;
    public RectTransform hpGauge;
    // pointer to events gameobject
    public GameObject eventSystem;

    private Animator animator;
    private int hp;
    private float invulnerableTimer;

    // Use this for initialization
    void Awake () {
        animator = GetComponent<Animator>();
        hp = maxHP;
        invulnerableTimer = 2;
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Weapon" && invulnerableTimer > 1) {
            invulnerableTimer = 0;
            animator.Play("TrainingDummy_Damaged");
            if (--hp == 0) {
                Destroy(this.gameObject);
                eventSystem.GetComponent<GlobalEvents>().raisePlayerHealth();
            }
        }
    }

    // Update is called once per frame
    void Update () {
        lowerHP();
        invulnerableTimer += Time.deltaTime;
	}

    private void lowerHP()
    {
        float hpPosition = (float)hp / maxHP;
        hpGauge.anchorMax = new Vector2(hpPosition, 1f);
    }
}
