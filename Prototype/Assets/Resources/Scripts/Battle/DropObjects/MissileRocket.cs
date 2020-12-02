using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileRocket : MonoBehaviour {

	public float damage = 100f;

	void Awake()
	{
		StartCoroutine(Timer());
	}
	IEnumerator Timer()
	{
		yield return new WaitForSeconds(10f);
		Destroy();
	}

	void OnCollisionEnter(Collision col)
	{
		Destroy();
	}
	void Destroy()
	{
		GameObject destroyEffect = Instantiate(Resources.Load("Prefabs/Effects/DestroyEffect", typeof(GameObject)), gameObject.transform.position, Quaternion.identity) as GameObject;
		destroyEffect.GetComponent<DestroyEffect>().damage = damage;
		Destroy(gameObject, 0f);
	}
}
