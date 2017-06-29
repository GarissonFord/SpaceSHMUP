using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
	public float speed = 10f; //m/s
	public float fireRate = 0.3f; //Seconds/shot
	public float health = 10;
	public int score = 100;

	public bool ________________;

	public Bounds bounds; 
	public Vector3 boundsCenterOffset;

	void Awake()
	{
		InvokeRepeating ("CheckOffscreen", 0f, 2f);
	}

	void Update()
	{
		Move ();
	}

	public virtual void Move()
	{
		Vector3 tempPos = pos;
		tempPos.y -= speed * Time.deltaTime;
		pos = tempPos;
	}

	public Vector3 pos
	{
		get 
		{
			return (this.transform.position);
		}
		set
		{
			this.transform.position = value;
		}
	}

	void CheckOffScreen()
	{
		//If bounds are still at default value
		if (bounds.size == Vector3.zero) 
		{
			bounds = Utils.CombineBoundsOfChildren (this.gameObject);
			//Gets difference between bounds center and current position
			boundsCenterOffset = bounds.center - transform.position;
		}

		//Each call updates the bounds to the object's current position
		bounds.center = transform.position + boundsCenterOffset;
		//Checks if completely offscreen
		Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
		if (off != Vector3.zero) 
		{
			//If gone off the bottom of the screen
			if(off.y < 0)
			{
				Destroy (this.gameObject);
			}
		}
	}
}
