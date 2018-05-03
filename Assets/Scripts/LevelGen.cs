using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour {

	public GameObject[] worldObjects;
	int cubeCounter;
	Vector3 targetForGen;
	Vector3 genStart;

	public int levelsGenerated = 0;

	public int worldWidth = 80;
	public int worldHeight = 30;
	public int worldDepth = 400;

	List<GameObject> cubes = new List<GameObject> ();
	List<GameObject> mirrors = new List<GameObject> ();
	List<GameObject> obstacles = new List<GameObject> ();



	public GameObject player;

	// Use this for initialization
	void Start () {
		cubeCounter = 0;
		for (int i = 0; i < 50; i++)
		{
			GameObject newCube;

			//newCube = Instantiate (worldObjects [0], new Vector3 (Random.Range (-worldWidth, worldWidth), Random.Range (-worldHeight, worldHeight), player.transform.position.z + Random.Range (10, 400)), Random.rotation);
			//newCube.transform.localScale = new Vector3 (Random.Range(4,7),Random.Range(4,7),Random.Range(4,7));

			targetForGen = new Vector3 (0, 0, player.transform.position.z + 200);
			//cubes.Add (newCube);
		}		

		targetForGen = new Vector3 (0, 0, player.transform.position.z + 200);
		genStart = new Vector3 (0, 0, player.transform.position.z + worldDepth);
	}
	
	// Update is called once per frame
	void Update () {

		if (Vector3.Distance(player.transform.position,targetForGen) < 70)
		{
			//Debug.Log ("GENTARGET! Player:" + player.transform.position + "Target: " + targetForGen);
			GenerateCubes ();
		}



	}

	void GenerateCubes()
	{
		for (int i = 0; i < 39; i++) //cubes
		{
			GameObject newCube;

			newCube = Instantiate (worldObjects [0], new Vector3 (Random.Range (-worldWidth, worldWidth), Random.Range (-worldHeight, worldHeight), genStart.z + Random.Range (0, worldDepth)), Random.rotation);
			newCube.transform.localScale = new Vector3 (Random.Range(2,10),Random.Range(2,10),Random.Range(2,10));
			cubes.Add (newCube);

		}

		for (int z = 0; z < 2; z++) //left mirrors
		{
			GameObject newMirror;
			Quaternion rotation = Quaternion.Euler(Random.Range(0,360),Random.Range(-155,-100),Random.Range(109,160));
			newMirror = Instantiate (worldObjects [1], new Vector3 (Random.Range (-worldWidth - 20, -worldWidth), Random.Range (-30, 30), genStart.z + Random.Range (0, worldDepth)),rotation);
			int randomScale = Random.Range (2, 7);
			newMirror.transform.localScale = new Vector3 (randomScale,randomScale,randomScale);

			mirrors.Add (newMirror);
		}
		for (int z = 0; z < 3; z++) //right mirrors
		{
			GameObject newMirror;
			Quaternion rotation = Quaternion.Euler (Random.Range (-75, -95), Random.Range (104, 40), Random.Range (-22, 42));
			newMirror = Instantiate (worldObjects [1], new Vector3 (Random.Range (worldWidth, worldWidth + 20), Random.Range (-worldHeight, worldHeight), genStart.z + Random.Range (0, worldDepth)),rotation);
			int randomScale = Random.Range (2, 7);
			newMirror.transform.localScale = new Vector3 (randomScale,randomScale,randomScale);

			mirrors.Add (newMirror);
		}


		for (int z = 0; z < 10 + levelsGenerated; z++)
		{
			GameObject newObstacle;
			newObstacle = Instantiate (worldObjects [2], new Vector3 (Random.Range (-worldWidth, worldWidth), Random.Range (-worldHeight, worldHeight), genStart.z + Random.Range (0, worldDepth)), Random.rotation);
			int randomScale = Random.Range (4, 15);
			newObstacle.transform.localScale = new Vector3 (randomScale,randomScale,randomScale);
			obstacles.Add (newObstacle);
		}
		targetForGen = new Vector3 (0, 0, targetForGen.z + 200);
		genStart = new Vector3 (0, 0, genStart.z + worldDepth);

		levelsGenerated++;

		if (levelsGenerated > 3)
		{
			
		}

		

		foreach (GameObject obj in GameObject.FindGameObjectsWithTag ("Target"))
		{
			if (obj.transform.position.z < player.transform.position.z - 800)
			{
				Destroy (obj);
			}
		}
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag ("Obstacle"))
		{
			if (obj.transform.position.z < player.transform.position.z - 800)
			{
				Destroy (obj);
			}
		}
	}
}

/*within a distance of the camera
destroy them once the camera is past them
cube prefab

*/