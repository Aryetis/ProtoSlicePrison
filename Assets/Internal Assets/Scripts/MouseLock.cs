using UnityEngine;
using System.Collections;

public class MouseLock : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		Time.timeScale = 0.25f;
	}
	
	// Update is called once per frame
	void Update ()
	{
//		Screen.lockCursor = true; //deprecated
//		Cursor.lockState = CursorLockMode.Locked ;
//		Cursor.visible = false ;
	}
}
