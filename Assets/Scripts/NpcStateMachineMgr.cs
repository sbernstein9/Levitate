using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IStateMachine
{
	void Enter();

	void Exit();

	void Execute();

}

public class NpcStateMachineMgr : MonoBehaviour {

	public enum NPCStates
	{
		WalkLeft,
		WalkRight,
		NONE
	};

	public IStateMachine currentState; //keep track of current state
	public NPCStates currentStateName = NPCStates.NONE;

	public void ChangeState(IStateMachine newState)
	{



		if (currentState != null)
		{
			currentState.Exit(); //exit the old state & run whatever functionality is in the old state's exit
		}


		// enter the new state
		currentState = newState;
		currentState.Enter();


	}

	public void UpdateState()
	{
		if (currentState != null)
			currentState.Execute();
	}


}
