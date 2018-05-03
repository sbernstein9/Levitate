using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSc : MonoBehaviour {

	NpcStateMachineMgr NPCStateMachine;   //store a reference to the NPCStateMachineMgr
	public float speed;

	// Use this for initialization
	void Start () {
		NPCStateMachine = new NpcStateMachineMgr();
		NPCStateMachine.currentStateName = NpcStateMachineMgr.NPCStates.NONE;

		int startDir = Random.Range (0, 2);
		if (startDir == 0)
		{
			NPCStateMachine.ChangeState (new WalkLeft (this.gameObject, NPCStateMachine)); //call a new state to switch to in the beginning
			//Debug.Log("Starting left!");
		}
		else if (startDir == 1)
		{
			NPCStateMachine.ChangeState (new WalkRight (this.gameObject, NPCStateMachine)); //call a new state to switch to in the beginning
			//Debug.Log("Starting right!");
		}
		speed = Random.Range (0.1f, 0.5f);

	}
	
	// Update is called once per frame
	void Update () {

		NPCStateMachine.UpdateState();

		if (transform.position.x >= 105)
		{
			NPCStateMachine.ChangeState(new WalkLeft(this.gameObject, NPCStateMachine)); 

		}
		else if (transform.position.x <= -105)
		{
			NPCStateMachine.ChangeState(new WalkRight(this.gameObject, NPCStateMachine)); 
		}
	
	}
}
public class WalkRight : IStateMachine
{

	public GameObject npcObj;
	public NpcStateMachineMgr mgr;

	public WalkRight(GameObject g, NpcStateMachineMgr mgr)
	{
		this.npcObj = g;
		this.mgr = mgr;
	}

	public void Enter()
	{
		//Debug.Log("Walking right!");
		mgr.currentStateName = NpcStateMachineMgr.NPCStates.WalkRight;

	}

	public void Execute()
	{
		//keep walking right
		npcObj.transform.position += Vector3.right * Random.Range(0.1f,0.6f);
	}

	public void Exit()
	{
	//	Debug.Log("Ok, I'll stop walking right");
	}
}

public class WalkLeft : IStateMachine
{

	public GameObject npcObj;
	public NpcStateMachineMgr mgr;

	public WalkLeft(GameObject g, NpcStateMachineMgr mgr)
	{
		this.npcObj = g;
		this.mgr = mgr;
	}


	public void Enter()
	{
		//Debug.Log("Walking left!");
		mgr.currentStateName = NpcStateMachineMgr.NPCStates.WalkLeft;
	}

	public void Execute()
	{
		npcObj.transform.position += Vector3.left * Random.Range(0.1f,0.6f);
	}

	public void Exit()
	{
		//Debug.Log("Ok, I'll stop walking left");
	}
}
