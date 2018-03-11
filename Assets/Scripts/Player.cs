using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class Player : MonoBehaviour {
	public static Player player;
	Rigidbody rb;
	public float movementSpeed, turnSpeed, jumpForce;
	public float downMult;
	public GameObject weaver;
	public GameObject weaverPrefab;
	public GameObject[] orbitWeavers;
	Coroutine laserRoutine;
	bool laserActive;

	public float sprintMod;

	public bool isWebGL;

	public GameObject checkpoint;

	public GameObject PETI;
	void Awake () {
		player = this;
		rb = GetComponent<Rigidbody> ();
	}

	// Use this for initialization
	void Start () {
		ToggleCursorState (false);
		Camera.main.transform.GetComponent<PostProcessingBehaviour>().profile.bloom.enabled = !isWebGL;
		Fade.fade.StartFade (false, 1);

		//StartCoroutine (SelectableSearch ());
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation *= Quaternion.Euler (0, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0);

		if (Input.GetKeyDown(KeyCode.Space)) {
			rb.AddForce (transform.up * jumpForce, ForceMode.VelocityChange);
		}

		if (Input.GetKeyDown(KeyCode.Tab)) {
			ToggleCursorState (!Cursor.visible);
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			Interact ();
		}
		if (Input.GetMouseButtonDown(0)) {
			Shoot ();
		}

		if (Input.GetMouseButtonDown(1)) {
			StartLaser ();
		}

		if (Input.GetMouseButtonUp(1)) {
			EndLaser ();
		}
	}

	void FixedUpdate () {
		Movement ();

		if (rb.velocity.y < 0) {
			Vector3 velMod = rb.velocity;
			velMod *= (1 + Time.deltaTime * downMult);
			rb.velocity = velMod;
//			print ("p");
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Laser") {
			Kill ();
		} else if (other.tag == "Tile") {
			PuzzleTile tile = other.GetComponent<PuzzleTile> ();
			if (tile.myType == PuzzleTile.Type.Death) {
				Kill ();
			}
		}
	}

//	void OnTriggerStay (Collider other) {
//		if (other.tag == "Laser") {
//			
//		}
//	}

	bool OnGround () {
		Ray ray = new Ray (transform.position + transform.up * 0.1f, Vector3.down);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 2)) {
			if (hit.transform.tag == "Ground") {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}

	void Movement () {
		float sprint = 1;
		if (Input.GetKey(KeyCode.LeftShift)) {
			sprint = sprintMod;
		}
		float f = Input.GetAxis ("Vertical") * movementSpeed * sprint * Time.deltaTime;
		float r = Input.GetAxis ("Horizontal") * movementSpeed * Time.deltaTime;
		Vector3 targetPosition = transform.position + transform.forward * f + transform.right * r;
		rb.MovePosition (targetPosition);
	}

	IEnumerator SelectableSearch () {
		while (true) {
			Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 10)) {
				if (hit.transform.tag == "Interactable") {
					PETI.SetActive (true);
				} else {
					PETI.SetActive (false);
				}
			} else {
				PETI.SetActive (false);
			}
			yield return new WaitForSeconds (0.25f);
		}
		yield break;
	}

	void Interact () {
		Ray ray = new Ray (Camera.main.transform.position, Camera.main.transform.forward);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 10)) {
			if (hit.transform.tag == "Interactable") {
				Interactable i = hit.transform.GetComponent<Interactable> ();
				i.Interact ();
			} else {
				print (hit.transform.name);
			}
		} else {
			print ("No hit");
		}
	}
	void ToggleCursorState (bool state) {
		Cursor.visible = state;
		if (state) {
			Cursor.lockState = CursorLockMode.None;
		} else {
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	public void Shoot () {
		if (GetActiveOrbitWeavers() > 0) {
			DeactivateOrbitWeaver ();
		} else {
			return;
		}
		StartCoroutine (ShootWeaver ());
		PlayerAudio.pAudio.mainAS.PlayOneShot (PlayerAudio.pAudio.shootMiniClips [Random.Range(0, PlayerAudio.pAudio.shootMiniClips.Length)]);
	}

	IEnumerator ShootWeaver () {
		GameObject newWeaver = Instantiate (weaverPrefab, weaver.transform.position, Quaternion.identity);
		newWeaver.transform.forward = (Camera.main.transform.position + Camera.main.transform.forward * 100) - weaver.transform.position;
		MoveOrRotate mover = newWeaver.AddComponent<MoveOrRotate> ();
		mover.miniweaverSpecial = true;
		mover.movement = new Vector3 (0, 0, 6 + Mathf.Abs(Input.GetAxis("Vertical") * movementSpeed));
		Destroy (newWeaver, 10);
		yield break;
	}

	public void StartLaser () {
		laserActive = true;
		if (laserRoutine != null) {
			StopCoroutine (laserRoutine);
		}
		PlayerAudio.pAudio.mainAS.PlayOneShot (PlayerAudio.pAudio.shootLaserClips [0]);
		laserRoutine = StartCoroutine (Laser ());
	}

	public void EndLaser () {
		laserActive = false;
	}

	IEnumerator Laser () {
		LineRenderer line = weaver.GetComponent<LineRenderer> ();
		float laserExtension = 0;

		while (laserExtension < 100) {
			laserExtension = Mathf.Lerp (laserExtension, 250, 0.5f);
			line.SetPosition (1, new Vector3 (0, 0, laserExtension));
			yield return null;
		}
		float laserTimer = 0;
		while (laserActive) {
			Ray ray = new Ray (weaver.transform.position, weaver.transform.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100)) {
				if (hit.transform.tag == "Tile") {
					PuzzleTile tile = hit.transform.GetComponent<PuzzleTile> ();
					tile.currentAlpha += Time.deltaTime * 2;
				} else if (hit.transform.tag == "CMW") {
					CMW_Controllers c = hit.transform.GetComponent<CMW_Controllers> ();
					c.SendInput ();
					print ("Ray hit CMW");
				}
			}
			laserTimer += Time.deltaTime;
			if (laserTimer > 1.3f) {
				laserActive = false;
			}
			yield return null;
		}

		while (laserExtension > 0.05f) {
			laserExtension = Mathf.Lerp (laserExtension, 0, 0.5f);
			line.SetPosition (1, new Vector3 (0, 0, laserExtension));
			yield return null;
		}

		laserRoutine = null;
		yield break;
	}
		
	public void Kill () {
		print ("Dead");
	}

	public void ActivateOrbitWeaver () {
		foreach (GameObject g in orbitWeavers) {
			if (g.activeSelf == false) {
				g.SetActive (true);
				break;
			}
		}
	}

	public void DeactivateOrbitWeaver () {
		GameObject lastActiveWeaver = null;
		foreach (GameObject g in orbitWeavers) {
			if (g.activeSelf) {
				lastActiveWeaver = g;
			}
		}
		if (lastActiveWeaver != null) {
			lastActiveWeaver.SetActive (false);
		}

	}

	public void ActivateAllOrbitWeavers () {
		foreach (GameObject g in orbitWeavers) {
			g.SetActive (true);
		}
	}

	public int GetActiveOrbitWeavers () {
		int i = 0;
		foreach (GameObject g in orbitWeavers) {
			if (g.activeSelf) {
				i++;
			}
		}
		return i;
	}
}
