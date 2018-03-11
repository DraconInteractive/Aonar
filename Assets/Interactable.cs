using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
	Player player;
	public bool activated;

	public bool turnGreen;
	public float greenStrength = 1;

	public GameObject indicator;
	void Awake () {
		
	}
	// Use this for initialization
	void Start () {
		player = Player.player;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Interact () {
		activated = true;

		if (turnGreen) {
			Renderer r = GetComponent<Renderer> ();
			r.material.color = Color.green;
			r.material.SetFloat ("_DissolveDistance", 5);
			r.material.SetColor ("_Emission", Color.green * greenStrength);
			r.material.SetColor ("_EmissionColor", new Color (0, 1, 0.5f, 1));
		}
	}

	void OnTriggerEnter (Collision col) {
		if (col.transform.tag == "Player") {
			indicator.SetActive (true);
		}
	}

	void OnTriggerExit (Collision col) {
		if (col.transform.tag == "Player") {
			indicator.SetActive (false);
		}
	}
}
