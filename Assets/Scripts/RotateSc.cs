using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateSc : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, 13 * Time.deltaTime);
		transform.Rotate(Vector3.right, 7 * Time.deltaTime);

		if (Input.GetKeyDown(KeyCode.Return))
		{
			SceneManager.LoadScene ("test");
		}
	}
}
