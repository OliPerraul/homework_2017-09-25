using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtBar : MonoBehaviour {


    SpriteRenderer spriteRend_healthpegs;

	// Use this for initialization
	void Start ()
    {
        spriteRend_healthpegs = GetComponentInChildren<SpriteRenderer>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void GetHurt()
    {

    }

   
    

}
