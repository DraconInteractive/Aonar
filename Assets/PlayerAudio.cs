using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
	public static PlayerAudio pAudio;
	AudioSource aS;
	public AudioSource mainAS;
	public AudioClip[] ambientClips, shootMiniClips, shootLaserClips;
	// Use this for initialization

	void Awake () {
		pAudio = this;
	}

	void Start () {
		aS = GetComponent<AudioSource> ();
		StartCoroutine (PlayClips ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void NewBackground () {
		StartCoroutine (PlayClips ());
	}
	IEnumerator PlayClips () {
		int i = 0;
		int ii = ambientClips.Length;

		while (i < ii) {
			aS.clip = ambientClips[i];
			aS.Play ();
			while (aS.isPlaying) {
				yield return null;
			}
			i++;
		}

		NewBackground ();
	}
}
