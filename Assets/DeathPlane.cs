using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		print ("dying");
		dying = true;
		Fade.fade.StartFade(true, 1);
		yield return new WaitForSeconds (1);
		//Player.player.transform.position = Player.player.checkpoint.transform.position + Vector3.up * 0.5f;
		//dying = false;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		yield break;
	}
}
