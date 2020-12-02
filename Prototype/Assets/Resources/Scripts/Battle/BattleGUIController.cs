using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleGUIController : MonoBehaviour {
	
	public GameObject menuPanel;
	public GameObject deadPanel;
	public GameObject snipePanel;
	
	public GameObject scoreBar;
	public GameObject battleInfo;
	public GameObject menuButton;
	public GameObject crest;
	public GameObject speakButton;
	public GameObject robotActionPanel;

	public GameObject megabotBar;
	public GameObject megabotActionPanel;

	// Абилка робота
	public Image robotAbilityButtonImage;
	public GameObject robotAbilityBar;
	public Image robotAbilityBarImage;
	public Text robotAbilityTimerText;
	public GameObject robotAbilityLogo;

	// Абилка робота
	public Image weaponAbilityButtonImage;
	public GameObject weaponAbilityBar;
	public Image weaponAbilityBarImage;
	public Text weaponAbilityTimerText;
	public GameObject weaponAbilityLogo;

	public Text ammoInfo;
	public Text hpInfo;
	public Text deadInfoText;

	public void ShowHudPanel()
	{
		BattleHud(true);
		RobotHud(true);
		MegabotHud(false);

		menuPanel.SetActive(false);
		deadPanel.SetActive(false);
		snipePanel.SetActive(false);
	}
	public void ShowMegabotPanel()
	{
		BattleHud(true);
		RobotHud(false);
		MegabotHud(true);

		menuPanel.SetActive(false);
		deadPanel.SetActive(false);
		snipePanel.SetActive(false);
	}
	public void ShowMenuPanel()
	{
		BattleHud(false);
		RobotHud(false);
		MegabotHud(false);

		menuPanel.SetActive(true);
		deadPanel.SetActive(false);
		snipePanel.SetActive(false);
	}
	public void ShowDeadPanel()
	{
		BattleHud(false);
		RobotHud(false);
		MegabotHud(false);

		menuPanel.SetActive(false);
		deadPanel.SetActive(true);
		snipePanel.SetActive(false);
	}
	public void ShowSnipePanel()
	{
		BattleHud(false);
		RobotHud(false);
		MegabotHud(false);

		menuPanel.SetActive(false);
		deadPanel.SetActive(false);
		snipePanel.SetActive(true);
	}

	void BattleHud(bool set)
	{
		scoreBar.SetActive(set);
		battleInfo.SetActive(set);
		menuButton.SetActive(set);
		crest.SetActive(set);
		speakButton.SetActive(set);
	}
	void RobotHud(bool set)
	{
		robotActionPanel.SetActive(set);
	}
	void MegabotHud(bool set)
	{
		megabotBar.SetActive(set);
		megabotActionPanel.SetActive(set);
	}
}
