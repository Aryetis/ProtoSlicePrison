using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {
    public GameObject bulletPrefab;
    public GameObject grenadePrefab;
	public Transform projectilesFolder;
    public TypeOfGrenade grenadeType = TypeOfGrenade.Explosive;
	public float healingSpeed = 2f;
    public float throwGrenadeAngle = 50f;
    public float throwGrenadeForce = 15f;

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
			Shoot();
        if(Input.GetButtonDown("Fire2"))
            ThrowGrenade();
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



	void ChangeToNormal()
	{
		//TODO change material
		state = State.normal;
	}

	void ChangeToOnCoolDown()
	{
		//TODO change material
		state = State.onCoolDown;
	}

	void ChangeToRegenerating()
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

	public static void TakeDamage(float dmg)
	{
		dmgCoolDown = true;


	}


	


	void Healing()
	{
		if(health != 100)
			health++;
	}

	void Shoot()
	{
		GameObject bullet = (GameObject) Instantiate(bulletPrefab, Camera.main.transform.position, Camera.main.transform.rotation );
		bullet.transform.parent = projectilesFolder;
		Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
		Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<CapsuleCollider>());
		bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletForce);
	}


    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, Camera.main.transform.position, Camera.main.transform.rotation);
        grenade.transform.parent = projectilesFolder;
        grenade.GetComponent<GrenadeBehavior>().type = TypeOfGrenade.Explosive;
        Physics.IgnoreCollision(grenade.GetComponent<Collider>(), GetComponent<Collider>());
        Physics.IgnoreCollision(grenade.GetComponent<Collider>(), GetComponent<CapsuleCollider>());
        Vector3 grenadeThrowVector = Quaternion.AngleAxis(throwGrenadeAngle, Vector3.Cross(grenade.transform.forward, Vector3.up)) * grenade.transform.forward * throwGrenadeForce;
        grenadeThrowVector.Normalize();
        grenade.GetComponent<Rigidbody>().AddForce(grenadeThrowVector*throwGrenadeForce, ForceMode.Impulse);
    }
}
