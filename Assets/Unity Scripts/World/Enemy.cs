using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //Components
    private SpriteAnimator spriteAnimator;

    
    //

    private GameObject player;

    [SerializeField]
    private float move_speed = .002f;
    
    
    // Use this for initialization
	void Start ()
    {
        spriteAnimator = GetComponent<SpriteAnimator>();
        player = GameObject.Find("Player");

        spriteAnimator.Play("idle", true, 0);

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!MathUtils.IsBetween((int)transform.position.x, (int)player.transform.position.x - 5, (int)player.transform.position.x + 5) && !MathUtils.IsBetween((int)transform.position.y, (int)player.transform.position.y - 5, (int)player.transform.position.y + 5))
        {
            move_speed = .002f;

        }
        else
            move_speed = .01f;
        

        transform.position = Vector2.Lerp(transform.position, player.transform.position, move_speed);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        ScriptedEntity scrEnt = other.gameObject.GetComponent<ScriptedEntity>();

        if (scrEnt != null)
        {
            if (scrEnt.type.Equals("bullet"))
                ObjectPoolManager.DestroyPooled(gameObject);
        }
    }



}
