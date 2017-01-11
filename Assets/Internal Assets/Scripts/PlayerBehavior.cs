using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {
	public GameObject BulletPrefab;
	public Transform bulletsFolder;
	public float healingSpeed = 2f;

	private enum State {normal, onCoolDown, regenarating};
	private int bleedCooldown = 5;
	private State state = State.normal;
	private static float health = 100f;
	private float bulletForce = 500f;
	private static bool dmgCoolDown = false;
	private static bool stateUpdated = false ;

	// Use this for initialization
	void Start ()
	{
		InvokeRepeating("healing", 1, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{		
		if(Input.GetButtonDown("Fire1"))
			shoot();
		if (stateUpdated)
			switch(state)
			{
				case State.normal:

					break;

			    case State.onCoolDown:
			        InvokeRepeating("healing", healingSpeed, 0);//how to stop invokeRepeating ?
			        break;
			
			    case State.regenarating:

			        break;
			}
	}



	void changeToNormal()
	{
		//TODO change material
		state = State.normal;
	}

	void changeToOnCoolDown()
	{
		//TODO change material
		state = State.onCoolDown;
	}

	void changeToRegenerating()
	{
		//TODO change material
		state = State.regenarating;
	}

	/***************************************************************************
	 *                                                                         *
	 *                                                                         *
	 *                                                                         *
	 *                                                                         *
	 *                                                                         *
	 ***************************************************************************/

	public static void takeDamage(float dmg)
	{
		dmgCoolDown = true;


	}


	


	void healing()
	{
		if(health != 100)
			health++;
	}

	void shoot()
	{
		GameObject bullet = (GameObject) Instantiate(BulletPrefab, Camera.main.transform.position, Camera.main.transform.rotation );
		bullet.transform.parent = bulletsFolder;
		Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
		Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<CapsuleCollider>());
		bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletForce);
	}
}
