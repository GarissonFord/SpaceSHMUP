using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour 
{
	static public Hero		S;

	public float	speed = 30;
	public float	rollMult = -45;
	public float  	pitchMult= 30;

	[SerializeField]
	private float _shieldLevel = 1;

	public bool	_____________________;
	public Bounds bounds;

	void Awake()
	{
		S = this;
		bounds = Utils.CombineBoundsOfChildren (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");

		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		transform.position = pos;
		
		bounds.center = transform.position;
		
		// constrain to screen
		Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
		if (off != Vector3.zero) 
		{  // we need to move ship back on screen
			pos -= off;
			transform.position = pos;
		}
		
		// rotate the ship to make it feel more dynamic
		transform.rotation = Quaternion.Euler(yAxis*pitchMult, xAxis * rollMult, 0);
	}

	public GameObject lastTriggerGo = null;

	void OnTriggerEnter(Collider other)
	{
		//Find the tag of other.gameObject or its parents
		GameObject go = Utils.FindTaggedParent(other.gameObject);
		if (go != null) 
		{
			//Helps make sure the last object only damages the shield once, in case it's touching
			if (go == lastTriggerGo) 
			{
				return;
			}
			lastTriggerGo = go;

			if (go.CompareTag ("Enemy")) 
			{
				//Decrease the shield and destroy the enemy
				shieldLevel--;
				Destroy (go);
			} 
			else 
			{
				print ("Triggered: " + go.name);
			}
		} 
		else 
		{
			print ("Triggered: " + other.gameObject.name);
		}
	}

	public float shieldLevel
	{
		get
		{
			return(_shieldLevel);
		}
		set
		{
			_shieldLevel = Mathf.Min (value, 4);
			//If the shield will be less than 0
			if (value < 0) 
			{
				Destroy (this.gameObject);
			}
		}
	}
}