using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour {

	public float damage = 100f;

	void Awake()
	{
		gameObject.GetComponent<ParticleSystem>().Play();
		Destroy(gameObject, 3f);
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			if (col.gameObject.GetComponent<PlayerController>() && col.gameObject.GetComponent<PlayerController>().playerType == PlayerController.PlayerTypes.Bot)
			{
				col.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
			}
		}
	}
}
