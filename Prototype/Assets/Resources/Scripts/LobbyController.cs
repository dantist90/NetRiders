using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{

	LobbyGUIController GUI;
	Settings SET;

	public LobbyRobot[] robots;

	private void Awake()
	{
		GUI = FindObjectOfType<LobbyGUIController>();
		SET = FindObjectOfType<Settings>();

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	private void Start()
	{
		GUI.ShowGaragePanel();
		SelectDefaultRobot();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			RaycastHit hit;
			Ray ClickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(ClickRay.origin, ClickRay.direction * 100, Color.yellow);
			if (Physics.Raycast(ClickRay, out hit, 200))
			{
				GameObject hitGo = hit.transform.gameObject;
				if (hitGo.tag == "LobbyRobot")
				{
					SelectRobot(hitGo.GetComponent<LobbyRobot>());
				}
			}
		}
	}

	public void DeselectAllLobbyRobots()
	{
		for (int i = 0; i < robots.Length; i++)
		{
			robots[i].UnCheckLobbyRobot();
		}
		GUI.robotInfo.SetActive(false);
	}
	public void HideAllLobbyRobots()
	{
		for (int i = 0; i < robots.Length; i++)
		{
			robots[i].gameObject.SetActive(false);
		}
	}
	public void ShowAllLobbyRobots()
	{
		for (int i = 0; i < robots.Length; i++)
		{
			robots[i].gameObject.SetActive(true);
		}
	}
	public void SelectDefaultRobot()
	{
		SelectRobot(robots[0]);
	}

	void SelectRobot(LobbyRobot robot)
	{
		DeselectAllLobbyRobots();
		GUI.robotInfo.SetActive(true);
		robot.CheckLobbyRobot();
		SET.SetUserRobot(robot.robotType.ToString(), robot.weaponType.ToString(), robot.robotAbilityType.ToString(), robot.weaponAbilityType.ToString());
		SET.SetUserRobotSetting(robot.health, robot.speed);
		GUI.UpdateSettings(robot.health, robot.speed);
	}

	public void SaveCustomSetting()
	{
		if (GUI.healthField.text != "" && GUI.speedField.text != "")
		{
			float health = Mathf.Min(Mathf.Max(0, float.Parse(GUI.healthField.text)), 2000);
			float speed = Mathf.Min(Mathf.Max(5, float.Parse(GUI.speedField.text)), 10);
			SET.SetUserRobotSetting(health, speed);
		}
	}

	public void LoadMap1()
	{
		SceneManager.LoadScene("Map1");
	}
	public void LoadMap2()
	{
		SceneManager.LoadScene("Map2");
	}
}
