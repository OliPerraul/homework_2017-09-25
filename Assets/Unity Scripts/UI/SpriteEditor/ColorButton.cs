using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour {

    [HideInInspector]
    public Button button;
    [HideInInspector]
    public Image image;

    static Brush brush;
    
	// Use this for initialization
	void Start ()
    {
        button = GetComponent<Button>();//get button
        image = GetComponent<Image>();//get button

        if (brush == null)              //get brush
            brush = GameObject.Find("Brush").GetComponent<Brush>();

        //listen to own onclick
        button.onClick.AddListener(delegate { ChangeBrushColor(); });
    }

    void ChangeBrushColor()
    {
        brush._Color = image.color;
    }
    
    
}
