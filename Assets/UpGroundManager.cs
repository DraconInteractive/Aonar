using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpGroundManager : MonoBehaviour {
	public UpGround[] allGrounds;
	Player player;
	public float speed;
	// Use this for initialization
	void Start () {
		player = Player.player;
		allGrounds = GetComponentsInChildren<UpGround> ();
	}
	
	// Update is called once per frame
	void Update () {
//		foreach (UpGround ground in allGrounds) {
//			float dist = Vector3.Distance (ground.transform.position, player.transform.position);
//			if (dist > 10) {
//				ground.transform.position = Vector3.MoveTowards (ground.transform.position, ground.initPos - Vector3.up * 4.5f, speed * Time.deltaTime);
//			} else {
//				ground.transform.position = Vector3.MoveTowards (ground.transform.position, ground.initPos, speed * Time.deltaTime);
//			}
//		}
	}
}
