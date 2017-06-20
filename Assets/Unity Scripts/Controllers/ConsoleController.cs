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
        //TODO OPTIMISE by inserting only when database is changed
        //Insert sprites vars in global environemtn
        foreach (KeyValuePair<string, Texture2D> entry in Global.sprite_database)
        {
            Global.main_script.Globals[entry.Key] = entry.Key;
        }
        
        //Add entities to global environment
        foreach (KeyValuePair<string, EntityScript> entry in Global.entity_database)
        {
            EntityScript entity = entry.Value;
            entity.OverwriteTable(); //overwrite old table with newer values (getmousex...)
            Global.main_script.Globals[entry.Key] = entity.table;
        }

            
        string scriptCode = inputField_code;

        Global.main_script.DoString(scriptCode);
             
        //Debug.Log(Global.main_script.Globals.Get("printed"));
                                
        CreateFromWorldTable();
        
    }

    //instantiate scriptedEntities from world table
    void CreateFromWorldTable()
    {
        //used to iterate
        DynValue item = new DynValue();
    
        foreach (var key in Global.world.Keys)
        {
            item = Global.world.Get(key); //get table item

            if (EntityScript.IsValidEntity(item))
            {
                int id = item.ReferenceID;
            
                Table entity = item.Table; //pass in the entity table

                if (!Global.world_added.Contains(id))//if not already in the world
                {
                  
                    Global.world_added.Add(id);
                    GameObject scrEnt = ScriptedEntity.Create(entity, id);

                }//
           
            }//

           
        }//end loop

    
        
    }

  
}
