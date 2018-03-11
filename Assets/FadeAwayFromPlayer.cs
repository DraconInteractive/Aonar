using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAwayFromPlayer : MonoBehaviour {
	Player player;
	Renderer r;
	public float toOne;
	public float distanceFade;
	public enum BlockType
	{
		Standard,
		Goal
	};
	public BlockType myBlockType;
	// Use this for initialization
	void Start () {
		player = Player.player;
		r = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance (transform.position, player.transform.position);

		toOne = dist / distanceFade;
		//toOne = 1 / toOne;
		toOne = Mathf.Pow (toOne, 2);
		r.material.SetFloat ("_DissolvePercentage", toOne);
	}

	void OnCollisionEnter (Collision col) {
		if (col.transform.tag == "Player") {
			//StartCoroutine (SetColorOverTime (Color.green, 1));
			distanceFade = 100;
		}
	}

	IEnumerator SetColorOverTime (Color col, float time) {
		for (float f = 0; f <= 1; f += Time.deltaTime / time) {
			Color c = r.material.color;
			r.material.color = Color.Lerp (c, col, f);
			yield return null;
		}
		yield break;
	}
}
