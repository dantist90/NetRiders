using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegabotWeapon : MonoBehaviour {

	BattleController BC;
	public MegabotController mc;
	public Transform shootSocket1;
	public Transform shootSocket2;
	RaycastHit hit;
	public GameObject impactEffect;
	float nextTimeToFire = 0f;
	public float fireRate = 0.05f;
	public float damage = 10f;

	public enum WeaponTypes { Laser, Rocketgun, Sword };
	public WeaponTypes weaponType;

	public Rigidbody bullet;

	bool shoot = false;

	private void Awake()
	{
		BC = FindObjectOfType<BattleController>();
	}

	private void Update()
	{
		if (Time.time >= nextTimeToFire)
		{
			if (Input.GetButton("Fire1") && mc.currentSocket == 0 && weaponType == WeaponTypes.Laser)
			{
				mc.weaponHolderAnim[0].SetTrigger("Hit");

				if (Physics.Raycast(mc.FPSCamera.transform.position, mc.FPSCamera.transform.forward, out hit, 1000f))
				{
					GameObject impactGO1 = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
					LineRenderer lr1 = impactGO1.GetComponent<LineRenderer>();
					lr1.SetPosition(0, shootSocket1.position);
					lr1.SetPosition(1, hit.point);
					lr1.SetPosition(2, shootSocket2.position);
					Destroy(impactGO1, 0.1f);
				}
				if (hit.transform.gameObject.tag == "Player" && Time.time >= nextTimeToFire)
				{
					PlayerController hitPC = hit.transform.gameObject.GetComponent<PlayerController>();
					if (hitPC.teamNumber != BC.userTeamNumber)
					{
						nextTimeToFire = Time.time + fireRate;
						hitPC.TakeDamage(damage);
					}
				}
			}
			if (Input.GetButtonDown("Fire1") && mc.currentSocket == 1 && weaponType == WeaponTypes.Sword)
			{
				mc.weaponHolderAnim[1].SetTrigger("Hit");

				if (Physics.Raycast(mc.FPSCamera.transform.position, mc.FPSCamera.transform.forward, out hit, 10f))
				{
					if (hit.transform.gameObject.tag == "Player")
					{
						PlayerController hitPC = hit.transform.gameObject.GetComponent<PlayerController>();
						if (hitPC.teamNumber != BC.userTeamNumber)
						{
							nextTimeToFire = Time.time + fireRate;
							hitPC.TakeDamage(damage);
						}
					}
				}
			}
			if (Input.GetButtonDown("Fire1") && mc.currentSocket == 2 && weaponType == WeaponTypes.Rocketgun && shoot == false)
			{
				StartCoroutine("RocketShoot");
			}
		}
	}
	IEnumerator RocketShoot()
	{
		shoot = true;
		for (int i = 0; i < 5; i++)
		{
			mc.weaponHolderAnim[2].SetTrigger("Hit");
			Rigidbody rocketInstance;
			rocketInstance = Instantiate(bullet, shootSocket1.position, shootSocket1.rotation) as Rigidbody;
			rocketInstance.GetComponent<MissileRocket>().damage = damage;

			Vector3 rot = rocketInstance.rotation.eulerAngles;
			rot = new Vector3(rot.x, rot.y + 10, rot.z);
			rocketInstance.rotation = Quaternion.Euler(rot);
			rocketInstance.AddForce(shootSocket1.forward * 3000);
			yield return new WaitForSeconds(fireRate);
		}
		shoot = false;
	}
}
