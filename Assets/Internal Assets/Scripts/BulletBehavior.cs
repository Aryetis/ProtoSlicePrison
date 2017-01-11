using UnityEngine;
using System.Collections;

public class BulletBehavior : MonoBehaviour {

	private float TTL = 20f;

	// Use this for initialization
	void Start ()
	{
		InvokeRepeating("autodestruct", TTL, 0f);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void autodestruct()
	{
		Destroy(gameObject);
	}

	void OnCollisionEnter(Collision col)
	{
		Destroy (gameObject);
		//TODO play destruction FX
	}
}
