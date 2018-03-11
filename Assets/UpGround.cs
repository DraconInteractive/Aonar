using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpGround : MonoBehaviour {
	public Vector3 initPos;
	Player player;
	public bool willFall = true;
	//Renderer r;
	//public float toOne;
	// Use this for initialization
	void Awake () {
		initPos = transform.position;
		//r = GetComponent<Renderer> ();
	}

	void Start () {
		player = Player.player;
	}

	void Update () {
		if (willFall) {
			float dist = Vector3.Distance (transform.position, player.transform.position);
			if (dist > 10) {
				transform.position = Vector3.MoveTowards (transform.position, initPos - Vector3.up * 4.5f, 6 * Time.deltaTime);
			} else {
				transform.position = Vector3.MoveTowards (transform.position, initPos, 6 * Time.deltaTime);
			}
		}

		/*
		toOne = dist / 3;
		toOne = 1 / toOne;
		toOne = Mathf.Pow (toOne, 4);
		Color c = r.material.color;
		c.a = Mathf.Clamp(toOne, 0, 1);
		r.material.color = c;
		*/
	}
}
