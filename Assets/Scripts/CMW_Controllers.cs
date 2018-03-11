using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMW_Controllers : MonoBehaviour {
	CWMControl control;
	public float mod;
	void Awake () {
		control = transform.parent.GetComponent<CWMControl> ();
	}

	public void SendInput () {
		control.ButtonInput (mod);
		print ("Button Send");
	}
}
