using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class EnnemyBehavior : MonoBehaviour
{
	public const float speed = 0.05f; // move speed of the ennemy during walking phases
	public const float duplicateSequenceLength = 3.0f; // time of the duplicating sequence (=> addToButtonListno movement)
	public const float stunLength = 3.0f; // Stun time caused by upper collision with the player 
	public GameObject ennemyPrefab; // ennemyPrefab used to spawn clones
	public Material walkingEnnemy; // materials used during walking phases
	public Material stunnedEnnemy; // materials used during stunned phases
	public Material duplicatingEnnemy; // materials used during duplicating phases
	public Material newBorn; // materials used during born phases
	public const float dmg = 10f; // damage dealt to the player by collision 
		
	private GameObject father; // father of the current ennemy <=> ennemy who gave born to him (used to cancel collisions between each others)
	private static GameObject childOf; // "directory" used to store ennemies instance in the unity hierarchy
	private enum State {walking, duplicating, stunned, newBorn };
	private State state = State.walking;
	private bool stateUpdated = true;
	private Renderer renderer;
	private GameObject linkedButton; //button associated to ennemy
	private GameObject raycastedButton; // button hit by Raycast (direction = Vector3.Down, all layers ignored except button one)
	private List<GameObject> upCollidingEnnemies;
	private List<GameObject> downCollidingEnnemies;
	private float maxDistanceRaycast = 10.0f; //maximum distance for raycasting during button detection





	// Use this for initialization
	void Start ()
	{
		linkedButton = null;
		renderer = GetComponent<Renderer>();
		childOf = GameObject.Find("EnnemiesFolder");
		upCollidingEnnemies = new List<GameObject>();
		downCollidingEnnemies = new List<GameObject>();
	}
	
	
	
	// Update is called once per frame
	void FixedUpdate ()
	{	
		/*
		 * STATE HANDLER
		 */
		if (stateUpdated) 
			switch(state)
			{
				case State.duplicating:
			        StartCoroutine(duplicateAction(duplicateSequenceLength));
					break;

				case State.walking:
			        walkingAction();
					break;

				case State.stunned:
			        StartCoroutine(stunAction(stunLength));
					break;

				case State.newBorn:
			        newBornAction();
					break;
			}
	}
	
	/***************************************************************************
	 *                                                                         *
	 *                                                                         *
	 *                           STATE METHODS                                 *
	 *                                                                         *
	 *                                                                         *
	 ***************************************************************************/
	


	public void changeToStunned()
	{
		GetComponent<Renderer>().material = stunnedEnnemy ;
		state = State.stunned;
		stateUpdated = true;
	}

	public void changeToWalking()
	{
		GetComponent<Renderer>().material = walkingEnnemy ;
		state = State.walking;
		stateUpdated = true;
	}
	
	public void changeToDuplicating()
	{
		GetComponent<Renderer>().material = duplicatingEnnemy ;
		state = State.duplicating;
		stateUpdated = true;
	}

	/***************************************************************************
	 *                                                                         *
	 *                                                                         *
	 *                         COLLISIONS METHODS                              *
	 *                                                                         *
	 *                                                                         *
	 ***************************************************************************/

	void OnCollisionEnter(Collision collision)
	{
		if ( collision.gameObject.tag == "Bullet" && state == State.walking )
			//Duplicate on collision with bullets 
			changeToDuplicating();

		if (collision.gameObject.name == "player")
		{
			// Stun when collisioning with player from above
		    if ( collision.gameObject.transform.position.y - this.transform.position.y >=
		        this.GetComponent<Collider>().bounds.size.y && state == State.walking )
			    changeToStunned();
			else 
				PlayerBehavior.takeDamage(dmg);
		}
		
		// used to handle button's interactions ( cf checkButton() )	
		if ( collision.gameObject.tag == "Ennemy")
		{	// if collision.gameObject is colliding below or above our ennemy instance
			//TODO y detect value may need some tweaking
			if (collision.contacts[0].normal.y <= 0.5)
			{	// collision.gameObject <=> ennemy on top
				// add to list of up hits 
				if ( !upCollidingEnnemies.Contains(collision.gameObject) )
					upCollidingEnnemies.Add(collision.gameObject);
			}
			if (collision.contacts[0].normal.y >= -0.5)
			{
				// link-up button
				addToButtonList(collision.gameObject, null, null); //no need to call addToButtonList because collision work both ways <=> 1 collision = 2 events
				// add to list of down hits
				if ( !downCollidingEnnemies.Contains(collision.gameObject) )
					downCollidingEnnemies.Add(collision.gameObject);
			}
        }
	}
	
	
	
	void OnCollisionExit(Collision collision)
	{
		// used to handle button's interactions (cf checkButton())
		if ( collision.gameObject.tag == "Ennemy" )
		{
			removeFromButtonList();
			//TODO handle collidingList
		}
	}
	
	
	
	void OnTriggerEnter(Collider collision)
	{
		// used to handle button's interactions (cf checkButton())
		if ( collision.gameObject.tag == "Button" && checkButtonByRaycast() == collision.gameObject )
		{   // if we're "colliding" with button and we're clearly above it (raycast sent from center of ennemy hit the button) 
		    // (update or) link linkedButton variable
				addToButtonList(null, null, collision.gameObject);
		}
	}
	
	
	
	void OnTriggerExit(Collider collision)
	{
		// used to handle button's interactions (cf checkButton())

		if ( collision.gameObject.tag == "Button" && linkedButton == collision.gameObject)
		// KNOWN PROBLEM, if a ennemy slide from button to the top of an ennemy
		// the cube will move from the "directCollision state" to "stacked" state without us detecting it
		// eg:
		//                                  [A]                                               [A]
		//           [A][B]           =>       [B]                                     =>     [B]
		//       -----Button-----         -----Button-----                               -----Button-----
		//
		//     collision detected         cube stacking up                                 cube stacked but [A] is not 
		//	but refused by the normal   but no event triggered		                       considered as such...
		//                              => cube leaved button <=> extracted from list
		//								BUT not detected as stacked because the normal
		//                              was wrong in precedent stage, and no new 
		//                              collision event has been sent
		//
		//  =====> NO KNOWN FIX but not a problem because collisions are jittery 
		//         and such a smooth transition is theorically impossible with our ennemy model
		{
			removeFromButtonList();
			//TODO handle collidingList
		}
	}
	
	
	
	/***************************************************************************
	 *                                                                         *
	 *                                                                         *
	 *                           ACTION METHODS                                *
	 *                                                                         *
	 *                                                                         *
	 ***************************************************************************/
	


	void walkingAction()
	{
		Vector3 playerPos = GameObject.Find("player").transform.position;

		if ( ! Physics.Linecast(transform.position, playerPos, ~(1<<8 | 1<<9)) ) // ( ~ <=> inverse bits) ignore layer 8 and 9
			// check if there is collision with, anyLayer - Layer 8 "Ignore LineCast"
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerPos.x, 0, playerPos.z), speed);
	}


	void newBornAction()
	{
//		stateUpdated = false;

		// add repusling force
		//TODO add repulsion force (with Y axis support if stuck on XZ)
//		transform.Translate(Vector3.forward);
//		transform.rigidbody.MovePosition()
//		transform.position = Vector3.MoveTowards(transform.position, Vector3.forward, 0.0001f);

		/*****************************************************************************************************/
		//detect collision with father
		if( ! renderer.bounds.Intersects(father.GetComponent<Renderer>().bounds) ) 
		{ // if no more collision, swtich to walking State
			//reactivate collisions
			Physics.IgnoreCollision(this.GetComponent<Collider>(), father.GetComponent<Collider>(), false);

			changeToWalking();
		}
	}



	IEnumerator duplicateAction(float waitTime)
	{
		stateUpdated = false;

		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GameObject clone = Instantiate(ennemyPrefab) as GameObject;
		clone.GetComponent<EnnemyBehavior>().father = this.gameObject;
		clone.GetComponent<EnnemyBehavior>().state = State.newBorn;
		Physics.IgnoreCollision(clone.GetComponent<Collider>(), this.GetComponent<Collider>()); //desactivate collision between father and son BEFORE moving the position
		clone.GetComponent<Renderer>().material = newBorn;
		clone.transform.parent = childOf.transform ;
		clone.transform.position = this.transform.position;

		yield return new WaitForSeconds(waitTime);

		changeToWalking();
	}



	IEnumerator stunAction(float waitTime)
	{
		stateUpdated = false;

		yield return new WaitForSeconds(waitTime);

		changeToWalking();
	}



	/***************************************************************************
	 *                                                                         *
	 *                                                                         *
	 *                         AUXILIARY METHODS                               *
	 *                                                                         *
	 *                                                                         *
	 ***************************************************************************/



	private GameObject checkButtonByRaycast()
	{
		// return button under ennemy (if any) 
		// WARNING doesn't check if in collision (directly or not) 
		
		RaycastHit hitObject; // contains informations about first hit object 
		Debug.DrawRay(transform.position, Vector3.down * maxDistanceRaycast );
		if ( Physics.Raycast(transform.position, Vector3.down, out hitObject, maxDistanceRaycast, (1<<10)) )
			return hitObject.collider.gameObject;
		else
			return null;
	}



	private void removeFromButtonList()
	{
		/*
		 * Function handling the linking between a button and one ennemy instance and its children,
		 * remove the ennemy<->button link and update internal button's list 
		 * WARNING DO NOT HANDLE bot/UpEnnemyCollidedBehavior, and those must be updated AFTER the call of addToButtonList
		 * this functions seperate the work in 3 different tasks:
		 * _ handling indirect collisions (eg : ennemies stacking on each others)
		 * _ refreshing children of an ennemy (used by recursive calls)
		 * _ direct ennemy<->button collision
		 *
		 * botEnnemy <=> ennemy colliding with the bottom of our current ennemy instance 
		 * refreshedLinkedBUtton <=> new value of linkedButton, used when doing recursive calls over ennemies
		 * directButton <=> button colliding with current ennemy instance
		 */

		 //TODO



	}


	private void addToButtonList(GameObject botEnnemy, GameObject refreshedLinkedButton, GameObject directButton)
	{
		/*
		 * Function handling the linking between a button and one ennemy instance and its children,
		 * adding the ennemy<->button link and update internal button's list 
		 * WARNING DO NOT HANDLE bot/UpEnnemyCollidedBehavior, and those must be updated AFTER the call of addToButtonList
		 * this functions seperate the work in 3 different tasks:
		 * _ handling indirect collisions (eg : ennemies stacking on each others)
		 * _ refreshing children of an ennemy (used by recursive calls)
		 * _ direct ennemy<->button collision
		 *
		 * botEnnemy <=> ennemy colliding with the bottom of our current ennemy instance 
		 * refreshedLinkedBUtton <=> new value of linkedButton, used when doing recursive calls over ennemies
		 * directButton <=> button colliding with current ennemy instance
		 /


		/*
		 * HANDLE INDIRECT COLLISIONS (collisions between ennemies, possibly ennemies stacking on button)
		 */
		if ( botEnnemy != null )
		{
			EnnemyBehavior botEnnemyCollidedBehavior = botEnnemy.GetComponent<EnnemyBehavior>();

			if ( botEnnemyCollidedBehavior.linkedButton != null && 
				// if the ennemy we're colliding with is linked to a button
				! downCollidingEnnemies.Contains(botEnnemy) &&
				// if the ennemy we're colliding is not already in our downCollidingEnnemies list
				botEnnemyCollidedBehavior.linkedButton != linkedButton && 
				// if the button the colliding ennemy is different from our 
				botEnnemyCollidedBehavior.linkedButton == checkButtonByRaycast()
				// if the ennemy instance is confirmed to be over the right button by racyasting
				)
				// if (all of the above) => we need to update our linkedButton and all "ennemy sons" 's button 
			{
				linkedButton = botEnnemyCollidedBehavior.linkedButton;
				linkedButton.GetComponent<ButtonBehavior>().addEnnemy(gameObject);

				// recursive calls to update this instance sons <=> ennemies stacked on top that need to get their linkedButton refreshed
				foreach (GameObject ennemy in upCollidingEnnemies)
					ennemy.GetComponent<EnnemyBehavior>().addToButtonList(null, linkedButton, null);
				return;
			}
		}


		/*
		 * HANDLE SONS REFRESHING
		 */
		if ( refreshedLinkedButton != null &&
			// if the function has been called out to refresh a linkedButton
			linkedButton != refreshedLinkedButton &&
			// if the linkedButton is not already corresponding to the new value &&
			refreshedLinkedButton == checkButtonByRaycast()
			// if the ennemy instance is confirmed to be over the button by racyasting
		   )
		{
			linkedButton = refreshedLinkedButton;
			linkedButton.GetComponent<ButtonBehavior>().addEnnemy(gameObject);
			foreach (GameObject ennemy in upCollidingEnnemies)
				ennemy.GetComponent<EnnemyBehavior>().addToButtonList(null, linkedButton, null);
			return;
		}
		
		
		/*
		 * HANDLE DIRECT BUTTON COLLISION ASSOCIATION
		 */
		if (directButton != null)
		{
			linkedButton = directButton;
			linkedButton.GetComponent<ButtonBehavior>().addEnnemy(gameObject);
			foreach (GameObject ennemy in upCollidingEnnemies)
				ennemy.GetComponent<EnnemyBehavior>().addToButtonList(null, linkedButton, null);
			return;
		}
	}



}