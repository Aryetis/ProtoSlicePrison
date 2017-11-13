using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum TypeOfGrenade {Decoy, Explosive};

public class GrenadeBehavior : MonoBehaviour
{
    public TypeOfGrenade type = TypeOfGrenade.Explosive;
    [SerializeField] AudioClip detonationSound;
    [SerializeField] private float timeToDetonate = 6.0f;
    [SerializeField] private float detonationRadius = 10.0f;
    [SerializeField] private float maxDetonationForce = 1000.0f;
    [SerializeField] private float decoyRadius = 50.0f;

    private SphereCollider decoySphereCollider; // Not in editor as we have to set its radius according to decoyRadius
                                                // TODO beautify it with onGui()

	// Use this for initialization
	void Start ()
    {
        decoySphereCollider = gameObject.AddComponent<SphereCollider>() as SphereCollider;
        decoySphereCollider.isTrigger = true;
        decoySphereCollider.radius = decoyRadius;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Explode ?
        if(timeToDetonate <= 0)
            Explode();
        else
            timeToDetonate -= Time.deltaTime;

        // Decoy ?
        if(type == TypeOfGrenade.Decoy)
        {
            
        }
	}

    private void Explode()
    {
        // Explode
        if(type == TypeOfGrenade.Explosive)
        {
            Vector3 explosionVector;
            float explosionForce;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detonationRadius, LayerMask.GetMask("Ennemies"));
            foreach(Collider c in hitColliders)
            {
                if(c.gameObject.CompareTag("Ennemy"))
                {   // for each ennemy in Area of detonation
                    explosionForce = ((detonationRadius - Vector3.Distance(c.gameObject.transform.position, transform.position)) / detonationRadius) * maxDetonationForce;
                    explosionVector = (c.gameObject.transform.position - transform.position).normalized;
                    c.gameObject.GetComponent<Rigidbody>().AddForce(explosionVector * explosionForce, ForceMode.Impulse);
                }
            }
        }
        else
        {
            OnTriggerExit(); // Force reset ennemies's target back to player;
        }

        // Play Sound
        AudioSource.PlayClipAtPoint(detonationSound,transform.position, 0.5f);
        // Destroy
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider col) // TODO : NOT WORKING right now because of case : 1/ multiple decoy at one place
                                                                                     // 2/ ONE decoy rolls away, destroy, etc
                                                                                     // 3/ It resets the target back to player for every ennemies
    {
        if(col.gameObject.CompareTag("Ennemy"))
            col.gameObject.GetComponent<EnnemyBehavior>().target = gameObject; // Set grenade as new target for every Ennemy in radius
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.CompareTag("Ennemy"))
            col.gameObject.GetComponent<EnnemyBehavior>().target = GameObject.Find("player"); // Set back player as target for every Ennemy going out of radius
    }
}
