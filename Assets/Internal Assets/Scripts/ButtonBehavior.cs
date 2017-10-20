using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class ButtonBehavior : MonoBehaviour
{
	public GameObject linkedDoor; // door to activate

	private int actualWeight = 0;
	private enum State {pushed, pulled};
	private State state = State.pulled;
	private DoorBehavior linkedDoorBehavior;
	private bool stateUpdated = true;
	private List<GameObject> collidingEnnemiesList; 
//	private Animator anim;
		


	// Use this for initialization
	void Start ()
	{
//		anim = GetComponent<Animator>();
//		anim.SetBool("buttonPushed", false);
		linkedDoorBehavior = linkedDoor.GetComponent<DoorBehavior>();
		collidingEnnemiesList = new List<GameObject>();
	}
		
	// Update every 0.02 ms call for fix operation (eg: rigidBody movement)
	void FixedUpdate()
	{
		if (stateUpdated)
		{
			stateUpdated = false;

			switch(state)
		    {
				case State.pushed:
				{
					if (actualWeight<=0)
						toggleSwitch();
					break;
				}
				
				case State.pulled:
				{
					if (actualWeight>0)
						toggleSwitch();
					break;
				}
		    }
		}
	}
	
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

            //pass the ennemy to the door
            linkedDoorBehavior.addEnnemy(ennemy);
			
			// add weight, and change stateUpdated flag to refresh state in update()
			actualWeight++;
			stateUpdated = true;
		}
	}
	
	
	
	public void removeEnnemy (GameObject ennemy)
	{
//        Debug.Log ("ButtonRemoveEnnemy");
//		if ( collidingEnnemiesList.Remove(ennemy) )
//        {   // if ennemy is present in the list, remove it
//
//            //pass the ennemy to the door
//            linkedDoorBehavior.removeEnnemy(ennemy);
//        
//			// remove weight, and change stateUpdated flag to refresh state in update()
//	        actualWeight--;
//			stateUpdated = true;
//		}
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
			transform.Translate(Vector3.back * 0.12f);
			state = State.pushed;
		}
		else
		{
//			anim.SetBool("buttonPushed", false);
			transform.Translate(Vector3.forward * 0.12f);
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