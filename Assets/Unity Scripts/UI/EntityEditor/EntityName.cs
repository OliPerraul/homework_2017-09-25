using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityName : MonoBehaviour
{
    InputField inputField;
    string previous_name;//used to access old location in dictionary
    public string name { get { return previous_name; } }


    EntityEditorController entityEditorCtrl;


    // Use this for initialization
    void Start()
    {
        inputField = GetComponent<InputField>();

        inputField.onEndEdit.AddListener(delegate { SaveName(); }); //add listen to event

        entityEditorCtrl = GameObject.Find("EntityEditorController").GetComponent<EntityEditorController>();

        SaveName();

    }
    
    void SaveName()
    {

        string name;

        if (inputField.text == "")
            inputField.text = AdjustNameWithIndex("obj_new");
        else
            inputField.text = AdjustNameWithIndex(inputField.text);


        name = inputField.text;

        //Add to dictionary    
        entityEditorCtrl.SaveToDatabase(previous_name, name);

        previous_name = name;//set previous to curr

    }
    
    string AdjustNameWithIndex(string name)
    {
        EntityScript ent;
        if (Global.entity_database.TryGetValue(name, out ent) && !name.Equals(previous_name)) //check if name exists
        {
            int index = 0;

            foreach (KeyValuePair<string, EntityScript> entry in Global.entity_database)
            {
                if (entry.Key.StartsWith(name))
                    index++;
            }

            return AdjustNameWithIndex(name + index);

        }
        else
        {
            return name;
        }



    }
}
