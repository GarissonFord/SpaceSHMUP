using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Another serializable data storage class similar to WeaponDefinition
[System.Serializable]
public class Part
{
	//To be defined in the Inspector
	public string name;
	public float health;
	public string[] protectedBy;

	//Set automatically in start
	public GameObject go;
	public Material mat;
}

public class Enemy_4 : Enemy 
{
	//This enemy starts offscreen and picks a random point to move to
	public Vector3[] points; //for the interpolation
	public float timeStart;
	public float duration = 4; //How long each movement is

	public Part[] parts; //Array of ship parts

	void Start()
	{
		points = new Vector3[2];
		//Because of Main.SpawnEnemy(), there is already an initial position 
		points [0] = pos;
		points [1] = pos;

		InitMovement ();

		//Caches the GO and Material for each part
		Transform t;
		foreach (Part prt in parts) 
		{
			t = transform.Find (prt.name);
			if (t != null) 
			{
				prt.go = t.gameObject;
				prt.mat = prt.go.GetComponent<Renderer> ().material;
			}
		}
	}

	void InitMovement()
	{
		//Pick a new point on screen
		Vector3 p1 = Vector3.zero;
		float esp = Main.S.enemySpawnPadding;
		Bounds cBounds = Utils.camBounds;
		p1.x = Random.Range (cBounds.min.x + esp, cBounds.max.x - esp);
		p1.y = Random.Range (cBounds.min.y + esp, cBounds.max.y - esp);

		points [0] = points [1]; //Shifts points[1] to points[0]
		points [1] = p1;

		timeStart = Time.time;
	}

	public override void Move()
	{
		//Completely overrides Enemy.Move() through linear interpolation

		float u = (Time.time - timeStart) / duration;
		if (u >= 1) 
		{
			InitMovement ();
			u = 0;
		}

		u = 1 - Mathf.Pow (1 - u, 2);

		pos = (1 - u) * points [0] + u * points [1];
	}

	//This overrides the OnCollisionEnter of Enemy.cs
	//Override keyword is not needed when using a common Unity function like OnCollisionEnter
	void OnCollisionEnter(Collision coll)
	{
		Debug.Log ("Collided");
		GameObject other = coll.gameObject;
		switch (other.tag) 
		{
		case "ProjectileHero":
			Projectile p = other.GetComponent<Projectile> ();
			//Check if enemy is onscreen to do damage
			bounds.center = transform.position + boundsCenterOffset;
			if (bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck (bounds, BoundsTest.offScreen) != Vector3.zero) 
			{
				Destroy (other);
				break;
			}

			//Hurts the enemy
			GameObject goHit = coll.contacts [0].thisCollider.gameObject;
			Part prtHit = FindPart (goHit);
			//If we can't find a prtHit
			if (prtHit == null) 
			{
				goHit = coll.contacts [0].otherCollider.gameObject;
				prtHit = FindPart (goHit);
			}
				
			//Checks if the part is still protected
			if (prtHit.protectedBy != null) 
			{
				foreach (string s in prtHit.protectedBy) 
				{
					//If one of the protected parts hasn't been destroyed
					if (!Destroyed (s)) 
					{
						//Then just destroy the ProjectileHero
						Destroy (other);
						return;
					}
				}
			}
				
			//If it's not protected, then it takes damage
			prtHit.health -= Main.W_DEFS [p.type].damageOnHit;
				
			//Show damage on the appropriate part
			ShowLocalizedDamage (prtHit.mat);
			if (prtHit.health <= 0) 
			{
				//Disable the damaged part if it is "destroyed"
				prtHit.go.SetActive (false);
			}
				
			//Checks if the whole ship is destroyed
			bool allDestroyed = true;

			//If a part still exists
			foreach (Part prt in parts) 
			{
				if (!Destroyed (prt))
				{
					//Then the whole thing hasn't been destroyed yet
					allDestroyed = false;
					break;
				}
			}

			if (allDestroyed)
			{
				//Tell the Main singleton it has been destroyed
				Main.S.ShipDestroyed (this);
				Destroy (this.gameObject);
			}
				
			Destroy (other); //Destroys ProjectileHero
			break;
		}
	}

	Part FindPart(string n)
	{
		foreach (Part prt in parts) 
		{
			if (prt.name == n) 
			{
				return(prt);
			}
		}
		return(null);
	}

	Part FindPart(GameObject go)
	{
		foreach (Part prt in parts) 
		{
			if (prt.go == go) 
			{
				return(prt);
			}
		}
		return (null);
	}

	//Returns true if the Part has been destroyed
	bool Destroyed(GameObject go)
	{
		return(Destroyed (FindPart (go)));
	}

	bool Destroyed(string n)
	{
		return(Destroyed (FindPart (n)));
	}

	bool Destroyed(Part prt)
	{
		//If no part was passed
		if (prt == null) 
		{
			return(true);
		}
		return (prt.health <= 0);
	}

	//Changes the color of just one part to red
	void ShowLocalizedDamage(Material m)
	{
		m.color = Color.red;
		remainingDamageFrames = showDamageForFrames;
	}
}
