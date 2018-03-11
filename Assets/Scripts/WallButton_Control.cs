using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallButton_Control : MonoBehaviour {

	public List<WallButton> wallButtons = new List<WallButton>();

	public GameObject affected;
	public int ActiveCheck () {
		int numActive = 0;

		foreach (WallButton b in wallButtons) {
			if (b.active) {
				numActive++;
			}
		}

		if (numActive + 1 == wallButtons.Count) {
			affected.SetActive (false);
			Destroy (this.gameObject, 2);
		}

		return numActive;
	}
}
