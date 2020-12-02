using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour {

	Settings SET;

	public enum ToggleTypes { Weapon, RobotAbility, WeaponAbility };
	public ToggleTypes toggleType;

	public string value;

	void Start()
	{
		SET = FindObjectOfType<Settings>();
	}

	public void ChangeSetting()
	{
		if (toggleType == ToggleTypes.Weapon)
		{
			SET.playerSettings[0][0,1] = value;
		}
		if (toggleType == ToggleTypes.RobotAbility)
		{
			SET.playerSettings[0][0,2] = value;
		}
		if (toggleType == ToggleTypes.WeaponAbility)
		{
			SET.playerSettings[0][0,3] = value;
		}
	}
}
