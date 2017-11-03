/*
 * Author : Aryetis
 * Copyright : CC BY-NC-SA 3.0
 */

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class localViewTool : EditorWindow 
{
    [SerializeField] static List<GameObject> hiddenObjects = new List<GameObject>();

    /* TODO : Add take care of hiding/restoring/showing only children of selected GameObject too if any
     *        check what happens if we close editor with some gameobjects "turned off" 
     * 
     */

	[MenuItem ("Tools/localView/HideSelection %&h")]
	static void HideSelection ()
	{
        foreach (GameObject go in UnityEditor.Selection.gameObjects)
            setSelectionState(go, false);
	}

    static void setSelectionState(GameObject go, bool status)
    {
        // Deactivate current renderer
        if (go.GetComponent<Renderer>() != null)
        {
            go.GetComponent<Renderer>().enabled = status;
            hiddenObjects.Add(go);
        }

        // Explore children
        int childrenCount = go.transform.childCount;
        foreach (Transform goct in go.transform)
            setSelectionState(goct.gameObject, status);
    }


	[MenuItem ("Tools/localView/ShowOnlySelection %&s")]
	static void ShowOnlySelection ()
	{
        // Hide all objects
		foreach(GameObject go in FindObjectsOfType(typeof(GameObject)) as GameObject[])
			if (go.GetComponent<Renderer>() != null)
			{
				go.GetComponent<Renderer>().enabled = false ;
				hiddenObjects.Add(go);
			}

        // Show selection
		foreach(GameObject go in UnityEditor.Selection.gameObjects)
            setSelectionState(go, true);
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
		//TODO AssignHotkeys
		Debug.Log ("Doing Something...");
		localViewTool window = (localViewTool)EditorWindow.GetWindow (typeof (localViewTool));
		window.Show();
	}
}
