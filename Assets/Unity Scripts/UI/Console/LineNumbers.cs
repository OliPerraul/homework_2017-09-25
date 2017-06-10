using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineNumbers : MonoBehaviour
{
    //ref to parent (inp field)
    InputField inputField;
    Text lineNumbers; 
    
    

	// Use this for initialization
	void Start ()
    {
        inputField = GetComponentInParent<InputField>();
        inputField.onValueChanged.AddListener(delegate { AdjustLineNumbers(); }); //add listen to event

        //retrieve lineNumbers text in child
        lineNumbers = transform.GetComponentInChildren<Text>();
       
    }
	
	
    //update line numbers ()
   public void AdjustLineNumbers() //temporaiy reutnr number of\n
    {
        //string of the inputfield to analyse
        string text = inputField.text;
        CharEnumerator it = text.GetEnumerator(); //iterator that iterates throughevery characters in the string
        int count = 0; //counts lines

        while (it.MoveNext())
        {
           char curr_char = it.Current;
           if (curr_char == '\n')//check for single \n
           count++;

        }

        //adjust the numbers
        lineNumbers.text = "";//reset
        for (int i = 0; i <= count; i++)
        {
            lineNumbers.text += i + "\n";
        }

          
    }

}
