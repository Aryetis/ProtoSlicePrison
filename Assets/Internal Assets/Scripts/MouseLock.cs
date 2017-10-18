using UnityEngine;
using System.Collections;

public class MouseLock : MonoBehaviour
{
	[SerializeField] private float timeScale;
	[SerializeField] private bool lockCursor;

	// Use this for initialization
	void Start ()
	{
		Time.timeScale = timeScale ;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lockCursor)
		{
			Cursor.lockState = CursorLockMode.Locked ;
			Cursor.visible = false ;
		}		
	}
}
