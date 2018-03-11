using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CircleDraw))]
public class CircleDrawEditor : Editor {

	public override void OnInspectorGUI () {
		DrawDefaultInspector ();
		var myTarget = (CircleDraw)target;

		if (GUILayout.Button("Make Circle")) {
			myTarget.MakeCircle ();
		}
	}
}
