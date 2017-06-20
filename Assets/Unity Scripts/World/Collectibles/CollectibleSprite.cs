using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSprite : Collectible
{
    //Init properties
    private static GameObject prefab;
   
    //
    
    Texture2D texture2d; //to add to the entity_database

    protected override void Collected()
    {
        Texture2D tex;
        if (!Global.sprite_database.TryGetValue(name, out tex))
        {
            SpriteEditorController.New(name, texture2d);
            base.Collected(); //particle effect, sound, and destroy
        }
        else if (tex.Equals(Instantiate<Texture2D>(texture2d)))
        {
            SpriteEditorController.New(name, texture2d);
            base.Collected(); //particle effect, sound, and destroy
        }
    }


    public static GameObject Create(Vector2 pos, Texture2D tex2d)//custom init
    {
        #region init statics
        if (collectiblesController == null)
            collectiblesController = GameObject.Find("CollectiblesController");

        if (prefab == null)
            prefab = Resources.Load("World/Collectibles/CollectibleSprite") as GameObject;

        #endregion

        //create from pool
        GameObject newObject = ObjectPoolManager.CreatePooled(prefab, Vector3.zero, Quaternion.Euler(0, 0, 0));
        //assign correct name
        newObject.name = tex2d.name;
        //assign parent
        newObject.transform.SetParent(collectiblesController.transform);

        //access main component
        CollectibleSprite collectibleSprite = newObject.GetComponent<CollectibleSprite>();

        //init main comp properties
        collectibleSprite.texture2d = tex2d;

        collectibleSprite.start_pos = pos;
        collectibleSprite.name = tex2d.name;

        //call start method with correct properties
        newObject.SendMessage("Start", SendMessageOptions.DontRequireReceiver);

        return newObject;//return object after params are init
    }


}
