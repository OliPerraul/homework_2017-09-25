using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System;

public class MasterControlProgram : MonoBehaviour
{
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
    	
    i = 0;
	while( world[i] != nil)
    do
    i = i+1;
    end
    world[i] = val;
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

    

    void Awake()
    {
        //Lua initialiszation
        Global.main_script = new Script();
        Global.main_script.DoString(existing_code);
        //get the World table from the script
        Global.world = Global.main_script.Globals.Get("world").Table;
        RegisterLuaFunctions();
            
    }


    #region Functions called from lua
    /////
    public static void RegisterLuaFunctions()
    {
        Global.main_script.Globals["getplayerx"] = (Func<float>)GetPlayerXFunction;
        Global.main_script.Globals["getplayery"] = (Func<float>)GetPlayerYFunction;
        Global.main_script.Globals["getmousex"] = (Func<float>)GetMouseXFunction;
        Global.main_script.Globals["getmousey"] = (Func<float>)GetMouseYFunction;

    }

    private static float GetPlayerXFunction()
    {
        GameObject player = GameObject.Find("Player");
        float x = player.transform.position.x;

        return x;
    }

    private static float GetPlayerYFunction()
    {
        GameObject player = GameObject.Find("Player");
        float y = player.transform.position.y;

        return y;
    }

    private static float GetMouseXFunction()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        float x = pos.x;

        return x;
    }


    private static float GetMouseYFunction()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        float y = pos.y;

        return y;
    }

    #endregion

}
