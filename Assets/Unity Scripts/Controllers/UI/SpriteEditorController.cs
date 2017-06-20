using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteEditorController : EditorController
{

    private static PixelCanvas pixelCanvas;

      
    // Use this for initialization
    void Start()
    {
        pixelCanvas = GetComponent<PixelCanvas>();
        
        //add listener to on click event
        butt_new.onClick.AddListener(delegate {New(); });

        //add listener to on click event
       // butt_delete.onClick.AddListener(delegate { SpriteEditorController.SelectSprite(); });

    }
     

    //create a new blank sprite when new button is clicked and adds it to the visual list
    public static void New()
    {
        string name = SpriteName.AdjustNameWithIndex("spr_new", Global.sprite_database);
        Texture2D tex = Instantiate<Texture2D>(Global.sprite_default);

        Global.sprite_database.Add(name, tex);

        SpriteIcon.Create(name, tex);
               
    }

    //TODO CREATE all icons FROM DATABAsE
    public static void New(string name, Texture2D tex)
    {
        name = SpriteName.AdjustNameWithIndex(name, Global.sprite_database);

        //add sprite to the database
        Global.sprite_database.Add(name, Instantiate<Texture2D>(tex));

        SpriteIcon.Create(name, tex);
       
    }

    
    //event handling method
    //key
    //width, height
    public static void SaveDimensions(string spr_name, int width, int height)
    {
        Texture2D tmp;
        Global.sprite_database.TryGetValue(spr_name, out tmp);

        var old_pixels = tmp.GetPixels32(); //ref to old pixels
      
        //Create a new array of pixels
        var new_pixels = new Color32[width * height];
        for (int i = 0; i < new_pixels.Length; i++)
        {
            try//if pixels match store old one: else clear
            {
                new_pixels[i] = old_pixels[i];
            }
            catch 
            {
                new_pixels[i] = Color.clear;
            }
        }

        tmp.Resize(width, height);
        tmp.SetPixels32(new_pixels);
        
        tmp.Apply();
          
    }

    //event handling method when icon clicked
    public static void SelectSprite()
    {
        
        GUI.FocusControl("");

        GameObject go = EventSystem.current.currentSelectedGameObject;
        SpriteName sprName =  go.transform.GetChild(0).GetComponent<SpriteName>();

        string spr_name = sprName.text;

        Texture2D tex;
        Global.sprite_database.TryGetValue(spr_name, out tex);
        
        pixelCanvas.SetCurrEditedTex(tex);
        pixelCanvas.AdjustColliderDimensions();
               
    }





}
