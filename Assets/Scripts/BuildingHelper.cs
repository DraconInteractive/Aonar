using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHelper : MonoBehaviour {
	public float threshold;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Move () {
		transform.position += Vector3.up * Random.Range (-threshold, threshold) + transform.right * Random.Range (-threshold, threshold) + transform.forward * Random.Range (-threshold, threshold);
	}
}
