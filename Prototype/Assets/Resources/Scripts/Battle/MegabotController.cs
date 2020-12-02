using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegabotController : MonoBehaviour {

	UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController FPC;
	BattleController BC;

	public int teamNumber;
	public float health;

	public float energy;
	public float maxEnergy = 1000f;
	public GameObject FPSCamera;
	public int currentSocket = 3;
	public GameObject[] weaponHolders;
	public Animator[] weaponHolderAnim = new Animator[3];
	GameObject[] weapons = new GameObject[3];
	MegabotWeapon[] mw = new MegabotWeapon[3];

	float fSpeed;
	float bSpeed;
	float sSpeed;

	private void Awake()
	{
		BC = FindObjectOfType<BattleController>();
		FPC = GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
		fSpeed = FPC.movementSettings.ForwardSpeed;
		bSpeed = FPC.movementSettings.BackwardSpeed;
		sSpeed = FPC.movementSettings.StrafeSpeed;

		for (int i = 0; i < 3; i++)
		{
			weaponHolderAnim[i] = weaponHolders[i].GetComponent<Animator>();
		}
		CreateWeapons();
	}
	private void Start()
	{
		energy = maxEnergy;
	}

	private void Update()
	{
		if (Input.GetKeyDown("1"))
		{
			currentSocket = 3;
		}
		if (Input.GetKeyDown("2"))
		{
			currentSocket = 0;
		}
		if (Input.GetKeyDown("3"))
		{
			currentSocket = 1;
		}
		if (Input.GetKeyDown("4"))
		{
			currentSocket = 2;
		}

		if (currentSocket < 3)
		{
			FPC.movementSettings.ForwardSpeed = 0;
			FPC.movementSettings.BackwardSpeed = 0;
			FPC.movementSettings.StrafeSpeed = 0;
		}
		else
		{
			FPC.movementSettings.ForwardSpeed = fSpeed;
			FPC.movementSettings.BackwardSpeed = bSpeed;
			FPC.movementSettings.StrafeSpeed = sSpeed;
		}
	}

	void CreateWeapons ()
	{
		weapons[0] = Instantiate(Resources.Load("Prefabs/MegabotWeapons/Laser", typeof(GameObject)), weaponHolders[0].transform.position, weaponHolders[0].transform.rotation) as GameObject;
		weapons[0].transform.SetParent(weaponHolders[0].transform);
		weaponHolderAnim[0].runtimeAnimatorController = Resources.Load("Animation/MegabotWeapons/Laser") as RuntimeAnimatorController;
		mw[0] = weapons[0].GetComponent<MegabotWeapon>();
		mw[0].mc = this;

		weapons[1] = Instantiate(Resources.Load("Prefabs/MegabotWeapons/Sword", typeof(GameObject)), weaponHolders[1].transform.position, weaponHolders[1].transform.rotation) as GameObject;
		weapons[1].transform.SetParent(weaponHolders[1].transform);
		weaponHolderAnim[1].runtimeAnimatorController = Resources.Load("Animation/MegabotWeapons/Sword") as RuntimeAnimatorController;
		mw[1] = weapons[1].GetComponent<MegabotWeapon>();
		mw[1].mc = this;

		weapons[2] = Instantiate(Resources.Load("Prefabs/MegabotWeapons/Rocketgun", typeof(GameObject)), weaponHolders[2].transform.position, weaponHolders[2].transform.rotation) as GameObject;
		weapons[2].transform.SetParent(weaponHolders[2].transform);
		weaponHolderAnim[2].runtimeAnimatorController = Resources.Load("Animation/MegabotWeapons/Rocketgun") as RuntimeAnimatorController;
		mw[2] = weapons[2].GetComponent<MegabotWeapon>();
		mw[2].mc = this;
	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		if (health <= 0f)
		{
			Die();
		}
	}

	void Die()
	{
		GameObject destroyEffect = Instantiate(Resources.Load("Prefabs/Effects/DestroyEffect", typeof(GameObject)), gameObject.transform.position, Quaternion.identity) as GameObject;
		destroyEffect.GetComponent<DestroyEffect>().damage = 0f;
		BC.megabotA = false;
		BC.teamA.Clear();
		if (BC.botEnabled)
		{
			BC.Spawn("User", teamNumber, 0, true);
			BC.Spawn("Bot", teamNumber, 1, true);
			BC.Spawn("Bot", teamNumber, 2, true);
			BC.Spawn("Bot", teamNumber, 3, true);
		}
		else
		{
			BC.Spawn("User", 0, 0, true);
		}
		Destroy(gameObject, 0f);
	}
}
