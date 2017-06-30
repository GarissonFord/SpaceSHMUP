using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
	//x holds a min value and y a max for a Random.Range() layer on
	public Vector2 rotMinMax = new Vector2(15, 90);
	public Vector2 driftMinMax = new Vector2(0.25f, 2);
	public float lifeTime = 6f; //Span of a power up's life
	public float fadeTime = 4f; //Seconds until it fades
	public bool _______________________;
	public WeaponType type;
	public GameObject cube; //child object
	public TextMesh letter;
	public Vector3 rotPerSecond;
	public float birthTime;

	void Awake()
	{
		//Seeks out the cube
		cube = transform.Find ("Cube").gameObject;
		letter = GetComponent<TextMesh> ();

		//Randomized velocity
		Vector3 vel = Random.onUnitSphere; //Random XYZ velocity
		vel.z = 0;
		vel.Normalize (); //Make length of vel 1
		vel *= Random.Range(driftMinMax.x, driftMinMax.y);
		GetComponent<Rigidbody> ().velocity = vel;

		//Quaternion.identity is equal to no rotation
		transform.rotation = Quaternion.identity;

		rotPerSecond = new Vector3 (Random.Range (rotMinMax.x, rotMinMax.y),
			Random.Range (rotMinMax.x, rotMinMax.y),
			Random.Range (rotMinMax.x, rotMinMax.y));

		//CheckOffScreen every 2 seconds
		InvokeRepeating("CheckOffScreen", 2f, 2f);

		birthTime = Time.time;
	}

	void Update()
	{
		//Rotate the cube every Update
		// *Time.time makes the rotation time based
		cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

		//Allows the powerup to fade over time
		float u = (Time.time - (birthTime + lifeTime)) / fadeTime;

		if (u >= 1) {
			Destroy (this.gameObject);
			return;
		}

		if (u > 0) {
			Color c = cube.GetComponent<Renderer> ().material.color;
			c.a = 1f - u;
			cube.GetComponent<Renderer> ().material.color = c;
			//Fades the letter
			c = letter.color;
			c.a = 1f - (u * 0.5f);
			letter.color = c;
		}
	}

	public void SetType(WeaponType wt)
	{
		WeaponDefinition def = Main.GetWeaponDefinition (wt);
		cube.GetComponent<Renderer> ().material.color = def.color;
		letter.text = def.letter;
		type = wt;
	}

	public void AbsorbedBy(GameObject target)
	{
		//Called when the Hero class picks up the power up 
		Destroy(this.gameObject);
	}

	void CheckOffScreen()
	{
		if (Utils.ScreenBoundsCheck (cube.GetComponent<Collider>().bounds, BoundsTest.offScreen) != Vector3.zero) 
		{
			Destroy (this.gameObject);
		}
	}
}
