using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteDimension : MonoBehaviour {

    private InputField xField;
    private int width;

    private InputField yField;
    private int height;
    

    // Use this for initialization
    void Start ()
    {
        xField = transform.GetChild(0).GetComponent<InputField>();
        yField = transform.GetChild(2).GetComponent<InputField>();
        
        xField.onEndEdit.AddListener(delegate { SaveSpriteDimensions(); }); //add listen to event
        yField.onEndEdit.AddListener(delegate { SaveSpriteDimensions(); }); //add listen to event

    }
	
	
    void SaveSpriteDimensions()
    {
        //adjust content of the fields

        if (xField.Equals(""))
            xField.text = "1";

        if (yField.Equals(""))
            yField.text = "1";

        width = Mathf.RoundToInt(Mathf.Clamp(int.Parse(xField.text), 1, 64));
        height = Mathf.RoundToInt(Mathf.Clamp(int.Parse(yField.text), 1, 64));

        xField.text = width.ToString();
        yField.text = height.ToString();

        SpriteIcon sprIc = transform.parent.GetComponent<SpriteIcon>();
   
        SpriteEditorController.SaveDimensions(sprIc.sprite_name, width, height);
        
    }
}
