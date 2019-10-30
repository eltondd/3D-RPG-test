using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTrigger : MonoBehaviour {

    public GameObject boss;
    public Light ambientLight;

    public GameObject bossName;
    public GameObject bossHPGauge;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            boss.SetActive(true);
            ambientLight.intensity = 0.75f;
            bossName.SetActive(true);
            bossHPGauge.SetActive(true);
        }
    }

}
