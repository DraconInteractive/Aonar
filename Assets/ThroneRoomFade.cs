using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThroneRoomFade : MonoBehaviour {
	public float delta;
	// Use this for initialization
	void Start () {
		StartCoroutine (Fade ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Fade () {
		Image i = GetComponent<Image> ();
		i.color = Color.black;
		delta = 0;
		for (float f = 0; f < 1; f += Time.deltaTime) {
			delta = f;
			i.color = Color.Lerp (Color.black, Color.clear, f);
			yield return null;
		}

		Destroy (transform.parent.gameObject, 0.1f);
		yield break;
	}
}
