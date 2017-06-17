using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteName : MonoBehaviour
{
    InputField inputField;
    string previous_name;//used to access old location in dictionary
    public string name { get { return previous_name; } }


    SpriteEditorController spriteEditorCtrl;
   

    // Use this for initialization
    void Start ()
    {
        inputField = GetComponent<InputField>();
        
        inputField.onEndEdit.AddListener(delegate { SaveSpriteName(); }); //add listen to event

        spriteEditorCtrl = GameObject.Find("SpriteEditorController").GetComponent<SpriteEditorController>();

        SaveSpriteName();

    }
	

    void SaveSpriteName()
    {
                
        string name;

        if(inputField.text == "")
            inputField.text = AdjustNameWithIndex("spr_new");
        else
            inputField.text = AdjustNameWithIndex(inputField.text);


        name = inputField.text;

        //Add to dictionary    
        spriteEditorCtrl.SaveToDatabase(previous_name, name);

        previous_name = name;//set previous to curr
        
    
    }
    

    string AdjustNameWithIndex(string name)
    {
        Texture2D tex;

        if (Global.sprite_database.TryGetValue(name, out tex) && !name.Equals(previous_name)) //check if name exists
        {
            int index = 0;

            foreach (KeyValuePair<string, Texture2D> entry in Global.sprite_database)
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
