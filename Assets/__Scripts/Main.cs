using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour 
{
	static public Main S;
	static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;

	public GameObject[] prefabEnemies;
	public float enemySpawnPerSecond = 0.5f;
	public float enemySpawnPadding = 1.5f;
	public WeaponDefinition[] weaponDefinitions;
	public GameObject prefabPowerUp;
	public WeaponType[] powerUpFrequency = new WeaponType[]{
											WeaponType.blaster, 
											WeaponType.blaster, 
											WeaponType.spread, 
											WeaponType.shield };

	public bool __________________;

	public WeaponType[] activeWeaponTypes;
	public float enemySpawnRate;

	void Awake()
	{
		S = this;
		Utils.SetCameraBounds (this.GetComponent<Camera>());
		enemySpawnRate = 1f / enemySpawnPerSecond;
		Invoke ("SpawnEnemy", enemySpawnRate);

		W_DEFS = new Dictionary<WeaponType, WeaponDefinition> ();
		foreach (WeaponDefinition def in weaponDefinitions)
		{
			W_DEFS [def.type] = def;
		}
	}

	static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
	{
		//Checks for the key in the dictionary
		if (W_DEFS.ContainsKey (wt)) 
		{
			return(W_DEFS [wt]);
		}

		//Returns a definition for WeaponType.none
		return(new WeaponDefinition ());
	}

	void Start()
	{
		activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
		for (int i = 0; i < weaponDefinitions.Length; i++)
		{
			activeWeaponTypes [i] = weaponDefinitions [i].type;
		}
	}

	public void SpawnEnemy()
	{
		//Pick random prefab
		int ndx = Random.Range(0, prefabEnemies.Length);
		GameObject go = Instantiate (prefabEnemies [ndx]) as GameObject;
		Vector3 pos = Vector3.zero;
		float xMin = Utils.camBounds.min.x + enemySpawnPadding;
		float xMax = Utils.camBounds.max.x - enemySpawnPadding;
		pos.x = Random.Range (xMin, xMax);
		pos.y = Utils.camBounds.max.y + enemySpawnPadding;
		go.transform.position = pos;
		Invoke ("SpawnEnemy", enemySpawnRate);
	}

	public void DelayedRestart(float delay)
	{
		//Invokes the Restart() method in delay seconds
		Invoke ("Restart", delay);
	}

	public void Restart()
	{
		//Reloads the scene to restart the game
		Application.LoadLevel ("_Scene_0");
	}

	public void ShipDestroyed(Enemy e)
	{
		//Possible powerup drop
		if (Random.value <= e.powerUpDropChance) {

			//Choose which powerup
			int ndx = Random.Range (0, powerUpFrequency.Length);
			WeaponType puType = powerUpFrequency [ndx];

			//Spawn a PowerUp
			GameObject go = Instantiate(prefabPowerUp) as GameObject;
			PowerUp pu = go.GetComponent<PowerUp> ();
			pu.SetType (puType);

			//Assign same position as destroyed ship
			pu.transform.position = e.transform.position;
		}
	}
}
