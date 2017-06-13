using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEntityController : MonoBehaviour
{
    /// <summary>
    /// num of active entities
    /// </summary>
   public static int num_entities { get; set; }


    /// <summary>
    /// Max num of entities
    /// </summary>
    [SerializeField]
    private int max_entities = 10;
 	    
    // Update is called once per frame
	void Update ()
    {
        //force center
        //transform.position = new Vector3(0, 0, 0);
        
         num_entities = transform.childCount;
                        
        //Recycle objects if max is reached
        if (transform.childCount > max_entities)
        {
            GameObject child = transform.GetChild(0).gameObject;
            ObjectPoolManager.DestroyPooled(child);

        }

    }
    
    
}
