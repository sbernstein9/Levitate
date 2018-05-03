using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSc : MonoBehaviour {

	float step;
	public float speed;
	public bool isMoving;

	public Camera mainCam;
	Transform camT;
	Vector3 startPos;
	Vector3 centerPos;
	Vector3 targetPos;
	Vector3 startRelCenter;
	Vector3 targetRelCenter;
	float startTime;
	public float journeyTime = 1;
	RaycastHit moveHit;
	Ray moveRay;
	public int rayDist = 100;
	GameObject raycastHitObject;
	public RawImage rearCamImg;

	public GameObject Vortex;

	MouseLook mainCamLook;
	public Shader highlightShader;

	// Use this for initialization
	void Start () {
		camT = mainCam.transform;
		moveRay = new Ray (camT.position,camT.forward);
		mainCamLook = mainCam.GetComponent<MouseLook> ();
		//step = Time.deltaTime * 10;
		//step = Mathf.SmoothStep(0,1,);
		isMoving = false;
	}
	
	// Update is called once per frame
	void Update () {

		bool raycastHasHit = false;
		bool rayHitBox = false;
		//step = Time.deltaTime * Time.deltaTime * (3*speed - (2*speed*Time.deltaTime));
		Debug.DrawRay (camT.position, camT.forward*50,Color.yellow);


		if (Physics.Raycast (camT.position, camT.forward*50, out moveHit, rayDist)) //&& Vector3.distance(startPos,moveHit.transform.position) < maxDist
		{
			if (moveHit.transform.gameObject.tag == "Target")
			{

				rayHitBox = true;
				raycastHitObject = moveHit.transform.gameObject;

				//raycastHitObject.GetComponent<Renderer>().material.shader = highlightShader;

				raycastHitObject.GetComponent<GlowObject> ().OnRaycastEnter ();



				if (Input.GetKeyDown (KeyCode.Space))
				{
					startPos = transform.position;
					
					//Debug.Log ("rayHit: " + moveHit.transform.name);
					targetPos = moveHit.transform.position;
					centerPos = (startPos + targetPos) * 0.5f;
					centerPos -= new Vector3 (0, 100f, 0);
					startRelCenter = startPos - centerPos;
					startTime = Time.time;
					targetRelCenter = targetPos - centerPos;
					isMoving = true;
				}
			}
			else 
			{
				//rayHitBox = false;
			}
		}
		else if (rayHitBox == true)
		{
			raycastHitObject.GetComponent<GlowObject> ().OnRaycastExit();
			Debug.Log ("not casting,notnull");
			//raycastHitObject.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
			//raycastHitObject = null;
			rayHitBox = false;
		}
		else
		{
			//Debug.Log ("nothit");
			if (raycastHitObject != null)
			{
				raycastHitObject.GetComponent<GlowObject> ().OnRaycastExit ();
			}
			rayHitBox = false;
		}
			
		//Debug.Log ("Raycastobj: " + raycastHitObject);
		//Debug.Log (rayHitBox);

		/*else if (Input.GetKeyUp(KeyCode.Space))
		{
			isMoving = false;
		}*/

		if (isMoving)
		{
			//float fracComplete = (Time.time - startTime) / journeyTime;
			float fracComplete = (Time.time - startTime) * speed;
			//step = Time.deltaTime * 10;

			transform.position = Vector3.Slerp (startRelCenter, targetRelCenter, fracComplete);
			//Debug.Log ("isMoving: " + startPos + targetPos + fracComplete);
			transform.position += centerPos;
			centerPos.x += Input.GetAxis ("Horizontal") * 0.24f;
			centerPos.y += Input.GetAxis ("Vertical") * 0.24f;


			if (Vector3.Distance (transform.position, targetPos) <= (raycastHitObject.transform.localScale.x+raycastHitObject.transform.localScale.y+raycastHitObject.transform.localScale.z)/3 + 5 || fracComplete > .8f)
			{
				//transform.position = targetPos;
				isMoving = false;
			}
		}
		else
		{
			//transform.position += new Vector3 (transform.position.x * Input.GetAxis ("Horizontal") * 0.2f, transform.position.y * Input.GetAxis("Vertical") * 0.2f, 0);
		}
	}

	void Move()
	{
		
	}

	void OnCollisionEnter(Collision col)
	{
		Debug.Log ("Collide!");
		if (col.gameObject.tag == "Obstacle")
		{
			Debug.Log ("Hit obstacle!");
			StartCoroutine (hitObstacle());
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.name == "MirrorTrigger")
		{
			Debug.Log ("MirrorTrigger!");

			Color imgColor = rearCamImg.color;
			//Color tempColor = new Color (0,0,0,0);
			for (float i = 0; i < 190; i+= 0.002f)
			{
				rearCamImg.color = new Color(imgColor.r,imgColor.g,imgColor.b,i);
				Debug.Log ("i: " + i + " imgColor: " + rearCamImg.color);
			}

			Vortex.GetComponent<VortexSc>().enabled = true;

		}
	}

	public IEnumerator hitObstacle()
	{
		float duration = 1;
		Quaternion StartRotation = transform.rotation;
		float t = 0f;
		mainCamLook.enabled = false;
		while (t<duration)
		{
			transform.rotation = StartRotation * Quaternion.AngleAxis(t / duration * 720f, Vector3.up);
			yield return null;
			t += Time.deltaTime;
		}
		transform.rotation = StartRotation;
		yield return new WaitForSeconds(1);
		mainCamLook.enabled = true;


	}
}