using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public float eyeHeight;
	float upAngle;
	Player player;
	Transform p;

	public float headY, speed, amplitude;
	float timer;

	public AudioClip[] footsteps;
	AudioSource aS;
	// Use this for initialization
	void Start () {
		player = Player.player;
		p = player.transform;
		timer = 0;
		aS = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = Vector3.zero + p.up * eyeHeight;
//		Quaternion rot = p.rotation;
		upAngle -= Input.GetAxis ("Mouse Y") * player.turnSpeed * Time.deltaTime;
		Quaternion rot = transform.localRotation;
		rot = Quaternion.Euler (Mathf.Clamp(upAngle, -90, 90),0,0);
		transform.localRotation = rot;

		Vector3 lp = transform.localPosition;
		timer += Time.deltaTime * speed * Input.GetAxis("Vertical");
		headY = Mathf.Sin (timer) * amplitude;
		if (Input.GetAxis("Vertical") > 0 && headY < -0.08f) {
			print ("footstep");
			aS.PlayOneShot (footsteps [Random.Range (0, footsteps.Length)]);
		}
		lp.y = eyeHeight + headY;
		transform.localPosition = lp;
	}
}
