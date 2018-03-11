using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMoving_Wall : Wall_Moving {
	public float speedMod;

	Vector3 currentPos, lastFramePos;
	Vector3 vel;


	public override IEnumerator MoveWall () {
		while (true) {
			lastFramePos = transform.position;
			if (toStart) {
				transform.position = Vector3.MoveTowards (transform.position, start.transform.position, speed * Time.deltaTime);
				if (Vector3.Distance(transform.position, start.transform.position) < 0.1f && speed > 0) {
					toStart = false;
				} else if (Vector3.Distance(transform.position, end.transform.position) < 0.1f && speed < 0) {
					toStart = false;
				}
			} else {
				transform.position = Vector3.MoveTowards (transform.position, end.transform.position, speed * Time.deltaTime);
				if (Vector3.Distance(transform.position, end.transform.position) < 0.1f && speed > 0) {
					toStart = true;
				} else if (Vector3.Distance(transform.position, start.transform.position) < 0.1f && speed < 0) {
					toStart = true;
				}
			}
			currentPos = transform.position;
			yield return null;
		}
	}

	void Update () {
		vel = currentPos - lastFramePos;
	}
	void OnCollisionStay (Collision col) {
		if (col.transform.tag == "Player") {
			col.transform.position += vel * 0.5f;
		}
	}
}
