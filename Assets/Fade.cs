using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {
	public static Fade fade;
	Image img;
	Coroutine fadeRoutine;
	void Awake () {
		fade = this;
		img = GetComponent<Image> ();
	}

	public void StartFade (bool toBlack, float speed) {
		if (fadeRoutine != null) {
			StopCoroutine (fadeRoutine);
		}
		fadeRoutine = StartCoroutine (DoFade (toBlack, speed));
	}

	public void StartPulseFade (float speed) {
		if (fadeRoutine != null) {
			StopCoroutine (fadeRoutine);
		}
		fadeRoutine = StartCoroutine (PulseFade (speed));
	}

	IEnumerator DoFade (bool toBlack, float speed) {
		if (toBlack) {
			while (img.color.a < 1) {
				Color c = img.color;
				c.a += Time.deltaTime * speed;
				img.color = c;
				yield return null;
			}
		}
		else 
		{
			while (img.color.a > 0) {
				Color c = img.color;
				c.a -= Time.deltaTime * speed;
				img.color = c;
				yield return null;
			}
		}
		yield break;
	}

	IEnumerator PulseFade (float speed) {
		while (img.color.a < 1) {
			Color c = img.color;
			c.a += Time.deltaTime * speed;
			img.color = c;
			yield return null;
		}
		yield return new WaitForSeconds (0.25f);
		while (img.color.a > 0) {
			Color c = img.color;
			c.a -= Time.deltaTime * speed;
			img.color = c;
			yield return null;
		}
		yield break;
	}
}
