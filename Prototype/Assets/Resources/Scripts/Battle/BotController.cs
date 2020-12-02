using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotController : MonoBehaviour {

	UnityEngine.AI.NavMeshAgent agent;
	UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter character;
	public Transform target;
	public float targetSearchRate = 2f;

	BattleController BC;
	PlayerController PC;

	RaycastHit hit;

	private void Awake()
	{
		BC = FindObjectOfType<BattleController>();
		PC = GetComponent<PlayerController>();
		agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
		character = GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter>();
		agent.updateRotation = false;
		agent.updatePosition = true;
	}

	private void Start()
	{
		SearchTarget();
	}


	private void Update()
	{
		if (target != null)
		{
			agent.SetDestination(target.position);

			float distance = Vector3.Distance(transform.position, target.position);
			Weapon weapon = PC.wp;

			// Стопаем
			if (target && distance < weapon.currentRange / 2)
			{
				agent.Stop();
			}
			else
			{
				agent.Resume();
			}

			// Атакуем
			if (distance < weapon.currentRange && Time.time >= weapon.nextTimeToFire && !weapon.magEmpty)
			{
				// Поворот за целью
				Vector3 targetRot = new Vector3(target.position.x, this.transform.position.y, target.position.z);
				transform.LookAt(targetRot);

				if (weapon.magazine != 0)
				{
					weapon.nextTimeToFire = Time.time + weapon.fireRate;
					weapon.magazine--;
					weapon.shootEffect1.Play();
					if (target.tag == "Player")
					{
						target.GetComponent<PlayerController>().TakeDamage(weapon.damage / 3);
					}
					if (target.tag == "Megabot")
					{
						target.GetComponent<MegabotController>().TakeDamage(weapon.damage / 3);
					}

					if (weapon.magazine == 0)
					{
						weapon.magEmpty = true;
						weapon.StartCoroutine("Reload");
					}
				}
				else
				{
					weapon.magEmpty = true;
					weapon.StartCoroutine("Reload");
				}
			}
		}

		if (agent.remainingDistance > agent.stoppingDistance)
			character.Move(agent.desiredVelocity, false, false);
		else
			character.Move(Vector3.zero, false, false);

		// Ищем цель
		float nextTimeToSearch = 0f;
		if (Time.time >= nextTimeToSearch)
		{
			nextTimeToSearch = Time.time + targetSearchRate;
			SearchTarget();
		}
	}
	void SearchTarget()
	{
		if (PC.teamNumber == 0 && BC.teamB.Count > 0)
		{
			float bestDist = 0f;
			int bestTarget = 0;
			for (int i = 0; i < BC.teamB.Count; i++)
			{
				float dist = Vector3.Distance(transform.position, BC.teamB[i].transform.position);
				if (i == 0)
				{
					bestDist = dist;
					bestTarget = i;
				}
				else
				{
					if (dist < bestDist)
					{
						bestDist = dist;
						bestTarget = i;
					}
				}
			}
			SetTarget(BC.teamB[bestTarget].transform);
		}
		if (PC.teamNumber == 1 && BC.teamA.Count > 0)
		{
			float bestDist = 0f;
			int bestTarget = 0;
			for (int i = 0; i < BC.teamA.Count; i++)
			{
				float dist = Vector3.Distance(transform.position, BC.teamA[i].transform.position);
				if (i == 0)
				{
					bestDist = dist;
					bestTarget = i;
				}
				else
				{
					if (dist < bestDist)
					{
						bestDist = dist;
						bestTarget = i;
					}
				}
			}
			SetTarget(BC.teamA[bestTarget].transform);
		}
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}
}
