using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyRobot : MonoBehaviour {
	
	public GameObject checkMark;
	public Text robotName;
	public float health;
	public float speed;

	public enum RobotTypes { Spy, Defender, Storm, Sniper };
	public RobotTypes robotType;
	public enum WeaponTypes { Pistols, Shotgun, Machinegun, Snipergun };
	public WeaponTypes weaponType;
	public enum RobotAbilityTypes { Dash, Shield, AutoAim, Flight, Grenade, EMP, Rocket, Invisibility };
	public RobotAbilityTypes robotAbilityType;
	public enum WeaponAbilityTypes { Dash, Shield, AutoAim, Flight, Grenade, EMP, Rocket, Invisibility };
	public WeaponAbilityTypes weaponAbilityType;

	public void CheckLobbyRobot()
	{
		checkMark.SetActive(true);
		robotName.color = ColorConverter.HexToColor("FF8E00FF");
	}
	public void UnCheckLobbyRobot()
	{
		checkMark.SetActive(false);
		robotName.color = ColorConverter.HexToColor("FFFFFFFF");
	}
}
