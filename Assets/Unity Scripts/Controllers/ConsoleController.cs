using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoonSharp.Interpreter;
using System;

public class ConsoleController : MonoBehaviour
{
    [SerializeField]
    private InputFieldController inputFieldController; //ref used to subscribe
    

    //TODO 
    //BEFORE SUBMIT READD ALL EXISTING OBJ TO THE TABLE WORLD
    //AFTER SUBMIT: IF INSTANCES ARE NOT PRESENT IN TABLE WORLD, DESTROY THEM FROM UNITY WORLD
    private string existing_code = @"

    world = {};--end table    
      
    function new(Entity)
	    instance = {}

	    for key, value in pairs(Entity) do
	    instance[key] = value;
	    end

	    return instance;
    end --end method

    function add(val) --used in game
	    world[tablelength(world)+1] = val;
	end --end method

    function add_to_table(val, table)
	    table[tablelength(table)+1] = val;
	end --end method

    function tablelength(T)
        local count = 0
         for _ in pairs(T) do count = count + 1 end
        return count
    end--end method

    --assign math functions
    abs = math.abs;
    acos= math.acos
    asin= math.asin;
    atan= math.atan;
    ceil= math.ceil;
    cos= math.cos;
    deg= math.deg;
    exp= math.exp;
    floor=math.floor;
    fmod=math.fmod;
    huge=math.huge;
    log=math.log;
    max=math.max;
    maxinteger=math.maxinteger;
    min=math.min;
    mininteger= math.mininteger;
    modf=math.modf;
    pi =math.pi;
    rad =math.rad;
    random= math.random;
    randomseed= math.randomseed;
    sin = math.sin;
    sqrt=math.sqrt;
    tan=math.tan;
    tointeger=math.tointeger;
    type=math.type;
    ult=math.ult;

    ";
              
    // Use this for initialization
	void Start ()
    {
        //subscribe to submit event
        inputFieldController.InputFieldSubmitted += this.OnInputFieldSubmitted;

    }
	
    //event handling method
    public void OnInputFieldSubmitted(string content)
    {
       InterpreteContent(content);
        
    }

    void InterpreteContent(string inputField_code)
    {
        string scriptCode = existing_code + inputField_code;
       

        Script script = new Script();

        script.DoString(scriptCode);

        //get the World table from the script
        Table luaTable_world = script.Globals.Get("world").Table;
                
        CreateFromWorldTable(luaTable_world);
    }

    //instantiate scriptedEntities from world table
    void CreateFromWorldTable(Table world)
    {
        //used to iterate
        DynValue item = new DynValue();

        foreach (var key in world.Keys)
        {
            item = world.Get(key); //get table item
           
            if (IsValidEntity(item))
            {
                Table entity = item.Table; //pass in the entity table
                GameObject scrEnt = ScriptedEntity.Create(entity);
               
            }
        }
                      
    }

    //helper method
    bool IsValidEntity(DynValue item)
    {
        if (!MoonSharpUtils.IsTable(item))
            return false;

        //table to check if right props are in
        Table entity = item.Table;

        DynValue test_x = entity.Get("x");
        DynValue test_y = entity.Get("y");
        DynValue test_start = entity.Get("start");
        DynValue test_update = entity.Get("update");

        if (test_x == null || !MoonSharpUtils.IsNumber(test_x))
            return false;

        if (test_y == null || !MoonSharpUtils.IsNumber(test_y))
            return false;

        if (test_start == null || !MoonSharpUtils.IsFunction(test_start))
            return false;

        if (test_update == null || !MoonSharpUtils.IsFunction(test_update))
            return false;
        
        //else return true
        return true;

    }

        
}
