using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrigger : MonoBehaviour {

	public Collider door1;
	public Collider door2;

	void OnTriggerStay(Collider other)
	{
		door1.enabled = false;
		door2.enabled = false;
	}
	void OnTriggerExit(Collider other)
	{
		door1.enabled = true;
		door2.enabled = true;
	}
}
