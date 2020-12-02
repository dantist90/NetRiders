using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour {

	BattleGUIController GUI;
	PlayerController PC;
	Rigidbody RB;
	
	public enum AbilityOwners { Robot, Weapon };
	public AbilityOwners abilityOwner;
	public enum AbilityTypes { Dash, Shield, AutoAim, Flight, Grenade, EMP, Rocket, Invisibility };
	public AbilityTypes abilityType;

	Image abilityButtonImage;
	GameObject abilityBar;
	Image abilityBarImage;
	Text abilityTimerText;
	GameObject abilityLogo;

	public bool abilityDone = true;
	public bool abilityEnabled = false;
	public int abilityReloadTime = 15;
	
	Weapon wp;
	Rigidbody missileRocket;
	Rigidbody rifGrenade;

	private void Start()
	{
		GUI = FindObjectOfType<BattleGUIController>();
		PC = GetComponent<PlayerController>();
		RB = GetComponent<Rigidbody>();
		wp = PC.weapon.GetComponent<Weapon>();

		missileRocket = Resources.Load<Rigidbody>("Prefabs/DropObjects/MissileRocket") as Rigidbody;
		rifGrenade = Resources.Load<Rigidbody>("Prefabs/DropObjects/RifGrenade") as Rigidbody;

		if (PC.playerType == PlayerController.PlayerTypes.User)
		{
			if (abilityOwner == AbilityOwners.Robot)
			{
				abilityButtonImage = GUI.robotAbilityButtonImage;
				abilityBar = GUI.robotAbilityBar;
				abilityBarImage = GUI.robotAbilityBarImage;
				abilityTimerText = GUI.robotAbilityTimerText;
				abilityLogo = GUI.robotAbilityLogo;
			}
			else
			{
				abilityButtonImage = GUI.weaponAbilityButtonImage;
				abilityBar = GUI.weaponAbilityBar;
				abilityBarImage = GUI.weaponAbilityBarImage;
				abilityTimerText = GUI.weaponAbilityTimerText;
				abilityLogo = GUI.weaponAbilityLogo;
			}

			// Подготовка
			abilityButtonImage.color = ColorConverter.HexToColor("0F1620DC");
			abilityBar.SetActive(false);
			abilityLogo.SetActive(true);
			abilityLogo.GetComponent<Image>().sprite = Resources.Load<Sprite>("GUI/Battle/Ability/" + abilityType.ToString()) as Sprite;
		}
	}

	void Update()
	{
		if (abilityDone && PC.playerType == PlayerController.PlayerTypes.User && !wp.isScoped)
		{
			if (Input.GetKeyDown("q") && abilityOwner == AbilityOwners.Robot)
			{
				AbilityStart();
			}
			if (Input.GetKeyDown("e") && abilityOwner == AbilityOwners.Weapon)
			{
				AbilityStart();
			}
		}
	}

	void AbilityStart()
	{
		abilityEnabled = true;
		abilityDone = false;
		abilityButtonImage.color = ColorConverter.HexToColor("2D4F80DC");
		if (abilityType == AbilityTypes.Dash) { StartCoroutine(Dash()); }
		if (abilityType == AbilityTypes.Shield) { StartCoroutine(Shield()); }
		if (abilityType == AbilityTypes.AutoAim) { StartCoroutine(AutoAim()); }
		if (abilityType == AbilityTypes.Flight) { StartCoroutine(Flight()); }
		if (abilityType == AbilityTypes.Grenade) { StartCoroutine(Grenade()); }
		if (abilityType == AbilityTypes.EMP) { StartCoroutine(EMP()); }
		if (abilityType == AbilityTypes.Rocket) { StartCoroutine(Rocket()); }
		if (abilityType == AbilityTypes.Invisibility) { StartCoroutine(Invisibility()); }
	}
	void AbilityEnd()
	{
		abilityEnabled = false;
		abilityButtonImage.color = ColorConverter.HexToColor("0F1620DC");
		StartCoroutine("AbilityReload");
	}
	IEnumerator AbilityReload()
	{
		abilityBar.SetActive(true);
		abilityBarImage.fillAmount = 0f;
		abilityLogo.SetActive(false);
		int timeLeft = abilityReloadTime;
		for (int i = 0; i <= abilityReloadTime; i++)
		{
			abilityBarImage.fillAmount = (float)i / abilityReloadTime;
			abilityTimerText.text = timeLeft.ToString();
			timeLeft = abilityReloadTime - i;
			yield return new WaitForSeconds(1f);
			if (i >= abilityReloadTime)
			{
				abilityDone = true;
				abilityBar.SetActive(false);
				abilityLogo.SetActive(true);
			}
		}
	}

	IEnumerator Dash()
	{
		RB.AddForce(gameObject.transform.forward * 800f, ForceMode.Impulse);
		RB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
		yield return new WaitForSeconds(2f);
		RB.constraints = RigidbodyConstraints.None;
		RB.constraints = RigidbodyConstraints.FreezeRotation;
		AbilityEnd();
	}
	IEnumerator Shield()
	{
		PC.shootEnabled = false;
		PC.shieldSocket.SetActive(true);
		PC.immortal = true;
		PC.TPSCamera.SetActive(true);
		GUI.crest.SetActive(false);
		yield return new WaitForSeconds(8f);
		PC.shootEnabled = true;
		PC.shieldSocket.SetActive(false);
		PC.immortal = false;
		PC.TPSCamera.SetActive(false);
		GUI.crest.SetActive(true);
		AbilityEnd();
	}
	IEnumerator AutoAim()
	{
		PC.outlineEffect.enabled = true;
		yield return new WaitForSeconds(8f);
		PC.outlineEffect.enabled = false;
		AbilityEnd();
	}
	IEnumerator Flight()
	{
		PC.FPC.advancedSettings.airControl = true;
		PC.FPC.m_Jump = true;
		PC.flight = true;
		yield return new WaitForSeconds(8f);
		PC.flight = false;
		AbilityEnd();
	}
	IEnumerator Grenade()
	{
		Rigidbody grenadeInstance1;
		grenadeInstance1 = Instantiate(rifGrenade, wp.abilityPoint1.position, wp.abilityPoint1.rotation) as Rigidbody;
		grenadeInstance1.AddForce(wp.abilityPoint1.forward * Random.Range(800f, 1200f));
		yield return new WaitForSeconds(0.15f);
		Rigidbody grenadeInstance2;
		grenadeInstance2 = Instantiate(rifGrenade, wp.abilityPoint2.position, wp.abilityPoint2.rotation) as Rigidbody;
		grenadeInstance2.AddForce(wp.abilityPoint2.forward * Random.Range(800f, 1200f));
		yield return new WaitForSeconds(4f);
		AbilityEnd();
	}
	IEnumerator EMP()
	{
		yield return new WaitForSeconds(8f);
		AbilityEnd();
	}
	IEnumerator Rocket()
	{
		Rigidbody rocketInstance;
		rocketInstance = Instantiate(missileRocket, wp.abilityPoint1.position, wp.abilityPoint1.rotation) as Rigidbody;
		rocketInstance.AddForce(wp.abilityPoint1.forward * 5000);
		yield return new WaitForSeconds(4f);
		AbilityEnd();
	}
	IEnumerator Invisibility()
	{
		PC.SetInvisible();
		yield return new WaitForSeconds(6f);
		PC.SetVisible();
		AbilityEnd();
	}

}
