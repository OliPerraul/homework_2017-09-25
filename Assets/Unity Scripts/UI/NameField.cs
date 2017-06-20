using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NameField : InputField
{
    protected string previous_name;//used to access old location in dictionary

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
                
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //save previous name on focus
        base.OnPointerDown(eventData);

        if (!isFocused)
        {
            previous_name = text;
        }

    }

    protected virtual void SaveName<TVal>(string _default, Dictionary<string, TVal> database)
    {
        string new_name = text;

        //if not equal the name saved when first focused
        if (!new_name.Equals(previous_name))
        {

            if (text == "")
                text = AdjustNameWithIndex(_default, database);
            else
                text = AdjustNameWithIndex(text, database);

            //Change name im dic
            EditorController.ChangeNameInDatabase(previous_name, text, database);
        }

    }
    
    //Check if existing
    public static string AdjustNameWithIndex<TVal>(string name, Dictionary<string, TVal> database)
    {
        TVal val;

        if (database.TryGetValue(name, out val)) //check if name exists
        {
            int index = 0;

            foreach (KeyValuePair<string, TVal> entry in database)
            {
                if (entry.Key.StartsWith(name))
                    index++;
            }

            return AdjustNameWithIndex<TVal>(name + index, database);

        }
        else
        {
            return name;
        }

    }
    
}
