using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{

    public float jmp_takeoff_spd = 7;
    public float max_spd = 7;

    private SpriteRenderer sprRend;
    private SpriteAnimator spriteAnimator;
    private Animator animator;


    //Health System
    private HealtBar healthBar;
    private int health;
    
    
    
	// Use this for initialization
	void Awake ()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<HealtBar>(); 

        sprRend = GetComponent<SpriteRenderer>();
        spriteAnimator = GetComponent<SpriteAnimator>();

       // animator = GetComponent<Animator>();
	}

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        //set the x val of move based on the incoming control input from the player
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jmp_takeoff_spd;

        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
                velocity.y = velocity.y * .5f; //release vel when play release jmp (by half)
        }
              
        //multiply the movement vector based on the speed of the player
        target_velocity = move * max_spd;

        //adjust animations
        AdjustSprite(move);

    }


    void AdjustSprite(Vector2 move)
    {

        //check if we need to flip sprite
        bool flip_spr = (sprRend.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flip_spr)
        {
            sprRend.flipX = !sprRend.flipX;

        }

        float velx = Mathf.Abs(velocity.x);

        if (velx > 0 && velx < max_spd)
            spriteAnimator.Play("walk", true, 0);
        else if (velx > 0)
            spriteAnimator.Play("run", true, 0);
        else spriteAnimator.Play("idle", true, 0);


    }


    public void GetHurt()
    {
        health--;

        

    }
    


}
