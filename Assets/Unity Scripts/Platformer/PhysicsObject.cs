using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float min_ground_normalY = .65f;
    public float gravMod = 1f; //allows us to scale grav using a float

    protected Vector2 velocity; //init 0,0
    protected Vector2 target_velocity; //where we stor the incoming input from outside as to where the obj is trying to move

    protected bool grounded;
    protected Vector2 ground_normal;

    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> list_hitBuffer = new List<RaycastHit2D>(16);

    //shell makes sure we never get stuck in other collider
    protected const float shellRadius = 0.01f; //padding added to dist make sure we never pass in other collider

    protected const float minMoveDist =  0.001f;

    //called when the object becomes active
    void OnEnable()
    {
       rb2d = GetComponent<Rigidbody2D>();
    }
    
    
    // Use this for initialization
	void Start ()
    {
        contactFilter.useTriggers = false; //not check collisions against trigger colliders

        //use settings from Physics2d settings to det what layers well check coll against
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;//use layer mask of actual physics object (built in physics obj)
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        target_velocity = Vector2.zero; //make sure we are not using target velocity from the prev frame
        ComputeVelocity();
	}

    protected virtual void ComputeVelocity()
    {
        //implemented in player controller

       // virtual method is an inheritable and overridable function or method for which dynamic dispatch is facilitated.
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = false;

        /////////////////////// gravity
        velocity += gravMod * Physics2D.gravity * Time.deltaTime; //change vel based on gravity
        velocity.x = target_velocity.x; //add incoming value into vel calc
        
        
        //change in position (position the object will be)
        Vector2 deltaPos = velocity * Time.deltaTime; //update position after frame is rendered
        
        //

        ///////////////////move along ground
        Vector2 move_along_ground = new Vector2(ground_normal.y, -ground_normal.x); //stores direction that were trying to move along ground

        //calculate x value to move
        Vector2 move = move_along_ground * deltaPos.x;

        Movement(move, false); //handles horizontal movement

        //

        /////////////////////handles vertical movement
        move = Vector2.up * deltaPos.y;
        
         Movement(move, true); 

        //

    }

    //move object by setting pos of rigid body
    void Movement(Vector2 move, bool yMovement)
    {
        //we only want to apply coll if the dist we are attempting to move is greater than a min value (ensures that we are not alway checking collision)
        float dist = move.magnitude;
        
        if (dist > minMoveDist)
        {
           int count =  rb2d.Cast(move, contactFilter, hitBuffer, dist+shellRadius);

            list_hitBuffer.Clear();

            //copy only indexes that actually hit something: iterates count times
            for(int i = 0; i< count; i++)
            {
                list_hitBuffer.Add(hitBuffer[i]);
            }

            //loop over hibuf list and compare object normal to a min ground normal value
            for (int i = 0; i < list_hitBuffer.Count; i++)
            {
                Vector2 currNormal = hitBuffer[i].normal;
                //det if player is grounded
                if (currNormal.y > min_ground_normalY)
                {
                    grounded = true;
                    if(yMovement)
                    {
                        ground_normal = currNormal;
                        currNormal.x = 0;

                    }
                }

                
                //scalar of the projection of velocity onto the normal to the ground which we have killed the x
                float s_projection = Vector2.Dot(velocity, currNormal) / currNormal.sqrMagnitude;

                if (s_projection < 0) //meaning movement and currNormal are generally opposite
                {
                          velocity = velocity - s_projection * currNormal; //cancel out part of the velocity that would be stopped by the collision
                }

                //prevent from getting stuck in other colliders
                //dist: dist away from object casting to hit point
                float modifiedDist = list_hitBuffer[i].distance - shellRadius; //remove shellRadius since we've ourselves added it earlier
                dist = modifiedDist < dist ? modifiedDist : dist; //if dist remaining until hitpoint is smaller than dist were about to move: use that as dist that were about to move 


            }
            

        }

        //apply movement
        rb2d.position = rb2d.position + move.normalized * dist;
    }




}
