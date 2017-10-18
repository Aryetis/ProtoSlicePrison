using UnityEngine;
using System.Collections;

public class DoorBehavior : MonoBehaviour
{
	private enum State{closed, opened};
	private State state = State.closed;
	private Transform battant1, battant2;
	

	// Use this for initialization
	void Start ()
	{
		battant1 = transform.Find("battant1");
		battant2 = transform.Find("battant2");
	}
	
	// Update is called once per frame
	void Update ()
	{
		
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
}