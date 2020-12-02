using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleController : MonoBehaviour {

	BattleGUIController GUI;
	Settings SET;

	public bool botEnabled;

	public Transform[] spawnTeamA = new Transform[4];
	public Transform[] spawnTeamB = new Transform[4];
	public List<GameObject> teamA = new List<GameObject>();
	public List<GameObject> teamB = new List<GameObject>();
	
	public int respawnTime = 3;

	public GameObject user;
	public Weapon userWeapon;
	public int userTeamNumber;
	public Transform userFPSCamTr;
	
	public Transform megabotSpawnA;
	public Transform megabotSpawnB;

	public bool megabotA;

	public int battleTime = 100;

	private void Awake()
	{
		GUI = FindObjectOfType<BattleGUIController>();
		SET = FindObjectOfType<Settings>();
		GUI.ShowHudPanel();
		
		StartCoroutine(BattleTimer());

		if (botEnabled)
		{
			CreateUser();
		}
		else
		{
			Spawn("User", 0, 0, false);
		}
	}

	IEnumerator BattleTimer()
	{
		Text timerText = GUI.battleInfo.GetComponent<Text>();

		for (int i = 0; i <= battleTime; i++)
		{
			int minRemained = (battleTime - i) / 60;
			int secRemained = (battleTime - i) - (minRemained * 60);
			if (secRemained >= 10)
			{
				timerText.text = string.Format("{0}:{1}", minRemained.ToString(), secRemained.ToString());
			}
			else
			{
				timerText.text = string.Format("{0}:0{1}", minRemained.ToString(), secRemained.ToString());
			}
			
			yield return new WaitForSeconds(1f);
			if (i >= battleTime)
			{
				SceneManager.LoadScene("Lobby");
			}
		}
	}

	public void CreateUser()
	{
		for (int tn = 0; tn < 2; tn++)
		{
			for (int pn = 0; pn < 4; pn++)
			{
				if (tn == 0 && pn == 0) // User
				{
					StartCoroutine(PlayerSpawn("User", 0, 0, false));
				}
				else
				{
					StartCoroutine(PlayerSpawn("Bot", tn, pn, false));
				}
			}
		}
	}

	public void Spawn(string playerType, int teamNumber, int playerNumber, bool delay)
	{
		StartCoroutine(PlayerSpawn(playerType, teamNumber, playerNumber, delay));
		if (playerType == "User")
		{
			GUI.ShowDeadPanel();
			StartCoroutine(RespawnTimer());
		}
	}

	IEnumerator RespawnTimer()
	{
		for (int i = 0; i <= respawnTime; i++)
		{
			int timeRemained = respawnTime - i;
			GUI.deadInfoText.text = string.Format("До респауна: {0}", timeRemained.ToString());
			yield return new WaitForSeconds(1f);
		}
	}

	public IEnumerator PlayerSpawn(string playerType, int teamNumber, int playerNumber, bool delay)
	{
		if (delay)
		{
			yield return new WaitForSeconds(respawnTime);
		}
		else
		{
			yield return new WaitForSeconds(0f);
		}

		GameObject player = null;
		PlayerController PC;
		Transform spawn;

		// Спаунпоинт
		if (teamNumber == 0)
		{
			spawn = spawnTeamA[playerNumber];
		}
		else
		{
			spawn = spawnTeamB[playerNumber];
		}

		// Создаем объект
		player = Instantiate(Resources.Load("Prefabs/" + playerType, typeof(GameObject)), spawn.position, spawn.rotation) as GameObject;
		PC = player.GetComponent<PlayerController>();

		// Тип игрока
		PC.playerNumber = playerNumber;
		if (playerType == "User")
		{
			PC.playerType = PlayerController.PlayerTypes.User;
			user = player;
			userTeamNumber = teamNumber;
			userFPSCamTr = user.GetComponent<PlayerController>().FPSCamera.transform;
			GUI.ShowHudPanel();
		}
		else
		{
			PC.playerType = PlayerController.PlayerTypes.Bot;
		}

		// Команда
		PC.teamNumber = teamNumber;
		if (teamNumber == 0) {
			teamA.Add(player);
		}
		else {
			teamB.Add(player);
		}

		// Robot Type
		if (SET.playerSettings[teamNumber][playerNumber, 0] == "Spy") { PC.robotType = PlayerController.RobotTypes.Spy; }
		if (SET.playerSettings[teamNumber][playerNumber, 0] == "Defender") { PC.robotType = PlayerController.RobotTypes.Defender; }
		if (SET.playerSettings[teamNumber][playerNumber, 0] == "Storm") { PC.robotType = PlayerController.RobotTypes.Storm; }
		if (SET.playerSettings[teamNumber][playerNumber, 0] == "Sniper") { PC.robotType = PlayerController.RobotTypes.Sniper; }

		// Weapons Type
		PC.weapon = Instantiate(Resources.Load("Prefabs/Weapons/" + SET.playerSettings[teamNumber][playerNumber, 1], typeof(GameObject)), PC.weaponHolder.transform.position, PC.weaponHolder.transform.rotation) as GameObject;
		PC.weapon.transform.SetParent(PC.weaponHolder.transform);
		PC.weapon.GetComponent<Weapon>().player = player.gameObject;

		if (SET.playerSettings[teamNumber][playerNumber, 1] == "Pistols") {
			PC.weaponType = PlayerController.WeaponTypes.Pistols;
			if (playerType == "User")
			{
				PC.weaponHolderAnim.runtimeAnimatorController = Resources.Load("Animation/Weapons/PistolsHolder") as RuntimeAnimatorController;
			}
		}
		if (SET.playerSettings[teamNumber][playerNumber, 1] == "Shotgun") {
			PC.weaponType = PlayerController.WeaponTypes.Shotgun;
			if (playerType == "User")
			{
				PC.weaponHolderAnim.runtimeAnimatorController = Resources.Load("Animation/Weapons/ShootGunHolder") as RuntimeAnimatorController;
			}
		}
		if (SET.playerSettings[teamNumber][playerNumber, 1] == "Machinegun") {
			PC.weaponType = PlayerController.WeaponTypes.Machinegun;
			if (playerType == "User")
			{
				PC.weaponHolderAnim.runtimeAnimatorController = Resources.Load("Animation/Weapons/MachineGunHolder") as RuntimeAnimatorController;
			}
		}
		if (SET.playerSettings[teamNumber][playerNumber, 1] == "Snipergun") {
			PC.weaponType = PlayerController.WeaponTypes.Snipergun;
			if (playerType == "User")
			{
				PC.weaponHolderAnim.runtimeAnimatorController = Resources.Load("Animation/Weapons/SniperGunHolder") as RuntimeAnimatorController;
			}
		}

		if (playerType == "User")
		{
			userWeapon = PC.weapon.GetComponent<Weapon>();
		}

		// Абилка робота
		PC.robotAbility = player.gameObject.AddComponent<Ability>();
		PC.robotAbility.abilityOwner = Ability.AbilityOwners.Robot;

		if (SET.playerSettings[teamNumber][playerNumber, 2] == "Dash") { PC.robotAbility.abilityType = Ability.AbilityTypes.Dash; }
		if (SET.playerSettings[teamNumber][playerNumber, 2] == "Shield") { PC.robotAbility.abilityType = Ability.AbilityTypes.Shield; }
		if (SET.playerSettings[teamNumber][playerNumber, 2] == "AutoAim") { PC.robotAbility.abilityType = Ability.AbilityTypes.AutoAim; }
		if (SET.playerSettings[teamNumber][playerNumber, 2] == "Flight") { PC.robotAbility.abilityType = Ability.AbilityTypes.Flight; }
		if (SET.playerSettings[teamNumber][playerNumber, 2] == "Grenade") { PC.robotAbility.abilityType = Ability.AbilityTypes.Grenade; }
		if (SET.playerSettings[teamNumber][playerNumber, 2] == "EMP") { PC.robotAbility.abilityType = Ability.AbilityTypes.EMP; }
		if (SET.playerSettings[teamNumber][playerNumber, 2] == "Rocket") { PC.robotAbility.abilityType = Ability.AbilityTypes.Rocket; }
		if (SET.playerSettings[teamNumber][playerNumber, 2] == "Invisibility") { PC.robotAbility.abilityType = Ability.AbilityTypes.Invisibility; }

		// Абилка пушки
		PC.weaponAbility = player.gameObject.AddComponent<Ability>();
		PC.weaponAbility.abilityOwner = Ability.AbilityOwners.Weapon;

		if (SET.playerSettings[teamNumber][playerNumber, 3] == "Dash") { PC.weaponAbility.abilityType = Ability.AbilityTypes.Dash; }
		if (SET.playerSettings[teamNumber][playerNumber, 3] == "Shield") { PC.weaponAbility.abilityType = Ability.AbilityTypes.Shield; }
		if (SET.playerSettings[teamNumber][playerNumber, 3] == "AutoAim") { PC.weaponAbility.abilityType = Ability.AbilityTypes.AutoAim; }
		if (SET.playerSettings[teamNumber][playerNumber, 3] == "Flight") { PC.weaponAbility.abilityType = Ability.AbilityTypes.Flight; }
		if (SET.playerSettings[teamNumber][playerNumber, 3] == "Grenade") { PC.weaponAbility.abilityType = Ability.AbilityTypes.Grenade; }
		if (SET.playerSettings[teamNumber][playerNumber, 3] == "EMP") { PC.weaponAbility.abilityType = Ability.AbilityTypes.EMP; }
		if (SET.playerSettings[teamNumber][playerNumber, 3] == "Rocket") { PC.weaponAbility.abilityType = Ability.AbilityTypes.Rocket; }
		if (SET.playerSettings[teamNumber][playerNumber, 3] == "Invisibility") { PC.weaponAbility.abilityType = Ability.AbilityTypes.Invisibility; }

		// Здоровье
		PC.maxHealth = float.Parse(SET.playerSettings[teamNumber][playerNumber, 4]);

		// Скорость
		PC.speed = float.Parse(SET.playerSettings[teamNumber][playerNumber, 5]);
	}

	// Кнопки
	public void GoToLobby()
	{
		//UnlockCursor();
		SceneManager.LoadScene("Lobby");
	}
	public void RestartLevel()
	{
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

	private void Update()
	{
		if (user && !megabotA && Input.GetKeyDown(KeyCode.M))
		{
			GUI.ShowMegabotPanel();
			megabotA = true;
			CreateMegabotA();
		}
	}
	void CreateMegabotA()
	{
		GameObject megabotA = Instantiate(Resources.Load("Prefabs/Megabot", typeof(GameObject)), megabotSpawnA.position, megabotSpawnA.rotation) as GameObject;
		user = megabotA;
		MegabotController MC = user.GetComponent<MegabotController>();
		userFPSCamTr = MC.FPSCamera.transform;
		MC.teamNumber = 0;
		MC.health = 5000f;

		for (int i = 0; i < teamA.Count; i++)
		{
			Destroy(teamA[i], 0);
		}
		teamA.Clear();
		teamA.Add(megabotA);
	}
}
