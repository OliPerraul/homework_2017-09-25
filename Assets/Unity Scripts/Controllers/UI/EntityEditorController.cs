using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityEditorController : EditorController
{
    [SerializeField]
    private GameObject _consoleContainer;
    static GameObject consoleContainer;

    [SerializeField]
    private InputField _console;
    static InputField console;


    static EntityName entityName;

    static EntityScript edited_entity;
    static GameObject edited_entity_icon;

    void Start()
    {
        //assign statics from private
        consoleContainer = _consoleContainer;
        console = _console;

        console.onEndEdit.AddListener(delegate { OnEndEdit(); }); //add listen to event

        //add listener to on click event
        butt_new.onClick.AddListener(delegate { New(); });

        //add listener to on click event
        // butt_delete.onClick.AddListener(delegate { SpriteEditorController.SelectSprite(); });
    }

    //create a new blank sprite when new button is clicked and adds it to the visual list
    public static void New()
    {
        string name = NameField.AdjustNameWithIndex("obj_new", Global.entity_database);
        EntityScript entity = new EntityScript(Global.default_entity_code, name);

        Global.entity_database.Add(name, entity);

        EntityIcon.Create(name, entity);

    }


    //TODO CREATE all icons FROM DATABAsE
    public static void New(string name, EntityScript ent)
    {
        name = NameField.AdjustNameWithIndex(name, Global.entity_database);

        //add sprite to the database
        Global.entity_database.Add(name, ent);

        EntityIcon.Create(name, ent);

    }


    //event handling method when icon clicked
    public static void SelectEntity()
    {
        edited_entity_icon = EventSystem.current.currentSelectedGameObject;
        InputField name_field = edited_entity_icon.transform.GetChild(0).GetComponent<InputField>();

        string ent_name = name_field.text;

        Global.entity_database.TryGetValue(ent_name, out edited_entity);

        ///UPDATE THE CONSOLE
        consoleContainer.SetActive(true);

        //set text of the console to code of the clicked object
        console.text = edited_entity.code;
        console.GetComponent<ColorCoder>().ColorCode();
    }

    public static void OnEndEdit()
    {
        string old_name = edited_entity.name;
        //get new name if changed via code
        string new_name = EntityScript.GetEntityNameFromCode(console.text);

        EntityScript ent;
        if (!Global.entity_database.TryGetValue(new_name, out ent))
        {//change icon text with that new name
            InputField name_field = edited_entity_icon.transform.GetChild(0).GetComponent<InputField>();
            name_field.text = new_name;
            ent = new EntityScript(console.text, new_name, new_name);

            if (ent.table != null) //TODO:MORE CHECKs IF VALID (eg name must match table name)
            {
                Global.entity_database.Remove(old_name);
                Global.entity_database.Add(new_name, ent);
            }
        }
        else//make a new entity using old name
        {
            ent = new EntityScript(console.text, old_name, new_name);

            if (ent.table != null) //TODO:MORE CHECKs IF VALID (eg name must match table name)
            {
                Global.entity_database.Remove(old_name);
                Global.entity_database[old_name] = ent;
            }
        }

    }


}
