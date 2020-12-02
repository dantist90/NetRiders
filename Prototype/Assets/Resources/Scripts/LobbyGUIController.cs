using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyGUIController : MonoBehaviour {

	public GameObject garagePanel;
	public GameObject balancerPanel;
	public GameObject shopPanel;

	public GameObject robotInfo;
	public Text health;
	public Text speed;

	public Text healthField;
	public Text speedField;

	public Toggle defWeaponToggle;
	public Toggle defRobotAbilityToggle;
	public Toggle defWeaponAbilityToggle;

	public void ShowGaragePanel()
	{
		garagePanel.SetActive(true);
		balancerPanel.SetActive(false);
		shopPanel.SetActive(false);
	}
	public void ShowBalancerPanel()
	{
		garagePanel.SetActive(false);
		balancerPanel.SetActive(true);
		shopPanel.SetActive(false);
	}
	public void ShowShopPanel()
	{
		garagePanel.SetActive(false);
		balancerPanel.SetActive(false);
		shopPanel.SetActive(true);
	}

	public void UpdateSettings(float h, float s)
	{
		health.text = h.ToString();
		speed.text = s.ToString();
	}
	public void ResetShopPanel()
	{
		healthField.text = "1";
		speedField.text = "2";
		defWeaponToggle.isOn = true;
		defRobotAbilityToggle.isOn = true;
		defWeaponAbilityToggle.isOn = true;
	}
}
