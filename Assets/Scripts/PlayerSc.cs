using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSc : MonoBehaviour {

	float step;
	public float speed;
	public bool isMoving;

	public Camera mainCam;
	public Camera feedbackCam;
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
	Color rearCamTarget;
	Color rearCamStart;
	private float rearCamStartTime;
	private float rearCamJourneyLength;
	public GameObject Vortex;

	bool colorLerp = false;

	MouseLook mainCamLook;
	public Shader highlightShader;

	// Use this for initialization
	void Start () {
		if (SceneManager.GetActiveScene().name == "Death")
		{
			feedbackCam.GetComponent<CustomFilter> ().enabled = true;
		}
		else
		{
			feedbackCam.GetComponent<CustomFilter> ().enabled = false;

		}

		camT = mainCam.transform;
		moveRay = new Ray (camT.position,camT.forward);
		mainCamLook = mainCam.GetComponent<MouseLook> ();
		//step = Time.deltaTime * 10;
		//step = Mathf.SmoothStep(0,1,);
		isMoving = false;
		rearCamStart = new Color (255, 255, 255, 0);
		rearCamTarget = new Color (255, 255, 255, 0.75f);
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
			//Debug.Log ("not casting,notnull");
			//raycastHitObject.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
			//raycastHitObject = null;
			rayHitBox = false;
		}
		else
		{
			if (raycastHitObject != null)
			{
				raycastHitObject.GetComponent<GlowObject> ().OnRaycastExit ();
			}
			rayHitBox = false;
		}
			
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

		if (colorLerp)
		{
			float distCovered = Time.time - rearCamStartTime;
			float fracJourney = distCovered / 2;
			rearCamImg.color = Color.Lerp (rearCamStart, rearCamTarget, fracJourney);
		}
	}

	void Move()
	{
		
	}

	void OnCollisionEnter(Collision col)
	{
		//Debug.Log ("Collide!");
		if (col.gameObject.tag == "Obstacle")
		{
			Debug.Log ("Hit obstacle!");
			StartCoroutine (hitObstacle());
		}

		if (col.gameObject ==  Vortex)
		{
			//Debug.Log ("Collide!");
			SceneManager.LoadScene ("Death");
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.name == "MirrorTrigger")
		{
			//Debug.Log ("MirrorTrigger!");

			Color imgColor = rearCamImg.color;

			rearCamStartTime = Time.time;
			colorLerp = true;

			Vortex.SetActive(true);
		}

		if (col.name == "RetryTrigger")
		{
			//Debug.Log ("RetryTrigger!");
			SceneManager.LoadScene ("test");

		}
		else if (col.name == "QuitTrigger")
		{
			//Debug.Log ("QuitTrigger!");
			Quit ();
		}
	}

	public void Quit()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
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