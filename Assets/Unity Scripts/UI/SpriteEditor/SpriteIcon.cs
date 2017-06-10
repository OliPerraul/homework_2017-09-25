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

    static SpriteEditorController spriteEditorCtrl;

    public Sprite sprite;
    private Button button;
    private InputField inputField;

    void Start()
    {
        //add listener to on click event
        button.onClick.AddListener(delegate { spriteEditorCtrl.SelectSprite(); });
        button.onClick.AddListener(delegate { SetSelectedOnClicked(); });
    }

    //THIS IS NOT THE ONE ASSOCIATED WITH THE DATABASE
    void SetTempIconSprite(Texture2D tex)
    {
        sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), Global.PPU);

        button.image.sprite = sprite;
    }

    void Update()
    {
        //set icon image on the fly
        if (selected.Equals(gameObject.name))
        {
            Texture2D tex;
            if (Global.sprite_database.TryGetValue(inputField.text, out tex))
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
    

    public static GameObject Create()//custom init
    {
        #region Init Statics
        if (spriteIcons == null)
            spriteIcons = GameObject.Find("SpriteIcons");
        if (prefab == null)
            prefab = Resources.Load("UI/SpriteIcon") as GameObject;
        if (spriteEditorCtrl == null)
            spriteEditorCtrl = GameObject.Find("SpriteEditorController").GetComponent<SpriteEditorController>();

        #endregion

        GameObject newObject = Instantiate(prefab, spriteIcons.transform) as GameObject;

        //assign correct name
        newObject.name = prefab.name + spriteIcons.transform.childCount;
                
        //access main component
        SpriteIcon spriteIcon = newObject.GetComponent<SpriteIcon>();

        #region init instance properties
        spriteIcon.button = spriteIcon.GetComponent<Button>();
        spriteIcon.inputField = spriteIcon.GetComponentInChildren<InputField>();
        //..
        //..
        #endregion

        spriteIcon.SetTempIconSprite(Instantiate<Texture2D>(Global.sprite_default));

        return newObject;
    }


}
