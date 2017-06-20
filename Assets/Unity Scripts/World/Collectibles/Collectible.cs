using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : PhysicsObject
{
    protected static GameObject collectiblesController;

    protected string name;
    protected Vector2 start_pos;

    void Start()
    {
        transform.position = start_pos;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
         if (other.tag == "Player")
         {
            Collected();
         }
                        
     }

    protected virtual void Collected()
    {
        ObjectPoolManager.DestroyPooled(gameObject);
    }

}
