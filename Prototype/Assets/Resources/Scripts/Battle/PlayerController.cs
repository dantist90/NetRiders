using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	BattleController BC;
	BattleGUIController GUI;
	public UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController FPC;

	public GameObject TPSCamera;
	public GameObject FPSCamera;
	public GameObject WeaponCamera;
	public GameObject shieldSocket;
	public GameObject weaponHolder;
	public Animator weaponHolderAnim;

	public Transform robotCanvas;
	public Text textCanvas;
	public Image fillCanvas;
	public Slider healthBar;
	Transform uCamTr;

	public enum PlayerTypes {User, Bot, Megabot};
	public PlayerTypes playerType;
	public enum RobotTypes {Spy, Defender, Storm, Sniper};
	public RobotTypes robotType;
	public enum WeaponTypes {Pistols, Shotgun, Machinegun, Snipergun};
	public WeaponTypes weaponType;

	public int teamNumber;
	public int playerNumber;

	public bool invisible = false;
	public bool immortal = false;
	public bool dead = false;
	public bool shootEnabled = true;
	public float maxHealth = 1000f;
	public float health;
	float hpProcent;

	public GameObject weapon;
	public Weapon wp;
	Rigidbody RB;

	public Ability robotAbility;
	public Ability weaponAbility;

	public cakeslice.OutlineEffect outlineEffect;
	public bool flight;

	public float speed = 8f;

	private void Start()
	{
		RB = GetComponent<Rigidbody>();
		BC = FindObjectOfType<BattleController>();
		GUI = FindObjectOfType<BattleGUIController>();
		wp = weapon.GetComponent<Weapon>();
		Debug.Log(string.Format("Spawn: {0}, {1}, {2}, {3}", playerType.ToString(), teamNumber, robotType.ToString(), weaponType.ToString()));

		health = maxHealth;
		healthBar.maxValue = maxHealth;
		UpdateHpInfo();

		if (playerType == PlayerTypes.User)
		{
			FPC = GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
			outlineEffect = FPSCamera.GetComponent<cakeslice.OutlineEffect>();
			outlineEffect.enabled = false;

			FPC.movementSettings.ForwardSpeed = speed;
			FPC.movementSettings.BackwardSpeed = speed / 2;
			FPC.movementSettings.StrafeSpeed = speed / 2;

			textCanvas.gameObject.SetActive(false);
		} else
		{
			// Canvas
			textCanvas.text = string.Format("{0}, {1}", robotType.ToString(), weaponType.ToString());
			if (BC.userTeamNumber == teamNumber)
			{
				textCanvas.color = ColorConverter.HexToColor("36B603FF");
				fillCanvas.color = ColorConverter.HexToColor("36B603FF");
			}
		}
	}
	private void Update()
	{
		if (playerType == PlayerTypes.User)
		{
			if (Input.GetKeyDown(KeyCode.LeftControl))
			{
				if (FPC.mouseLook.lockCursor)
				{
					UnlockCursor();
				}
				else
				{
					LockCursor();
				}
			}
		}
		else
		{
			if (BC.user)
			{
				float dist = Vector3.Distance(BC.user.transform.position, transform.position);
				if (BC.userWeapon.currentRange > dist) // Поворачиваем канвас бота к камере игрока
				{
					robotCanvas.gameObject.SetActive(true);
					robotCanvas.LookAt(robotCanvas.transform.position + BC.userFPSCamTr.rotation * Vector3.forward, BC.userFPSCamTr.rotation * Vector3.up);
				}
				else
				{
					robotCanvas.gameObject.SetActive(false);
				}
			}
			else
			{
				robotCanvas.gameObject.SetActive(true);
			}
		}
	}

	public void TakeDamage(float amount)
	{
		if (!immortal && !dead)
		{
			health -= amount;
			if (health <= 0f)
			{
				Die();
			}
			UpdateHpInfo();
		}
	}
	void Die()
	{
		dead = true;
		Debug.Log(string.Format("Died: {0}, {1}, {2}, {3}", playerType.ToString(), teamNumber, robotType.ToString(), weaponType.ToString()));
		if (teamNumber == 0)
		{
			BC.teamA.Remove(this.gameObject);
		}
		else
		{
			BC.teamB.Remove(this.gameObject);
		}
		BC.Spawn(playerType.ToString(), teamNumber, playerNumber, true);
		GameObject destroyEffect = Instantiate(Resources.Load("Prefabs/Effects/DestroyEffect", typeof(GameObject)), gameObject.transform.position, Quaternion.identity) as GameObject;
		destroyEffect.GetComponent<DestroyEffect>().damage = 0f;
		Destroy(gameObject, 0f);
	}

	// Невидимость
	public void SetInvisible()
	{
		invisible = true;
		foreach (GameObject gunMesh in wp.gunMeshes)
		{
			gunMesh.GetComponent<Renderer>().material.color = ColorConverter.HexToColor("5E87FF00");
		}
	}
	public void SetVisible()
	{
		invisible = false;
		foreach (GameObject gunMesh in wp.gunMeshes)
		{
			gunMesh.GetComponent<Renderer>().material.color = ColorConverter.HexToColor("FFFFFFFF");
		}
	}

	// Фриз
	public IEnumerator FlightFreeze() // Зависание в полете абилки снайпера
	{
		yield return new WaitForSeconds(5f);
		FPC.advancedSettings.airControl = false;
		Unfreeze();
	}
	public void Freeze()
	{
		RB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
	}
	public void Unfreeze()
	{
		RB.constraints = RigidbodyConstraints.None;
		RB.constraints = RigidbodyConstraints.FreezeRotation;
	}

	// Курсор
	private void LockCursor()
	{
		FPC.mouseLook.lockCursor = true;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	private void UnlockCursor()
	{
		FPC.mouseLook.lockCursor = false;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	void UpdateHpInfo()
	{
		healthBar.value = health;
		hpProcent = Mathf.Floor(health / maxHealth * 100);
		wp.hpInfo.text = string.Format("{0} %", hpProcent);
		if (playerType == PlayerTypes.User)
		{
			GUI.hpInfo.text = string.Format("Armor: {0} %", hpProcent);
		}
	}
}
