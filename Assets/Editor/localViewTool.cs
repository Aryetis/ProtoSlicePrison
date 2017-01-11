using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class localViewTool : EditorWindow 
{
	static List<GameObject> hiddenObjects = new List<GameObject>();



	[MenuItem ("Tools/localView/HideSelection %&h")]
	static void HideSelection ()
	{
		foreach(GameObject go in UnityEditor.Selection.gameObjects)
			if (go.GetComponent<Renderer>() != null)
			{
				go.GetComponent<Renderer>().enabled = false ;
				hiddenObjects.Add(go);
			}
	}

	[MenuItem ("Tools/localView/ShowOnlySelection %&s")]
	static void ShowOnlySelection ()
	{
		foreach(GameObject go in FindObjectsOfType(typeof(GameObject)) as GameObject[])
			if (go.GetComponent<Renderer>() != null)
			{
				go.GetComponent<Renderer>().enabled = false ;
				hiddenObjects.Add(go);
			}

		foreach(GameObject go in UnityEditor.Selection.gameObjects)
			go.GetComponent<Renderer>().enabled = true ;
	}

	[MenuItem ("Tools/localView/RestoreHiddenObjects %&r")]
	static void RestoreHiddenObjects ()
	{
		foreach(GameObject go in hiddenObjects)
			go.GetComponent<Renderer>().enabled = true ;
	}

	[MenuItem ("Tools/localView/Options")]
	static void Options ()
	{
		//TODO hide only ? hide + non interactable ? + AssignHotkeys
		Debug.Log ("Doing Something...");
		localViewTool window = (localViewTool)EditorWindow.GetWindow (typeof (localViewTool));
		window.Show();
	}
}
