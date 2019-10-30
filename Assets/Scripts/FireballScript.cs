using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireballScript : MonoBehaviour {

    public float speed = 15f;

    private Rigidbody rigidBody;
    private GameObject player;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(player.transform.position);
        Invoke("DestroySelf", 3.0f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            // DestroySelf();
        }
    }

    private void FixedUpdate() {
        rigidBody.MovePosition(rigidBody.position + 
            transform.forward * speed * Time.deltaTime);
    }

    private void DestroySelf()  {
        Destroy(gameObject);
    }
}
