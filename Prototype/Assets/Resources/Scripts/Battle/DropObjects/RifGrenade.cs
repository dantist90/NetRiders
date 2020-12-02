using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifGrenade : MonoBehaviour {

	public float damage = 50f;

	void Awake()
	{
		StartCoroutine(Timer());
	}
	IEnumerator Timer()
	{
		yield return new WaitForSeconds(10f);
		StartCoroutine(Destroy());
	}

	void OnCollisionEnter(Collision col)
	{
		StartCoroutine(Destroy());
	}
	IEnumerator Destroy()
	{
		yield return new WaitForSeconds(Random.Range(0.1f, 2f));
		GameObject destroyEffect = Instantiate(Resources.Load("Prefabs/Effects/DestroyEffect", typeof(GameObject)), gameObject.transform.position, Quaternion.identity) as GameObject;
		destroyEffect.GetComponent<DestroyEffect>().damage = damage;
		Destroy(gameObject, 0f);
	}
}
