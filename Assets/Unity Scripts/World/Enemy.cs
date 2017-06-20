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

    [SerializeField]
    public float damage = .01f;


    //Loot system//

        //rates
    [SerializeField]
    [Range(0,1)]
    protected float drop_rate_entities = .2f;

    [SerializeField]
    [Range(0, 1)]
    protected float drop_rate_sprites = .2f;

    [SerializeField]
    [Range(0, 1)]
    protected float drop_rate_health = .2f;

        //ammounts
    [SerializeField]
    [Range(1, 5)]
    public int drop_amount_entities = 5;

    [SerializeField]
    [Range(1, 5)]
    public int drop_amount_sprites = 5;

    [SerializeField]
    [Range(1, 5)]
    public int drop_amount_health = 5;


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
                Killed(); //TODO DAMAGED FIRST
        }
    }

    void Killed()
    {
        DropLoot();
        ObjectPoolManager.DestroyPooled(gameObject);
    }

    void DropLoot()//Override function to drop specific
    {
        DropProperties dropProp = new DropProperties();
        dropProp.drop_rate_entities =drop_rate_entities;
        dropProp.drop_rate_sprites = drop_rate_sprites;
        dropProp.drop_rate_health =  drop_rate_health;

        dropProp.amount_entities = Random.Range(1, drop_amount_entities);
        dropProp.amount_sprites = Random.Range(1, drop_amount_sprites);
        dropProp.amount_health = Random.Range(1, drop_amount_health);

        CollectiblesController.DropRandom(transform.position, dropProp);
    }




}
