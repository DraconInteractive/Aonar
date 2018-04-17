using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroneInteractable : Interactable {

	public override void Interact ()
	{
		base.Interact ();
		ThroneRoomController.instance.StartSequence ();
	}
}
