using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaver : MonoBehaviour {
	GameObject cam;
	float timer = 0;
	public float speed, amplitude;
	// Use this for initialization
	void Start () {
		cam = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
		timer += Time.deltaTime * speed;
		float r = Mathf.PerlinNoise (timer, 0) * amplitude;
		Vector3 lp = transform.localPosition;
		lp.y = -0.25f + r;
		transform.localPosition = lp;
	}
}
