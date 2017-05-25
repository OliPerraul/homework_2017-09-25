using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System;

public class ScriptedEntity : MonoBehaviour
{
    #region Dictionary lookup
    static Dictionary<int, string> builtIn_vars_lookup = new Dictionary<int, string>
    {
        { 1, "\"update\"" },
        { 2, "\"start\"" },
        { 3, "\"x\"" },
        { 4, "\"y\"" },
        { 5, "\"collidable\"" },
        { 6, "\"height\"" },
        { 7, "\"width\"" },
        { 8, "\"layer\"" },
        { 9, "\"left\"" },
        { 10, "\"right\"" },
        { 11, "\"top\"" },
        { 12, "\"bottom\"" },
        { 13, "\"originX\"" },
        { 14, "\"originY\"" },
        { 15, "\"type\"" },
        { 16, "\"visible\"" },

    };

  
    #endregion
    
    #region GameObject properties
      
    //Main table
    private Table entity_table;
    //sorted tables
    private Table builtIn_vars;
    private Table player_defined_vars;

    //Dictionary Items
    private Closure entity_update { get; set; }
    private Closure entity_start { get; set; } //fired once when object starts
    public double x = 0;
    public double y = 0;
    public double centerX { get; set; }
    public double centerY { get; set; }
    public bool collidable { get; set; }
    public double height { get; set; }
    public double width { get; set; }
    public double layer { get; set; }
    public double left { get; set; }
    public double right { get; set; }
    public double top { get; set; }
    public double bottom { get; set; }
    public double originX { get; set; }
    public double originY { get; set; }
    public string type { get; set; }
    public bool visible { get; set; }

    #endregion

    // Use this for initialization
    void Start ()
    {
        //assign dictionary items to instance properties     
        AssignBuiltInVars();
        
        ////////////call the start function/////////////
        entity_start.Call(entity_table);
        
        //update dictionaries from table
        TableToDictionaries();

        //assign dictionary items to instance properties     
        AssignBuiltInVars();

        //Behave according to Builtin vars
        ApplyBehaviour();
        
    }

    // Update is called once per frame
    void Update()
    {
        //assign dictionary items to instance properties     
        AssignBuiltInVars();

        //Update table with dic values
        DictionariesToTable();

        /////////////////call update function//////////////////
        entity_update.Call(entity_table);
        ////////////////////////////////////

        //update dictionaries from table
        TableToDictionaries();

        //assign dictionary items to instance properties     
        AssignBuiltInVars();

        //Behave according to Builtin vars
        ApplyBehaviour();
        
    }

    void AssignBuiltInVars()
    {
        //uded to check
        DynValue val = new DynValue();

        if (TryGetValue(builtIn_vars, "update", out val))
        entity_update = val.Function;
        
        if (TryGetValue(builtIn_vars, "start", out val))
            entity_start = val.Function;

        if (TryGetValue(builtIn_vars, "x", out val))
            x = val.Number;

        if (TryGetValue(builtIn_vars, "y", out val))
            y = val.Number;

        if (TryGetValue(builtIn_vars,"centerX", out val))
            centerX = val.Number;

        if (TryGetValue(builtIn_vars, "centerY", out val))
            centerY = val.Number;

        if (TryGetValue(builtIn_vars, "collidable", out val))
            collidable = val.Boolean;

        if (TryGetValue(builtIn_vars, "height", out val))
            height = val.Number;

        if (TryGetValue(builtIn_vars, "width", out val))
           width = val.Number;

        if (TryGetValue(builtIn_vars, "layer", out val))
            layer = val.Number;

        if (TryGetValue(builtIn_vars, "left", out val))
            left = val.Number;

        if (TryGetValue(builtIn_vars,"right", out val))
            right = val.Number;

        if (TryGetValue(builtIn_vars, "top", out val))
            top = val.Number;

        if (TryGetValue(builtIn_vars,"bottom", out val))
            bottom = val.Number;

        if (TryGetValue(builtIn_vars, "originX", out val))
           originX  = val.Number;

        if (TryGetValue(builtIn_vars,"originY", out val))
            originY = val.Number;

        if (TryGetValue(builtIn_vars, "type", out val))
            type = val.String;

        if (TryGetValue(builtIn_vars,"visible", out val))
            visible = val.Boolean;

     }
    //helper method
    static bool TryGetValue(Table table,string key, out DynValue val)
    {
       val = table.Get(key);

        if (val == null || val.IsNilOrNan())
            return false;
        else
            return true;
                  
     }
    
    //apply defined behaviour
    void ApplyBehaviour()
    {
        //update pos
        Vector3 pos = new Vector3((float)x, (float)y);
        transform.position = pos;

    }
    

    //Pass in Dictionaries values to entity table
    void DictionariesToTable()
    {
        //pass in builtin into script
        foreach (var key in builtIn_vars.Keys)
        {
            entity_table[key] = builtIn_vars[key];
        }
        //pass in player defined into script
        foreach (var key in player_defined_vars.Keys)
        {
            entity_table[key] = player_defined_vars[key];
        }
        /////////////////

    }


    //Updates dictionaries based on table
    void TableToDictionaries()
    {
        ////////retrieve builtin after update
        foreach (var key in builtIn_vars.Keys)
        {
             builtIn_vars[key] = entity_table.Get(key);
        }
        //retrieve player defined after update
        foreach (var key in player_defined_vars.Keys)
        {
            player_defined_vars[key] = entity_table.Get(key);
        }
        //////////////

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

        ScriptedEntity scriptedEntity = newObject.GetComponent<ScriptedEntity>(); //access main component
        InitProperties(ref scriptedEntity, entity_table);

        return newObject;//return object after params are init
    }
    
    static void InitProperties(ref ScriptedEntity scriptedEntity, Table entity_table)
    {
        Table player_defined_vars = new Table(entity_table.OwnerScript);
        Table builtIn_vars = new Table(entity_table.OwnerScript);

        //use look up table to sort built in vars from non
        DeterminePlayerDefinedVars(entity_table, ref player_defined_vars, ref builtIn_vars);
        
        scriptedEntity.entity_table = entity_table;//set the entity table
        scriptedEntity.player_defined_vars = player_defined_vars;//set vars
        scriptedEntity.builtIn_vars = builtIn_vars;       
    }

    static void DeterminePlayerDefinedVars(Table entity_table, ref Table player_defined_vars, ref Table builtIn_vars)
    {
        //sort entity variables
        foreach (var key in entity_table.Keys)
        {
             //get table item
            DynValue item = entity_table.Get(key);

            //add to correct table
            if (builtIn_vars_lookup.ContainsValue(key.ToString()))
            {
                builtIn_vars.Set(key, item);
            }
            else
                player_defined_vars.Set(key, item);
        }
    }



}
