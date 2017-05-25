using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System;

public class ScriptedEntity : MonoBehaviour
{
    private SpriteRenderer spriteRend;
    private Vector3 pos = new Vector3(0, 0, 0);

    //entity table
    private Table entity_table;

    //Moonsharp Methods
    private Closure entity_update { get; set; }
    private Closure entity_start { get; set; } //fired once when object starts
    
    //Player accessible properties
    public double x = 0;//REQUIRED
    public double y = 0;//REQUIRED

    public float centerX { get; set; }
    public float centerY { get; set; }
    public float collidable { get; set; }

    public float height { get; set; }
    public float width { get; set; }
    public float layer { get; set; }

    public float left { get; set; }
    public float right { get; set; }
    public float top { get; set; }
    public float bottom { get; set; }

    public float originX { get; set; }
    public float originY { get; set; }

    public string type { get; set; }
    public bool visible { get; set; }


    public Sprite sprite;

	// Use this for initialization
	void Start ()
    {
        entity_start.Call(entity_table); //call the start function

        //get the updated values
        x = entity_table.Get("x").Number;
        y = entity_table.Get("y").Number;

        //update pos
        pos = new Vector3((float)x, (float)y);
        transform.position = pos;

    }

    // Update is called once per frame
    void Update()
    {
        //pass in x, y
        entity_table["x"] = x;
        entity_table["y"] = y; //to do pass all other built in vars

        entity_update.Call(entity_table); //cal update function

        x = entity_table.Get("x").Number;
        y = entity_table.Get("y").Number;

         //update pos
        pos = new Vector3((float)x, (float)y);
        transform.position = pos;

    }

    public static GameObject Create(Table entity_table)//custom init
    {
        //find container
        GameObject scriptedEntityController = GameObject.Find("ScriptedEntityController");

        //begin creation
        GameObject prefab = Resources.Load("ScriptedEntity") as GameObject;
        GameObject newObject = ObjectPoolManager.CreatePooled(prefab, Vector3.zero, Quaternion.Euler(0, 0, 0));
        
        newObject.name = prefab.name; //assign correct name
        newObject.transform.SetParent(scriptedEntityController.transform);//assign parent

        //

        ScriptedEntity scriptedEntity = newObject.GetComponent<ScriptedEntity>(); //access main component
        InitProperties(ref scriptedEntity, entity_table);

        return newObject;//return object after params are init
    }


    static void InitProperties(ref ScriptedEntity scriptedEntity, Table ent_tab)
    {
        //entity properties
        DynValue start = ent_tab.Get("start");
        DynValue update = ent_tab.Get("update");
        DynValue x = ent_tab.Get("x");
        DynValue y = ent_tab.Get("y");

        scriptedEntity.entity_table = ent_tab;//set the entity table

        scriptedEntity.entity_start = start.Function;
        scriptedEntity.entity_update = update.Function;
        scriptedEntity.x = x.Number;
        scriptedEntity.y = y.Number;

    }



}
