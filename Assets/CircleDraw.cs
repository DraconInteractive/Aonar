using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDraw : MonoBehaviour {
	[Range(0,50)]
	public int segments = 50;
	[Range(0,5)]
	public float xRadius = 5;
	[Range(0,5)]
	public float yRadius = 5;
	LineRenderer line;

	public void MakeCircle () {
		line = GetComponent<LineRenderer> ();
		line.positionCount = segments + 1;
		line.useWorldSpace = false;

		float x, y, z;

		float angle = 20f;

		for (int i = 0; i < (segments + 1); i++) {
			x = Mathf.Sin(Mathf.Deg2Rad * angle) * xRadius;
			z = Mathf.Cos (Mathf.Deg2Rad * angle) * yRadius;

			line.SetPosition (i, new Vector3 (x, 0, z));

			angle += (360f / segments);
		}
	}
}
