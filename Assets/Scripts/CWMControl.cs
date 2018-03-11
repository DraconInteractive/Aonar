using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWMControl : MonoBehaviour {
	public List<DynamicMoving_Wall> walls = new List<DynamicMoving_Wall>();

	public void ButtonInput (float input) {
		foreach (DynamicMoving_Wall wall in walls) {
			wall.speed += wall.speedMod * input * Time.deltaTime;
		}
		print ("Button Receive");
	}
}
