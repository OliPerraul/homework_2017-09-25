using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PhysicsObject
{

    public float jmp_takeoff_spd = 7;
    public float max_spd = 7;

    private SpriteRenderer sprRend;
    private SpriteAnimator spriteAnimator;
    private Animator animator;


    /////Health System///
    private HealthBar healthBar;

    [SerializeField]
    public float HP = 1;

    private bool invulnerable;

    //dammage feedback
    [SerializeField]
    private float knockback_amount = 7;

    //[SerializeField]
    private float timer_glitch = 0;
    [SerializeField]
    private float glitch_time = 360;


    protected override void Update()
    {
        base.Update();

        healthBar.hide_percent = 1f - HP;

        UpdateShader();

        if (timer_glitch > 0)
            invulnerable = true;
        else
            invulnerable = false;
    }
    
	// Use this for initialization
	void Awake ()
    {
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        
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

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (!invulnerable)
            {
                Enemy enemy = other.GetComponent<Enemy>();
                GetHurt(enemy);
                Knockback(other);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (!invulnerable)
            {
                Enemy enemy = other.GetComponent<Enemy>();
                GetHurt(enemy);
                Knockback(other);
            }

        }
    }

    void Knockback(Collider2D other)
    {
        if (other.transform.position.x > transform.position.x)
            target_velocity.x += -knockback_amount;
        else
            target_velocity.x += knockback_amount;
    }

    public void GetHurt(Enemy enemy)
    {
        timer_glitch = glitch_time;

        HP = HP-enemy.damage;
        healthBar.GetHurt();

    }

  
    void UpdateShader()
    {
        if (timer_glitch > 0)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            sprRend.GetPropertyBlock(mpb);
            mpb.SetFloat("_WrapDispCoords", 1);
            mpb.SetFloat("_DispGlitchOn", 1);
            mpb.SetFloat("_ColorGlitchOn", 1);
            sprRend.SetPropertyBlock(mpb);


            //TODO put in regular update
            timer_glitch -= Time.time;

        }
        else
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            sprRend.GetPropertyBlock(mpb);
            mpb.SetFloat("_WrapDispCoords", 0);
            mpb.SetFloat("_DispGlitchOn", 0);
            mpb.SetFloat("_ColorGlitchOn", 0);
            sprRend.SetPropertyBlock(mpb);
        }

        
    }

}
