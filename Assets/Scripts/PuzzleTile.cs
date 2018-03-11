using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTile : MonoBehaviour {
	public enum Type {
		Death,
		Life
	};

	public Type myType;

	public float maxAlpha;
	public float currentAlpha;
	Renderer r;

	Color c;
	void Start () {
		r = GetComponent<Renderer> ();
		c = r.material.color;
		currentAlpha = 0;
	}
	void Update () {
		c.a = currentAlpha;
		r.material.color = c;
		if (currentAlpha > 0) {
			currentAlpha -= Time.deltaTime;
		}
	}
}
