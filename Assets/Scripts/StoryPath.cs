using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPath : MonoBehaviour {
	public List<GameObject> pathPoints = new List<GameObject>();
	public Material lineMat;
	public GameObject pointPrefab;
	public bool autoSetup;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Setup () {
		pathPoints.Clear ();
		foreach (Transform t in transform) {
			pathPoints.Add (t.gameObject);
		}
		if (pathPoints.Count == 0) {
			return;
		}


		int i = 0;
		foreach (GameObject g in pathPoints) {
			LineRenderer l = g.GetComponent<LineRenderer> ();
			if (l == null) {
				l = g.AddComponent<LineRenderer> ();
			}

			l.startWidth = 0.2f;
			l.endWidth = 0.2f;
			l.material = lineMat;
			l.useWorldSpace = true;
			l.positionCount = 2;
			l.SetPosition (0, g.transform.position);
//			if ((i+1) <= pathPoints.Count) {
//				
//			} else {
//				l.SetPosition (1, g.transform.position);
//			}
			try {
				l.SetPosition (1, pathPoints [i + 1].transform.position);
			} catch {
				l.SetPosition (1, g.transform.position);
			}

			i++;
		}
	}

	public GameObject AddPoint () {
		Vector3 target = transform.position;
		if (transform.childCount > 0) {
			target = transform.GetChild (transform.childCount - 1).position;
		}
		GameObject g = Instantiate (pointPrefab, target, Quaternion.identity);
		g.transform.SetParent (this.transform);
		g.name = "Point: " + transform.childCount;

		if (autoSetup) {
			Setup ();
		}
		return g;
	}
}
