using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class ButtonBehavior : MonoBehaviour
{
	public GameObject linkedDoor; // door to activate
	public int activationWeight;

	private int actualWeight = 0;
	private enum State {pushed, pulled};
	private State state = State.pulled;
	private DoorBehavior linkedDoorBehavior;
	private bool stateUpdated = true;
	private List<GameObject> collidingEnnemiesList; // <=> linkedEnnemiesList
	private Animator anim;
		


	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator>();
		anim.SetBool("buttonPushed", false);
		linkedDoorBehavior = linkedDoor.GetComponent<DoorBehavior>();
		collidingEnnemiesList = new List<GameObject>();
	}
	
	// Update is called once per frame
//	void Update ()
//	{
//		
//	}
	
	// Update every 0.02 ms call for fix operation (eg: rigidBody movement)
	void FixedUpdate()
	{
		if (stateUpdated)
		{
			stateUpdated = false;

Debug.Log("New weight ="+actualWeight);
			switch(state)
		    {
				case State.pushed:
				{
					if (actualWeight<activationWeight)
						toggleSwitch();
					break;
				}
				
				case State.pulled:
				{
					if (actualWeight>=activationWeight)
						toggleSwitch();
					break;
				}
		    }
		}
	}
	
	/***************************************************************************
	 *                                                                         *
	 *                                                                         *
	 *                   COLLISIONS MANAGEMENT METHODS                         *
	 *                                                                         *
	 *                                                                         *
	 ***************************************************************************/
	
	
//	void OnCollisionEnter(Collision col)
//	{ 
//	}
//
//
//	
//	void OnCollisionExit(Collision col)
//	{
//	}

	
	/*************************************************************************** 
	 *                                                                         *
	 *                                                                         *
	 *                  ENNEMY LIST MANAGEMENT METHODS                         *
	 *                                                                         *
	 *                                                                         *
	 ***************************************************************************/
		
	public void addEnnemy (GameObject ennemy)
	{		
		if ( ! collidingEnnemiesList.Contains(ennemy) )
		{   // if ennemy is not already listed
		
			//add ennemy to the list 
			collidingEnnemiesList.Add(ennemy);
			
			// add weight, and change stateUpdated flag to refresh state in update()
			actualWeight++;
			stateUpdated = true;
		}
	}
	
	
	
	public void removeEnnemy (GameObject ennemy)
	{
		if ( collidingEnnemiesList.Remove(ennemy) )
        {   // if ennemy is present in the list, remove it
        
			// remove weight, and change stateUpdated flag to refresh state in update()
	        actualWeight--;
			stateUpdated = true;
		}
	}
	
	
	
	/***************************************************************************
	 *                                                                         *
	 *                                                                         *
	 *                       DOORS MANAGEMENT METHODS                          *
	 *                                                                         *
	 *                                                                         *
	 ***************************************************************************/

	void toggleSwitch()
	{
		if (state == State.pulled)
		{
//			anim.SetBool("buttonPushed", true);
			transform.Translate(Vector3.back * 0.2f);
			linkedDoorBehavior.toggleDoor();
			state = State.pushed;
		}
		else
		{
//			anim.SetBool("buttonPushed", false);
			transform.Translate(Vector3.forward * 0.2f);
			linkedDoorBehavior.toggleDoor();
			state = State.pulled;
		}
	}
	
	
	
	/***************************************************************************
	 *                                                                         *
	 *                                                                         *
	 *                     AUXILIARY MANAGEMENT METHODS                        *
	 *                                                                         *
	 *                                                                         *
	 ***************************************************************************/


}