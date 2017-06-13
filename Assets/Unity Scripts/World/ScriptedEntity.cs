using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System;

public class ScriptedEntity : MonoBehaviour
{
        
    #region statics
    static GameObject scriptedEntityController;
    static GameObject prefab;
    static Camera cam; //ref to camera
    static GameObject player;
    #endregion

    #region Entity properties

    //Dictionary Items
    private Closure entity_update { get; set; }
    private Closure entity_start { get; set; } //fired once when object starts
    public Nullable<double> x = 0;
    public Nullable<double> y = 0;
    public Nullable<int> layer = null;
    public Nullable<double> hbwidth = null;
    public Nullable<double> hbheight = null;
    public Nullable<double> hboffsetx = null;
    public Nullable<double> hboffsety = null;
    public Nullable<double> originx = null;
    public Nullable<double> originy = null;
    public Nullable<bool> visible = null;
    public Texture2D sprite = null;
    public string type = null;

    #endregion


    //Components
    SpriteRenderer spriteRend;
    BoxCollider2D box2d;

    //Main table
    private Table entity_table;

    //spawn
    private Vector3 spawn_pos; //final spawn pos

    public enum SPAWN_TYPES
    {
        TYPE_SCREEN_CENTER,
        TYPE_MOUSE,
        TYPE_PLAYER,

    }

    public SPAWN_TYPES spawn_type;
    

    // Use this for initialization
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        box2d = GetComponent<BoxCollider2D>();
        
        //DetermineSpawnPos(spawn_type);

        //assign dictionary items to instance properties     
        AssignBuiltInVars();

        ////////////call the start function/////////////
        entity_start.Call(entity_table);

        //assign dictionary items to instance properties     
        AssignBuiltInVars();

        //Behave according to Builtin vars
        ApplyBehaviour();

    }

 
    // Update is called once per frame
    void Update()
    {
        /////////////////call update function//////////////////
        entity_update.Call(entity_table);
        ////////////////////////////////////

        //assign dictionary items to instance properties     
        AssignBuiltInVars();

        //Behave according to Builtin vars
        ApplyBehaviour();

    }

    void AssignBuiltInVars()
    {
        Debug.Log("");

        //assign vars contained in table to class vars (for certain cases--not player defined vars)
        DynValue val = new DynValue();

        if (TryGetValue(entity_table, "update", out val))
            entity_update = val.Function;

        if (TryGetValue(entity_table, "start", out val))
            entity_start = val.Function;

        if (TryGetValue(entity_table, "x", out val))
            x = val.Number;

        if (TryGetValue(entity_table, "y", out val))
            y = val.Number;
        
        if (TryGetValue(entity_table, "layer", out val))
            layer = (int) val.Number;

        if (TryGetValue(entity_table, "hbwidth", out val))
            hbwidth = val.Number;

        if (TryGetValue(entity_table, "hbheight", out val))
            hbheight = val.Number;

        if (TryGetValue(entity_table, "hboffsetx", out val))
            hboffsetx = val.Number;

        if (TryGetValue(entity_table, "hboffsety", out val))
            hboffsety= val.Number;

        if (TryGetValue(entity_table, "originx", out val))
            originx = val.Number;

        if (TryGetValue(entity_table, "originy", out val))
            originy = val.Number;

        if (TryGetValue(entity_table, "visible", out val))
            visible = val.Boolean;

        if (TryGetValue(entity_table, "sprite", out val))
        {
            Texture2D tex;
            if (Global.sprite_database.TryGetValue(val.String, out tex))
            {
                sprite = tex;
            }
           
        }

        if (TryGetValue(entity_table, "type", out val))
            type = val.String;
        

    }

    //helper method
    static bool TryGetValue(Table table, string key, out DynValue val)
    {
        val = table.Get(key);

        if (val == null || val.IsNilOrNan())
            return false;
        else
            return true;

    }


    #region Entity Behaviours
    //apply defined behaviour
    void ApplyBehaviour()
    {
        B_Origin();
        B_Position();
        B_Sprite();
        B_Hitbox();
        B_Type();

    }

    void B_Origin()
    {
        //GUI.DrawTexture()

    }
    
    void B_Position()
    {
        //update pos
        Vector3 pos = new Vector3((float)x, (float)y, 0);
        transform.position = spawn_pos + pos;

    }

    void B_Sprite()
    {
        //update sprite
        if (sprite != null)
            spriteRend.sprite = Sprite.Create(sprite, new Rect(0f, 0f, sprite.width, sprite.height), new Vector2(0.5f, 0.5f), Global.PPU);
        else
        {
            Texture2D tex = new Texture2D(16, 16);

            spriteRend.sprite = Sprite.Create(tex, new Rect(0f, 0f, sprite.width, sprite.height), new Vector2(0.5f, 0.5f), Global.PPU);
        }


    }


    void B_Hitbox()
    {
        float width = ((float)sprite.width)/Global.PPU;
        float height = ((float)sprite.height) / Global.PPU;
        float offset_x = 0;
        float offset_y = 0;

        if (hbwidth != null)
            width = (float)(hbwidth) / Global.PPU;

        if (hbwidth != null)
            height = (float)(hbheight) / Global.PPU;

        Vector2 dims = new Vector2(width, height);
        //adjust hitbox
        box2d.size = dims;

        if (hboffsetx != null)
            offset_x = (float)(hboffsetx) / Global.PPU;

        if (hboffsety != null)
            offset_y = (float)(hboffsety) / Global.PPU;

        Vector2 offsets = new Vector2(offset_x, offset_y);

        box2d.offset = offsets;

    }


    void B_Type()
    {
        if (type != null)
        {
            switch (type)
            {
                case "solid":

                    break;


                case "bullet":
                    box2d.isTrigger = true;
                    

                    break;


                case null:
                    box2d.isTrigger = true;
                    break;
            }
            
        }

    }

    #endregion

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (type)
        {
            case "bullet":
                if(other.tag != "ScriptedEntity")
                ObjectPoolManager.DestroyPooled(gameObject);

                break;


        }


    }


    public static GameObject Create(Table entity_table, SPAWN_TYPES spawn_type)//custom init
    {
        #region init statics
        if (scriptedEntityController == null)
            scriptedEntityController = GameObject.Find("ScriptedEntityController");

        if (prefab == null)
            prefab = Resources.Load("World/ScriptedEntity") as GameObject;

        if (cam == null)
            cam = Camera.main;

        if (player == null)
            player = GameObject.Find("Player");
        #endregion
        
        //create from pool
        GameObject newObject = ObjectPoolManager.CreatePooled(prefab, Vector3.zero, Quaternion.Euler(0, 0, 0));
        //assign correct name
        newObject.name = prefab.name;
        //assign parent
        newObject.transform.SetParent(scriptedEntityController.transform);
        //access main component
        ScriptedEntity scriptedEntity = newObject.GetComponent<ScriptedEntity>(); 
        //init main comp properties
        scriptedEntity.spawn_type = spawn_type;//set spawn type
        scriptedEntity.entity_table = entity_table;//set the entity table
        //call start method with correct properties
        newObject.SendMessage("Start", SendMessageOptions.DontRequireReceiver);

        return newObject;//return object after params are init
    }
    
    

}
