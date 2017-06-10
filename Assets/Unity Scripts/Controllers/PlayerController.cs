using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{

    public float jmp_takeoff_spd = 7;
    public float max_spd = 7;

    private SpriteRenderer sprRend;
    private Animator animator;

	// Use this for initialization
	void Awake ()
    {
        sprRend = GetComponent<SpriteRenderer>();
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
        

       // animator.SetBool("grounded", grounded);
        //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / max_spd);

    }





}
