using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour {

    GameObject console;

	// Use this for initialization
	void Start ()
    {
        console = transform.parent.gameObject;	
	}
	
	
    public void OnClick()
    {
        console.SetActive(false);
    }
}
