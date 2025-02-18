﻿using System.Collections;
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

	[Header("Hover Properties")]
	public Vector3 hoverOffset;
	public GameObject hoverPiece, instHover;

	[Header("Scene Changer Properties")]
	public bool sceneChanger;
	public string sceneName;

	void Awake () {
		ps = GetComponent<ParticleSystem> ();
	}

	void Start () {
		if (hoverPiece != null) {
			instHover = Instantiate (hoverPiece, transform.position + hoverOffset, transform.rotation, this.transform) as GameObject;
		}
	}

	void Update () {

	}

	void OnCollisionEnter (Collision col) {
		if (col.transform.tag == "Player" && !activated) {
			if (!target && !pedestal && !sceneChanger) {
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
			} else if (sceneChanger) {
				StartCoroutine (NextScene ());
			}

		} 
	}

	IEnumerator NextScene () {
		Fade.fade.StartFade (true, 1);
		yield return new WaitForSeconds (1);
		UnityEngine.SceneManagement.SceneManager.LoadScene (sceneName);
		yield break;

	}
	void ActivatePlatform () {
		aS = GetComponent<AudioSource> ();
		if (clip != null) {
			aS.PlayOneShot (clip);
		}
		aS.PlayOneShot (touchClip);
		Player.player.ActivateAllOrbitWeavers ();

		if (activateChunks.Length > 0) {
			foreach (GameObject g in activateChunks) {
				g.SetActive (true);
			}
		}

		if (deactivateChunks.Length > 0) {
			foreach (GameObject g in deactivateChunks) {
				g.SetActive (false);
			}
		}


		Player.player.checkpoint = this.gameObject;

		GetComponent<Renderer> ().material.SetFloat ("_DissolveDistance", 50);
		GetComponent<Renderer> ().material.SetColor ("_Emission", Color.green);

		ps.Play ();

		if (instHover != null) {
			Destroy (instHover);
		}
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
