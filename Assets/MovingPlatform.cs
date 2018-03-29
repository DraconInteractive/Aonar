using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
	public GameObject endPoint;
	Rigidbody rb;

	public float speed;

	public bool activated;
	void Awake () {
		rb = GetComponent<Rigidbody> ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision col) 
	{
		if (col.transform.tag == "Player" && !activated)
		{
			StartCoroutine (StartMove ());
		}
	}

	IEnumerator StartMove () {
		activated = true;
		while (Vector3.Distance(transform.position, endPoint.transform.position) > 1) {
			rb.velocity = (endPoint.transform.position - transform.position).normalized * speed * Time.deltaTime;
			yield return null;
		}
		//activated = false;
		rb.velocity = Vector3.zero;
		rb.isKinematic = true;
		yield break;
	}
}
