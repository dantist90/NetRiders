using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

	public static Settings instance;

	public string[][,] playerSettings;

	private void Awake()
	{
		if (instance == null) // Синглтон
		{
			instance = this;
			DontDestroyOnLoad(transform.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
			return;
		}

		playerSettings = new string[][,]{ // Team (0/1), number (0-3), value (0-4)
			new string[4, 6] {
				{"Spy", "Pistols", "Dash", "Grenade", "1000", "10" }, // robot, weapon, robotAbility, weaponAbility, health, speed
				{"Defender", "Shotgun", "Shield", "EMP", "1600", "9" },
				{"Storm", "Machinegun", "AutoAim", "Rocket", "1200", "8" },
				{"Sniper", "Snipergun", "Flight", "Invisibility", "800", "7" }
			},
			new string[4, 6] {
				{"Spy", "Pistols", "Dash", "Grenade", "1000", "10" },
				{"Defender", "Shotgun", "Shield", "EMP", "1600", "9" },
				{"Storm", "Machinegun", "AutoAim", "Rocket", "1200", "8" },
				{"Sniper", "Snipergun", "Flight", "Invisibility", "800", "7" }
			}
		};
	}

	public void SetUserRobot(string robot, string weapon, string robotAbility, string weaponAbility)
	{
		playerSettings[0][0,0] = robot;
		playerSettings[0][0,1] = weapon;
		playerSettings[0][0,2] = robotAbility;
		playerSettings[0][0,3] = weaponAbility;
	}
	public void SetUserRobotSetting(float h, float s)
	{
		playerSettings[0][0,4] = h.ToString();
		playerSettings[0][0,5] = s.ToString();
	}
}
