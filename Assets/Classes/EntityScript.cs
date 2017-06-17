using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class EntityScript
{
    public Table table { get; set; }
    public string code { get; set; }
    public string name { get; set; }
    
    public EntityScript(string _code, string _name)
    {
        name = _name;

        code = _name + _code;

        //ref to sprite database
        AddSpriteReference();

        Global.main_script.DoString(code);
        //try else the entity class is invalid (TODO)
        DynValue val = Global.main_script.Globals.Get(name);

        if (IsValidEntity(val))
            table = val.Table;
        else
            table = null;

      }

    public EntityScript(string _code, string _name, string old_name)
    {
        name = _name;

        int index = _code.IndexOf(old_name);

        if (index != -1)
            _code = _code.Remove(index, old_name.Length); //remove old class name

        code = _name + _code;

        //ref to sprite database
        AddSpriteReference();

        Global.main_script.DoString(code);
        //try else the entity class is invalid (TODO)
        DynValue val = Global.main_script.Globals.Get(name);

        if (IsValidEntity(val))
            table = val.Table;
        else
            table = null;

    }

    //used to update tables value when added to the world (e.g getmousex()...)
    public void OverwriteTable()
    {
        Global.main_script.DoString(code);
        //try else the entity class is invalid (TODO)
        DynValue val = Global.main_script.Globals.Get(name);

        if (IsValidEntity(val))
            table = val.Table;
        else
            table = null;
    }


    //helper method
    public static bool IsValidEntity(DynValue item)
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
        

   
    /// /TODO PUT SOMEWHERE ELSE
  
    void AddSpriteReference()
    {
        foreach (KeyValuePair<string, Texture2D> entry in Global.sprite_database)
        {
            Global.main_script.Globals[entry.Key] = entry.Key;
        }

    }


}
