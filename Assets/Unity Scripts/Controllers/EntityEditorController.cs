using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityEditorController : MonoBehaviour
{
    [SerializeField]
    private GameObject console;
    [SerializeField]
    InputField inputField;

    private EntityScript edited_entity;
    private GameObject edited_entity_icon;

    void Start()
    {
        inputField.onEndEdit.AddListener(delegate { OnEndEdit(); }); //add listen to event

    }

    //create a new blank sprite when new button is clicked and adds it to the visual list
    public void NewEntity()
    {
        EntityIcon.Create();

    }

    //event handling method
    //prev: previous key of the sprite
    //new: new key of the sprite ///TODO CHANGE FUNCTION NAME TO: CHANGEVALUENAMEINDATABASE
    public void SaveToDatabase(string prev, string _new)
    {
        if (prev == null)
        {
            Global.entity_database.Add(_new, new EntityScript(Global.default_entity_code, _new));
        }
        else
        {
            EntityScript tmp;
            if (Global.entity_database.TryGetValue(prev, out tmp))
            {
                Global.entity_database.Remove(prev); //create a new entity from existing code
                Global.entity_database.Add(_new, new EntityScript(tmp.code, _new, prev));
            }
        }

    }

    //event handling method when icon clicked
    public void SelectEntity()
    {
        edited_entity_icon= EventSystem.current.currentSelectedGameObject;
        InputField name_field = edited_entity_icon.transform.GetChild(0).GetComponent<InputField>();

        string ent_name = name_field.text;

        Global.entity_database.TryGetValue(ent_name, out edited_entity);

        ///UPDATE THE CONSOLE
        console.SetActive(true);

        //set text of the console to code of the clicked object
        inputField.text = edited_entity.code;
        inputField.GetComponent<ColorCoder>().ColorCode();
    }

    public void OnEndEdit()
    {
        string old_name = edited_entity.name;

        //get new name if changed via code
        int index = inputField.text.IndexOf("=");
        string new_name = inputField.text.Substring(0, index);
        new_name = new_name.Replace(" ", "");//remove white space

       
        EntityScript ent;
        if (!Global.entity_database.TryGetValue(new_name, out ent))
        {//change icon text with that new name
            InputField name_field = edited_entity_icon.transform.GetChild(0).GetComponent<InputField>();
            name_field.text = new_name;
            ent = new EntityScript(inputField.text, new_name, new_name);
            
            if (ent.table != null) //TODO:MORE CHECKs IF VALID (eg name must match table name)
            {
                Global.entity_database.Remove(old_name);
                Global.entity_database.Add(new_name, ent);
            }
        }
        else//make a new entity using old name
        {
            ent = new EntityScript(inputField.text, old_name, new_name);

            if (ent.table != null) //TODO:MORE CHECKs IF VALID (eg name must match table name)
            {
                Global.entity_database.Remove(old_name);
                Global.entity_database[old_name] = ent;
            }
        }

      }


}
