using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteEditorController : MonoBehaviour
{

    private PixelCanvas pixelCanvas;

      
    // Use this for initialization
    void Start()
    {
        pixelCanvas = GetComponent<PixelCanvas>();
    }

    // Use this for initialization
    void Update()
    {
       // Debug.Log(Global.sprite_database.Count);
    }


    //create a new blank sprite when new button is clicked and adds it to the visual list
    public void NewSprite()
    {
        SpriteIcon.Create();
               
    }

    //event handling method
    //prev: previous key of the sprite
    //new: new key of the sprite
    public void SaveToDatabase(string prev, string _new)
    {
        if (prev == null)
        {
            Global.sprite_database.Add(_new, Instantiate<Texture2D>(Global.sprite_default));
        }
        else
        {
            Texture2D tmp;
            Global.sprite_database.TryGetValue(prev, out tmp);
            Global.sprite_database.Remove(prev);

            Global.sprite_database.Add(_new, tmp);
        }
                   
    }
    
    //event handling method
    //key
    //width, height
    public void SaveDimensions(string spr_name, int width, int height)
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
    public void SelectSprite()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        InputField inpF =  go.transform.GetChild(0).GetComponent<InputField>();

        string spr_name = inpF.text;

        Texture2D tex;
        Global.sprite_database.TryGetValue(spr_name, out tex);


        pixelCanvas.SetCurrEditedTex(tex);
        pixelCanvas.AdjustColliderDimensions();

       
    }



}
