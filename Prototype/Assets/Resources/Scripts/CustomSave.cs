using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomSave : MonoBehaviour {

	Settings SET;
	LobbyGUIController GUI;

	public Button saveButton;
	public GameObject saveErrText;

	void Start()
	{
		GUI = FindObjectOfType<LobbyGUIController>();
		SET = FindObjectOfType<Settings>();
	}

	void Update () {
		if (SET.playerSettings[0][0,2] == SET.playerSettings[0][0,3] || GUI.healthField.text == "" || GUI.speedField.text == "")
		{
			saveButton.interactable = false;
			saveErrText.SetActive(true);
			saveErrText.GetComponent<Text>().text = "Укажите значения здоровья и скорости, выберите разные абилки.";
		}
		else
		{
			saveButton.interactable = true;
			saveErrText.SetActive(false);
		}
	}
}
