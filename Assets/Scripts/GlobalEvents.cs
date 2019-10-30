using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvents : MonoBehaviour {

    public GameObject player;
    public GameObject wizard;
    public GameObject trainingDummy;

    public GameObject gameOver;
    public GameObject gameWin;
    public GameObject bossHPGauge;

    public Light crystalLight;
    public Transform mainCamera;
	
    public void raisePlayerHealth() {
        player.GetComponent<PlayerScript>().maxHP += 10;
        player.GetComponent<PlayerScript>().hp += 10;
    }

	public void EndGameCutscene() {
        player.GetComponent<PlayerScript>().movable = false;
        Vector3.MoveTowards(player.transform.position, new Vector3(-90f, 0, 95f), 20f);
        player.transform.LookAt(crystalLight.transform);
        mainCamera.LookAt(crystalLight.transform.position);
        while (crystalLight.intensity < 2f) {
            crystalLight.intensity += .05f;
        }
        gameWin.SetActive(true);
        bossHPGauge.SetActive(false);
    }

    public void openGameOverMenu() {
        player.GetComponent<PlayerScript>().movable = false;
        wizard.GetComponent<WizardScript>().canAttack = false;
        gameOver.SetActive(true);
    }
}
