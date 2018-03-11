using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaverBullet : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Enemy") {
			Destroy (other.gameObject);
			Destroy (this);
		} else if (other.tag == "WallButton") {
			other.GetComponent<WallButton> ().Activate ();
		}
	}
}
