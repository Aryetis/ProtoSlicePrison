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


	// Use this for initialization
	void Start ()
    {
		
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
//            GameObject debugSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//            debugSphere.transform.localScale = new Vector3(detonationRadius, detonationRadius, detonationRadius);
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
            Debug.Log("decoy !!!");
        }

        // Play Sound
        AudioSource.PlayClipAtPoint(detonationSound,transform.position, 0.5f);
        // Destroy
        Destroy(gameObject);
    }
}
