using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildingHelper))]
public class BuildingHelper_Editor : Editor {

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		var myTarget = (BuildingHelper)target;
		if (GUILayout.Button("Move Random")) {
			myTarget.Move ();
		}
	}
}
