using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour {

	public GameObject[] checkpoints;
	bool dying;
	void OnCollisionEnter (Collision col) {
		if (col.transform.tag == "Player") {
			if (!dying) {
				StartCoroutine (Die ());
			}

		}
	}

	IEnumerator Die () {
		dying = true;
		Fade.fade.StartPulseFade (1);
		yield return new WaitForSeconds (1);
		Player.player.transform.position = Player.player.checkpoint.transform.position + Vector3.up * 0.5f;
		dying = false;
		yield break;
	}
}
