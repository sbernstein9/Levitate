using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexSc : MonoBehaviour {

	public static VortexSc instance;

	public float vortexSpeed = 0.8f;
	// Use this for initialization
	void Start () {

		instance = this;
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + vortexSpeed);
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag != "Player")
		{
			Destroy (col.gameObject);
		}
	}
}
