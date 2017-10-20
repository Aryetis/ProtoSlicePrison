using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorBehavior : MonoBehaviour
{
    public int activationWeight;

    private int actualWeight = 0;
	private enum State{closed, opened};
	private State state = State.closed;
    private Transform battant1, battant2;
    private List<GameObject> collidingEnnemiesList; // <=> linkedEnnemiesList
    private bool stateUpdated = true;
	

	// Use this for initialization
	void Start ()
	{
        battant1 = transform.Find("battant1");
		battant2 = transform.Find("battant2");
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
            case State.opened:
                {
                    if (actualWeight<activationWeight)
                        toggleDoor();
                    break;
                }

            case State.closed:
                {
                    if (actualWeight>=activationWeight)
                        toggleDoor();
                    break;
                }
            }
        }
    }

	public void toggleDoor()
	{
		if (state == State.opened)
		{
			battant1.Translate(Vector3.up * 1.4f);
			battant2.Translate(Vector3.down * 1.4f);
			state = State.closed;
		}
		else
		{
			battant1.Translate(Vector3.down * 1.4f);
			battant2.Translate(Vector3.up * 1.4f);
			state = State.opened;
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
//        Debug.Log ("+1 ennemy Door");
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



}

