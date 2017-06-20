using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleEntity: Collectible
{
    //Init properties
    private static GameObject prefab;


    //

    EntityScript entity; //to add to the entity_database

        
    protected override void Collected()
    {
        EntityScript ent;
        if (!Global.entity_database.TryGetValue(name, out ent))
        {
           EntityEditorController.New(name, entity);
            base.Collected(); //particle effect, sound, and destroy
        }
        else if (ent.code.Equals(entity.code))
        {
            EntityEditorController.New(name, entity);
            base.Collected(); //particle effect, sound, and destroy
        }
    }
    

    public static GameObject Create(Vector2 pos, EntityScript _entity)//custom init
    {
        #region init statics
        if (collectiblesController == null)
            collectiblesController = GameObject.Find("CollectiblesController");

        if (prefab == null)
            prefab = Resources.Load("World/Collectibles/CollectibleEntity") as GameObject;
        
        #endregion

        //create from pool
        GameObject newObject = ObjectPoolManager.CreatePooled(prefab, Vector3.zero, Quaternion.Euler(0, 0, 0));
        //assign correct name
        newObject.name = _entity.name;
        //assign parent
        newObject.transform.SetParent(collectiblesController.transform);
               
        //access main component
        CollectibleEntity collectibleEntity = newObject.GetComponent<CollectibleEntity>();
    
        //init main comp properties
        collectibleEntity.entity = _entity;

        collectibleEntity.start_pos = pos;
        collectibleEntity.name = _entity.name;

              
        //call start method with correct properties
        newObject.SendMessage("Start", SendMessageOptions.DontRequireReceiver);

        return newObject;//return object after params are init
    }


}
