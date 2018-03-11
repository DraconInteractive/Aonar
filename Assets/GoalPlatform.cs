using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlatform : MonoBehaviour {

	AudioSource aS;
	public AudioClip clip, touchClip;

	bool activated = false;

	public GameObject[] assocTiles;
	public GameObject[] activateChunks, deactivateChunks;
	public float range;

	[Header("Target Properties")]
	public bool target = false;

	[Header("Pedestal Properties")]
	public bool pedestal = false;
	public Interactable[] pedestals;

	ParticleSystem ps;

	void Awake () {
		ps = GetComponent<ParticleSystem> ();
	}

	void Update () {
//		if (activated) {
//			float dist = Vector3.Distance (transform.position, Player.player.transform.position);
//			if (dist > range) {
//				Destroy (this.gameObject);
//			}
//		}
	}
	void OnCollisionEnter (Collision col) {
		if (col.transform.tag == "Player" && !activated) {
			if (!target && !pedestal) {
				ActivatePlatform ();
			} else if (pedestal) {
				bool allActivated = true;
				foreach (Interactable inter in pedestals) {
					if (!inter.activated) {
						allActivated = false;
					}
				}
				if (allActivated) {
					ActivatePlatform ();
				}
			}

		} 
	}

	void ActivatePlatform () {
		aS = GetComponent<AudioSource> ();
		if (clip != null) {
			aS.PlayOneShot (clip);
		}
		aS.PlayOneShot (touchClip);
		Player.player.ActivateAllOrbitWeavers ();

		foreach (GameObject g in activateChunks) {
			g.SetActive (true);
		}
		foreach (GameObject g in deactivateChunks) {
			g.SetActive (false);
		}

		Player.player.checkpoint = this.gameObject;

		GetComponent<Renderer> ().material.SetFloat ("_DissolveDistance", 50);
		GetComponent<Renderer> ().material.SetColor ("_Emission", Color.green);

		ps.Play ();

		activated = true;
	}

	void OnTriggerEnter (Collider col) {
		if (col.transform.tag == "Miniweaver" && !activated && target) {

			if (clip != null) {
				aS = GetComponent<AudioSource> ();
				aS.PlayOneShot (clip);
			}

			foreach (GameObject g in activateChunks) {
				g.SetActive (true);
			}
			foreach (GameObject g in deactivateChunks) {
				g.SetActive (false);
			}

			activated = true;
		}
	}

	public void ActivateArea () {
		foreach (GameObject g in assocTiles) {
			g.SetActive (true);
		}
	}
}
