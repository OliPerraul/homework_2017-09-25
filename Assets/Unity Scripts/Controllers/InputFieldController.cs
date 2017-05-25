using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldController : MonoBehaviour {

    private InputField inputField;

    [SerializeField]
    private Text content;


    public delegate void InputFieldSubmittedEventHandler(string content);
    public event InputFieldSubmittedEventHandler InputFieldSubmitted; //event

    
    // Use this for initialization
    void Start ()
    {
        inputField = GetComponent<InputField>();

        inputField.onValidateInput += RestrictNewLine; //add listen to event              
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (CheckSubmit()) //calls the method that notifies the subs
        {
            OnSubmitted();
        }
        
    }//end


       
    //Check if submit is asked
    bool CheckSubmit()
    {
        //get key ctrl, and keydown return
        if ((Input.GetKeyDown(KeyCode.Return) && Input.GetKey(KeyCode.LeftControl))
         || (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Return)))
        {
            //TODO: check if empty, clear out a portuin (the main)
             return true;
        }

        return false;
    }//end


    //Event publisher method
    protected virtual void OnSubmitted()
    {
        if (InputFieldSubmitted != null)
        {
            //broadcast event
            InputFieldSubmitted(inputField.text);
        }
              
    }//end
    
    
    char RestrictNewLine(string text, int char_index, char added_char)
    {

        //get key ctrl, and keydown return
        if ((Input.GetKeyDown(KeyCode.Return) && Input.GetKey(KeyCode.LeftControl))
         || (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Return)))
        {
            added_char = '\0';
           
        }
        
        return added_char;
     }//end
    

}
