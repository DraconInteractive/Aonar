using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Moving : MonoBehaviour {
	public GameObject start, end;
	[HideInInspector]
	public bool toStart;
	public float speed;
	// Use this for initialization
	void Start () {
		transform.position = start.transform.position;
		toStart = false;
		StartCoroutine (MoveWall ());
	}

	public virtual IEnumerator MoveWall () {
		while (true) {
			if (toStart) {
				transform.position = Vector3.MoveTowards (transform.position, start.transform.position, speed * Time.deltaTime);
				if (Vector3.Distance(transform.position, start.transform.position) < 0.1f) {
					toStart = false;
				}
			} else {
				transform.position = Vector3.MoveTowards (transform.position, end.transform.position, speed * Time.deltaTime);
				if (Vector3.Distance(transform.position, end.transform.position) < 0.1f) {
					toStart = true;
				}
			}
			yield return null;
		}
	}
}
