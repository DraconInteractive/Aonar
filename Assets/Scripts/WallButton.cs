using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallButton : MonoBehaviour {

	public Color activeColor, inactiveColor;
	Renderer r;

	public int index;

	public bool active;

	WallButton_Control controller;
	void Awake () {
		r = GetComponent<Renderer> ();
		controller = transform.parent.GetComponent<WallButton_Control> ();
	}

	public void Activate () {

		int numActive = controller.ActiveCheck ();


		if (numActive == (index - 1)) {
			r.material.color = activeColor;
			active = true;
		} else {
			foreach (WallButton b in controller.wallButtons) {
				b.Deactivate ();
			}
		}


	}

	public void Deactivate () {
		r.material.color = inactiveColor;
		active = false;
	}
}
