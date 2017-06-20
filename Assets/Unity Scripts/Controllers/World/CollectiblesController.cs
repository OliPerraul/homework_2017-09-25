using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DropProperties
{
    public float drop_rate_entities;
    public float drop_rate_sprites;
    public float drop_rate_health;

    public int amount_entities;
    public int amount_sprites;
    public int amount_health;
}

public class CollectiblesController : MonoBehaviour
{
    //used to prevent excessive duplicates
    public static List<string> dropped_entities = new List<string>();
    public static List<string> dropped_sprites = new List<string>();
    //    public static List<CollectibleHealth> col_healths = new List<CollectibleHealth>();



    //Pos near to drop
    //amount of objects to drop
    //change to drop (0-1)
    public static void DropRandom(Vector2 pos, DropProperties dropProp)
    {
        for (int i = 0; i < dropProp.amount_entities + 1; i++)//ammount chances to drop something
        {
            float condition = Random.Range(0f, 1f);

            if (condition <= dropProp.drop_rate_entities)
            {
                Vector2 near_pos = FindNearPos(pos, new Vector2(-1, 1), new Vector2(-1, 1));
                DropEntity(near_pos);
            }
        }

        for (int i = 0; i < dropProp.amount_sprites + 1; i++)
        {
            float condition = Random.Range(0f, 1f);

            if (condition <= dropProp.drop_rate_sprites)
            {
                Vector2 near_pos = FindNearPos(pos, new Vector2(-1, 1), new Vector2(-1, 1));
                DropSprite(near_pos);
            }
        }

        for (int i = 0; i < dropProp.amount_health + 1; i++)
        {
            float condition = Random.Range(0f, 1f);

            if (condition <= dropProp.drop_rate_health)
            {
                Vector2 near_pos = FindNearPos(pos, new Vector2(-1, 1), new Vector2(-1, 1));
                DropHealth(near_pos, CollectibleHealth.Sizes.NORMAL);
            }
        }


    }

    static Vector2 FindNearPos(Vector2 pos, Vector2 x_range, Vector2 y_range)
    {
        Vector2 near_pos;
        near_pos.x = pos.x + Random.Range(x_range.x, x_range.y);
        near_pos.y = pos.y + Random.Range(y_range.x, y_range.y);
        
        return near_pos;
    }


    //ENTITY
    static void DropEntity(Vector2 pos)
    {
        bool dropped = false;

        while (!dropped)
        {
            //Add entities to global environment
            foreach (KeyValuePair<string, EntityScript> entry in Global.template_entity_database)
            {
                float condition = Random.Range(0f, 1f);

                if (condition > .5f)
                {
                    CollectibleEntity.Create(pos, entry.Value);
                    dropped_entities.Add(entry.Key);
                    dropped = true;
                }

                if (dropped)
                    break;
            }
        }
    }

    static void DropEntity(Vector2 pos, params string[] drops)
    {
        foreach (string drop in drops)
        {
            EntityScript entity;

            Global.template_entity_database.TryGetValue(drop,out entity);
            CollectibleEntity.Create(pos, entity);
            dropped_entities.Add(drop);//add name to the list
        }
    }

    //SPRITE
    static void DropSprite(Vector2 pos)
    {
        bool dropped = false;

        while (!dropped)
        {
            //Add entities to global environment
            foreach (KeyValuePair<string, Texture2D> entry in Global.template_sprite_database)
            {
                float condition = Random.Range(0f, 1f);

                if (condition > .5f)
                {
                    CollectibleSprite.Create(pos, entry.Value);
                    dropped_entities.Add(entry.Key);
                    dropped = true;
                }

                if (dropped)
                    break;
            }
        }
    }

    static void DropSprite(Vector2 pos, params string[] drops)
    {
        foreach (string drop in drops)
        {
            Texture2D tex;

            Global.template_sprite_database.TryGetValue(drop, out tex);
            CollectibleSprite.Create(pos, tex);
            dropped_sprites.Add(drop);//add name to the list
        }
    }


    //SPRITE
    static void DropHealth(Vector2 pos, CollectibleHealth.Sizes size)
    {
       CollectibleHealth.Create(pos, size);
          
    }


    //void DropHealths(Vector2 pos, params int[] drops)
    //{ //int amount to heal
    //    foreach (int drop in drops)
    //    {
    //        //CollectibleHealth.Create(drop);
    //    }
    //}

}
