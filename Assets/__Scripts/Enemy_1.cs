using System.Collections;
using UnityEngine;

//Extends the Enemy class
public class Enemy_1 : Enemy 
{
	//num seconds in a full sine wave
	public float waveFrequency = 2;

	public float waveWidth = 4;
	public float waveRotY = 45;

	private float x0 = -12345;
	private float birthTime;

	void Start()
	{
		x0 = pos.x;
		birthTime = Time.time;
	}

	public override void Move()
	{
		Vector3 tempPos = pos;
		//Theta changes based on time
		float age = Time.time - birthTime;
		float theta = Mathf.PI * 2 * age / waveFrequency;
		float sin = Mathf.Sin (theta);
		tempPos.x = x0 + waveWidth * sin;
		pos = tempPos;

		Vector3 rot = new Vector3 (0, sin * waveRotY, 0);
		this.transform.rotation = Quaternion.Euler (rot);
		//Inherited method from superclass
		base.Move ();
	}
}
