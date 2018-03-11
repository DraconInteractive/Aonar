using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor ;

[CustomEditor(typeof(StoryPath))]
public class StoryPath_Editor : Editor {

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		var myTarget = (StoryPath)target;

		EditorGUILayout.Space ();

		GUI.color = new Color (1, 1, 0.0f);
		if (GUILayout.Button("Add Point")) {
			GameObject g = myTarget.AddPoint ();
			Selection.activeGameObject = g;
		}

		if (GUILayout.Button ("Setup Line")) {
			myTarget.Setup ();
		}


	}
}
