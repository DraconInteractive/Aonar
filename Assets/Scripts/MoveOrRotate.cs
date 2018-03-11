using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOrRotate : MonoBehaviour {
	public Vector3 movement, rotation;
	public bool local, miniweaverSpecial;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (miniweaverSpecial) {
			transform.position += transform.forward * movement.z * Time.deltaTime;
		} else {
			transform.position += movement * Time.deltaTime;
		}


		if (rotation.magnitude != 0) {
			if (local)
			{
				transform.localRotation *= Quaternion.Euler (rotation * Time.deltaTime);
			}
			else 
			{
				transform.rotation *= Quaternion.Euler (rotation * Time.deltaTime);
			}

		}
	}

//	void OnDrawGizmos () {
//		Gizmos.color = Color.red;
//		Gizmos.DrawLine (transform.position, transform.position + (transform.localPosition + rotation).normalized * 5);
//
//	}
		
}
