using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpriteIcon : MonoBehaviour
{
    static string selected = "";

    static GameObject spriteIcons;//container
    static GameObject prefab;

    public Sprite sprite;
    public string sprite_name;
    
    private Button button;
    private SpriteName spriteName;

    void Start()
    {
        //add listener to on click event
        button.onClick.AddListener(delegate { SpriteEditorController.SelectSprite(); });
        button.onClick.AddListener(delegate { SetSelectedOnClicked(); });
       // button.onClick.AddListener(delegate { spriteName.SaveSpriteName(); });
    }

    //THIS IS NOT THE ONE ASSOCIATED WITH THE DATABASE
    void SetTempIconSprite(Texture2D tex)
    {
        sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), Global.PPU);

        button.image.sprite = sprite;
    }

    void Update()
    {
     
            //set sprite name
            sprite_name = spriteName.text;

            //set icon image on the fly
            if (selected.Equals(gameObject.name))
            {
                Texture2D tex;
                if (Global.sprite_database.TryGetValue(spriteName.text, out tex))
                {
                    sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), Global.PPU);
                    button.image.sprite = sprite;
                }
            }
           
        
    }


    void SetSelectedOnClicked()
    {
        selected = gameObject.name;
    }
       

    public static void Create(string name, Texture2D tex2d)//custom init
    {
        #region Init Statics
        if (spriteIcons == null)
            spriteIcons = GameObject.Find("SpriteIcons");
        if (prefab == null)
            prefab = Resources.Load("UI/SpriteIcon") as GameObject;


        #endregion

        GameObject newObject = Instantiate(prefab, spriteIcons.transform) as GameObject;

        //assign correct name
        newObject.name = prefab.name + spriteIcons.transform.childCount;

        //access main component
        SpriteIcon spriteIcon = newObject.GetComponent<SpriteIcon>();

        #region init instance properties
        spriteIcon.button = spriteIcon.GetComponent<Button>();
        spriteIcon.spriteName = spriteIcon.GetComponentInChildren<SpriteName>();
        spriteIcon.SetTempIconSprite(tex2d);

        spriteIcon.spriteName.text = name;
        spriteIcon.sprite_name = name;


        //..
        //..
        #endregion
             
        InputField width_field = spriteIcon.transform.GetChild(1).GetChild(0).GetComponent<InputField>();
        InputField height_field = spriteIcon.transform.GetChild(1).GetChild(2).GetComponent<InputField>();

        width_field.text = tex2d.width.ToString();
        height_field.text = tex2d.height.ToString();


    }



}
