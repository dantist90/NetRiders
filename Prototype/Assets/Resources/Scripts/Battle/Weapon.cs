using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour {

	BattleGUIController GUI;

	public GameObject player;
	PlayerController PC;
	Transform playerCamTr;
	int playerTeam;
	BattleController BC;
	Camera cam;
	Rigidbody RB;

	public float damage = 10f;
	public float range = 50f;
	public float currentRange;
	public float fireRate = 0.05f;
	public float reloadTime = 5f;
	public int magFull = 5;
	public bool magEmpty = false;
	public int magazine;
	public float nextTimeToFire = 0f;
	public enum BarrelCount { one, two };
	public BarrelCount barrels;
	bool currentBarrel;
	public ParticleSystem shootEffect1;
	public ParticleSystem shootEffect2;
	public GameObject impactEffect;
	public Transform shootPoint;
	RaycastHit hit;

	public Transform abilityPoint1;
	public Transform abilityPoint2;

	public GameObject[] gunMeshes;

	public enum WeaponTypes { Pistols, Shotgun, Machinegun, Snipergun };
	public WeaponTypes weaponType;

	// Прицел
	public bool isScoped = false;
	public float scopedFOV = 40f;
	public float scopedRange = 100f;
	private float normalFOV;

	// Панель на оружии
	public Text ammoInfo;
	public Text hpInfo;

	private void Start()
	{
		GUI = FindObjectOfType<BattleGUIController>();
		PC = player.GetComponent<PlayerController>();
		playerCamTr = PC.FPSCamera.transform;
		playerTeam = PC.teamNumber;
		BC = FindObjectOfType<BattleController>();
		cam = PC.FPSCamera.GetComponent<Camera>();
		RB = player.GetComponent<Rigidbody>();

		magazine = magFull;
		UpdateAmmoInfo();
		currentRange = range;
	}

	private void Update()
	{
		if (PC.playerType == PlayerController.PlayerTypes.User)
		{
			if (Physics.Raycast(playerCamTr.position, playerCamTr.forward, out hit, currentRange))
			{
				float dist = Vector3.Distance(hit.transform.position, transform.position);
				if (hit.transform.gameObject.tag == "Player" && dist <= currentRange && Time.time >= nextTimeToFire && !magEmpty && PC.shootEnabled)
				{
					Shoot();
				}
			}
			if (Input.GetButtonDown("Fire2") && PC.shootEnabled)
			{
				isScoped = !isScoped;
				PC.weaponHolderAnim.SetBool("Scoped", isScoped);
				if (isScoped)
				{
					StartCoroutine(OnScoped());
				}
				else
				{
					OnUnscoped();
				}
			}
		}
	}
	void Shoot()
	{
		PlayerController hitPC = hit.transform.gameObject.GetComponent<PlayerController>();
		if (hitPC.teamNumber != playerTeam)
		{
			if (magazine != 0)
			{
				nextTimeToFire = Time.time + fireRate;
				magazine--;
				UpdateAmmoInfo();

				if (PC.invisible) { PC.SetVisible(); }
				if (magazine == 0)
				{
					magEmpty = true;
					StartCoroutine("Reload");
				}
			}
			else
			{
				magEmpty = true;
				StartCoroutine("Reload");
			}

			if (barrels == BarrelCount.one)
			{
				shootEffect1.Play();
			}
			else
			{
				if (currentBarrel)
				{
					shootEffect1.Play();
					currentBarrel = false;
				}
				else
				{
					shootEffect2.Play();
					currentBarrel = true;
				}
			}
			
			hitPC.TakeDamage(damage);
			GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
			Destroy(impactGO, 2f);
		}
	}

	IEnumerator Reload()
	{
		yield return new WaitForSeconds(reloadTime);
		magazine = magFull;
		magEmpty = false;
		UpdateAmmoInfo();
	}

	// Прицел
	void OnUnscoped()
	{
		GUI.ShowHudPanel();
		currentRange = range;
		cam.fieldOfView = normalFOV;
		PC.WeaponCamera.SetActive(true);
		PC.Unfreeze();
	}
	IEnumerator OnScoped()
	{
		if (PC.FPC.m_Jumping && PC.flight)
		{
			PC.Freeze();
			StartCoroutine(PC.FlightFreeze());
		}

		currentRange = scopedRange;
		yield return new WaitForSeconds(.15f);
		normalFOV = cam.fieldOfView;
		cam.fieldOfView = scopedFOV;
		if (weaponType == WeaponTypes.Snipergun) {
			PC.WeaponCamera.SetActive(false);
			GUI.ShowSnipePanel();
		}
	}

	void UpdateAmmoInfo()
	{
		string magazineS = magazine.ToString();
		string magFullS = magFull.ToString();
		ammoInfo.text = string.Format("{0} / {1}", magazineS, magFullS);
		if (PC.playerType == PlayerController.PlayerTypes.User)
		{
			GUI.ammoInfo.text = string.Format("Ammo: {0} / {1}", magazineS, magFullS);
		}
	}
}
