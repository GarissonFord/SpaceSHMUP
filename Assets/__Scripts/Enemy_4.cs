using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Part{
	public string name;
	public float health;
	public string[] protectedBy;

	public GameObject go;
	public Material mat;
}

public class Enemy_4 : Enemy {

	public Vector3[] points;
	public float timeStart;
	public float duration = 4;

	public Part[] parts;

	void Start(){
		points = new Vector3[2];

		points [0] = pos;
		points [1] = pos;

		InitMovement ();

		Transform t;
		foreach (Part prt in parts) {

		}
	}

	void InitMovement(){
		Vector3 p1 = Vector3.zero;
		float esp = Main.S.enemySpawnPadding;
		Bounds cBounds = Utils.camBounds;
		p1.x = Random.Range (cBounds.min.x + esp, cBounds.max.x - esp);
		p1.y = Random.Range (cBounds.min.y + esp, cBounds.max.y - esp);

		points [0] = points [1];
		points [1] = p1;

		timeStart = Time.time;
	}

	public override void Move()
	{
		float u = (Time.time - timeStart) / duration;
		if (u >= 1) {
			InitMovement ();
			u = 0;
		}

		u = 1 - Mathf.Pow (1 - u, 2);

		pos = (1 - u) * points [0] + u * points [1];
	}
}
