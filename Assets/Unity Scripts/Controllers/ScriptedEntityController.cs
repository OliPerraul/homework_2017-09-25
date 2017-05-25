using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEntityController : MonoBehaviour
{
    [SerializeField]
    private int max_children = 10;
 	
    
    // Update is called once per frame
	void Update ()
    {
        //force center
        transform.position = new Vector3(0, 0, 0);


        //Recycle objects if max is reached
        if (transform.childCount > max_children)
        {
            GameObject child = transform.GetChild(0).gameObject;
            ObjectPoolManager.DestroyPooled(child);

        }

    }





}
