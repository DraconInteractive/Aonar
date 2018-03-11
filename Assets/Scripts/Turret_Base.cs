using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Base : MonoBehaviour {
	public enum Type {
		Laser
	};

	public Type type;

	public GameObject target, turret, laserCollider;
	// Use this for initialization
	void Start () {
		if (type == Type.Laser) {
			LineRenderer l = turret.GetComponent<LineRenderer> ();
			l.SetPosition (1, target.transform.position);
			l.SetPosition (0, turret.transform.position);
			laserCollider.transform.position = turret.transform.position + (target.transform.position - turret.transform.position) / 2;
			laserCollider.transform.LookAt (target.transform.position);
			Vector3 colLength = laserCollider.transform.localScale;
			colLength.z = (target.transform.position - turret.transform.position).magnitude;
			laserCollider.transform.localScale = colLength;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
