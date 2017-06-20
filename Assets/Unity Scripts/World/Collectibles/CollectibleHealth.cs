using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHealth : Collectible
{
    //Init properties
    private static GameObject prefab;
    private static Player player;

    //
    public enum Sizes
    {
        SMALL,
        NORMAL,
        BIG
    }

    float HP;

    protected override void Collected()
    {
        base.Collected(); //particle effect, sound, and destroy

        //HEAL
        player.HP =Mathf.Clamp(player.HP+HP, 0f, 1f);
        
    }


    public static GameObject Create(Vector2 pos, Sizes size)//custom init
    {
        #region init statics
        if (collectiblesController == null)
            collectiblesController = GameObject.Find("CollectiblesController");

        if (player == null)
            player = GameObject.Find("Player").GetComponent<Player>();

        if (prefab == null)
            prefab = Resources.Load("World/Collectibles/CollectibleHealth") as GameObject;

        #endregion

        //create from pool
        GameObject newObject = ObjectPoolManager.CreatePooled(prefab, Vector3.zero, Quaternion.Euler(0, 0, 0));
        //assign correct name
        newObject.name = "health";
        //assign parent
        newObject.transform.SetParent(collectiblesController.transform);

        //access main component
        CollectibleHealth collectibleHealth = newObject.GetComponent<CollectibleHealth>();

        collectibleHealth.start_pos = pos;


        //init main comp properties
        switch (size) //TODO CHANGE SPRITE
        {
            case Sizes.SMALL:
                collectibleHealth.HP = .05f;
                break;

            case Sizes.NORMAL:
                collectibleHealth.HP = .10f;
                break;

            case Sizes.BIG:
                collectibleHealth.HP = .20f;
                break;
             
        }
        

        //call start method with correct properties
        newObject.SendMessage("Start", SendMessageOptions.DontRequireReceiver);

        return newObject;//return object after params are init
    }


}
