using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class EnnemyBehavior : MonoBehaviour
{
    [SerializeField] private const float speed = 0.05f;                  // move speed of the ennemy during walking phases
    [SerializeField] private const float duplicateSequenceLength = 3.0f; // time of the duplicating sequence (=> addToButtonListno movement)
    [SerializeField] private const float stunLength = 3.0f;              // Stun time caused by upper collision with the player 
    [SerializeField] private GameObject ennemyPrefab;                    // ennemyPrefab used to spawn clones
    [SerializeField] private Material walkingEnnemy;                     // materials used during walking phases
    [SerializeField] private Material stunnedEnnemy;                     // materials used during stunned phases
    [SerializeField] private Material duplicatingEnnemy;                 // materials used during duplicating phases
    [SerializeField] private Material newBorn;                           // materials used during born phases
    [SerializeField] private const float dmg = 10f;                      // damage dealt to the player by collision 
		
    public GameObject target;
    private GameObject father;                                          // father of the current ennemy <=> ennemy who gave born to him (used to cancel collisions between each others)
    private static GameObject childOf;                                  // "directory" used to store ennemies instance in the unity hierarchy
	private enum State {walking, duplicating, stunned, newBorn };
	private State state = State.walking;
	private bool stateUpdated = true;
	private Renderer renderer;
    private ButtonBehavior linkedButton;                                //button associated to ennemy
	private GameObject raycastedButton;                                 // button hit by Raycast (direction = Vector3.Down, all layers ignored except button one)
	private float maxDistanceRaycast = 10.0f;                           // maximum distance for raycasting during button detection
    private RaycastHit directDownHit;                                   // 
    private bool enteringButton=false;



	// Use this for initialization
	void Start ()
	{
        childOf = GameObject.Find("EnnemiesFolder");
        renderer = GetComponent<Renderer>();
		linkedButton = null;
		renderer = GetComponent<Renderer>();
        target = GameObject.Find("player");
    }
	
	
	
	// Update is called once per 0.2ms
	void FixedUpdate ()
	{	
        /*
         * BUTTON SWITCHING HANDLER
         */

        //TODO : BEAUTIFY IT / WHAT CAUSED IT TO NOT WORK ANYMORE ? WAS/IS IT WORKING IN THE FIRST PLACE ?
        if (linkedButton != null)
        {

            // Check that we didn't slide off the button (stacked up cube situation)
            if (linkedButton != checkButtonByRaycast () && enteringButton == false)
            {
                linkedButton.removeEnnemy (gameObject);
                linkedButton = null;
            }
            // check that we're not entering button anymore / we're now ON the button
            if (linkedButton == checkButtonByRaycast () && enteringButton == true)
                enteringButton = false;
        }



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
	


	public void ChangeToStunned()
	{
		GetComponent<Renderer>().material = stunnedEnnemy ;
		state = State.stunned;
		stateUpdated = true;
	}

	public void ChangeToWalking()
	{
		GetComponent<Renderer>().material = walkingEnnemy ;
		state = State.walking;
		stateUpdated = true;
	}
	
	public void ChangeToDuplicating()
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
			ChangeToDuplicating();

        if (collision.gameObject.CompareTag("Player"))
		{
			// Stun when collisioning with player from above
            // TODO change to raycast and measure angle to determine if above or not, should work better with slanted cubes
		    if ( collision.gameObject.transform.position.y - transform.position.y >=
		        GetComponent<Collider>().bounds.size.y && state == State.walking )
			    ChangeToStunned();
			else 
				PlayerBehavior.TakeDamage(dmg);
		}

        if (collision.gameObject.CompareTag("Ennemy"))
        {
            ButtonBehavior foo = collision.gameObject.GetComponent<EnnemyBehavior>().linkedButton;
            if (foo != null && checkButtonByRaycast () == foo)
            {
                linkedButton = foo;
                linkedButton.addEnnemy(gameObject);
            }
        }
	}
	
	
	
	void OnCollisionExit(Collision collision)
	{
        // linkedButton.removeEnnemy done in the fixedUpdate !
//        if (collision.gameObject.tag == "Button")
//            enteringButton = false;
	}
	
	
	
	void OnTriggerEnter(Collider collision)
	{
		// used to handle button's interactions (cf checkButton())
        if ( collision.gameObject.tag == "Button" )
		{   // if we're "colliding" with button and we're clearly above it (raycast sent from center of ennemy hit the button) 
		    // (update or) link linkedButton variable
            linkedButton = collision.gameObject.GetComponent<ButtonBehavior>();
            linkedButton.addEnnemy(gameObject);
            enteringButton = true;
		}
	}
	
	
	
	void OnTriggerExit(Collider collision)
	{
        // linkedButton.removeEnnemy done in the fixedUpdate !
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
        if ( ! Physics.Linecast(transform.position, target.transform.position, ~(1<<8 | 1<<9)) ) // ( ~ <=> inverse bits) ignore layer 8 and 9
			// check if there is collision with, anyLayer - Layer 8 "Ignore LineCast" and 9
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, 0, target.transform.position.z), speed);
	}


	void newBornAction()
	{
		// add repusling force
		//TODO add repulsion force (with Y axis support if stuck on XZ)
        //  sphereCast to determine direction with some random and layers 
//		transform.Translate(Vector3.forward);
//		transform.rigidbody.MovePosition()
//		transform.position = Vector3.MoveTowards(transform.position, Vector3.forward, 0.0001f);

		/*****************************************************************************************************/
		//detect collision with father
        if (!renderer.bounds.Intersects (father.GetComponent<Renderer> ().bounds))
        { // if no more collision, swtich to walking State
            //reactivate collisions
            stateUpdated = false;
            Physics.IgnoreCollision (GetComponent<Collider> (), father.GetComponent<Collider> (), false);
            ChangeToWalking ();
        }
        else
        {
            stateUpdated = true; // Ennemies still colliding => apply force and check state till there is no more collisions
            //TODO add repulsion force (with Y axis support if stuck on XZ)
            //  sphereCast to determine direction with some random and layers
        }

	}



	IEnumerator duplicateAction(float waitTime)
	{
		stateUpdated = false;

		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GameObject clone = Instantiate(ennemyPrefab) as GameObject;
		clone.GetComponent<EnnemyBehavior>().father = gameObject;
		clone.GetComponent<EnnemyBehavior>().state = State.newBorn;
		Physics.IgnoreCollision(clone.GetComponent<Collider>(), GetComponent<Collider>()); //desactivate collision between father and son BEFORE moving the position
		clone.GetComponent<Renderer>().material = newBorn;
		clone.transform.parent = childOf.transform ;
		clone.transform.position = transform.position;

		yield return new WaitForSeconds(waitTime);

		ChangeToWalking();
	}



	IEnumerator stunAction(float waitTime)
	{
		stateUpdated = false;

		yield return new WaitForSeconds(waitTime);

		ChangeToWalking();
	}



	/***************************************************************************
	 *                                                                         *
	 *                                                                         *
	 *                         AUXILIARY METHODS                               *
	 *                                                                         *
	 *                                                                         *
	 ***************************************************************************/



    private ButtonBehavior checkButtonByRaycast()
	{
		// return button under ennemy (if any) 
		// WARNING doesn't check if in collision (directly or not) 
		
		RaycastHit hitObject; // contains informations about first hit object 
		Debug.DrawRay(transform.position, Vector3.down * maxDistanceRaycast );
		if ( Physics.Raycast(transform.position, Vector3.down, out hitObject, maxDistanceRaycast, (1<<10)) )
            return hitObject.collider.gameObject.GetComponent<ButtonBehavior>();
		else
			return null;
	}

}