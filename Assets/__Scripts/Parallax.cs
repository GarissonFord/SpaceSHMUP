using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

	public GameObject poi; //The ship
	public GameObject[] panels; //The scrolling foregrounds
	public float scrollSpeed = -30f;

	public float motionMult = 0.25f;

	private float panelHt; //height of the panels
	private float depth; //pos.z of the panels

	void Start()
	{
		panelHt = panels [0].transform.localScale.y;
		depth = panels [0].transform.position.z;

		//initial positions
		panels[0].transform.position = new Vector3(0, 0, depth);
		panels [1].transform.position = new Vector3 (0, panelHt, depth);
	}

	void Update()
	{
		float tY, tX = 0;
		tY = Time.time * scrollSpeed % panelHt + (panelHt * 0.5f);
		if (poi != null)
		{
			tX = -poi.transform.position.x * motionMult;
		}

		panels [0].transform.position = new Vector3 (tX, tY, depth);

		if (tY >= 0) {
			panels [1].transform.position = new Vector3 (tX, tY - panelHt, depth);
		} else {
			panels [1].transform.position = new Vector3 (tX, tY + panelHt, depth);
		}
	}
}
