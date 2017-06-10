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

    #region Dictionary lookup
    static Dictionary<int, string> builtIn_vars_lookup = new Dictionary<int, string>
    {
        { 1, "\"update\"" },
        { 2, "\"start\"" },
        { 3, "\"x\"" },
        { 4, "\"y\"" },
        { 5, "\"collidable\"" },
        { 6, "\"height\"" },
        { 7, "\"width\"" },
        { 8, "\"layer\"" },
        { 9, "\"left\"" },
        { 10, "\"right\"" },
        { 11, "\"top\"" },
        { 12, "\"bottom\"" },
        { 13, "\"originX\"" },
        { 14, "\"originY\"" },
        { 15, "\"type\"" },
        { 16, "\"visible\"" },

    };


    #endregion

    #region Entity properties

    //Dictionary Items
    private Closure entity_update { get; set; }
    private Closure entity_start { get; set; } //fired once when object starts
    public double x = 0;
    public double y = 0;
    public double centerX { get; set; }
    public double centerY { get; set; }
    public bool collidable { get; set; }
    public double height { get; set; }
    public double width { get; set; }
    public double layer { get; set; }
    public double left { get; set; }
    public double right { get; set; }
    public double top { get; set; }
    public double bottom { get; set; }
    public double originX { get; set; }
    public double originY { get; set; }
    public string type { get; set; }
    public bool visible { get; set; }
    public Texture2D sprite { get; set; }

    #endregion


    //Components
    SpriteRenderer spriteRend;

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

    private SPAWN_TYPES spawn_type;



    // Use this for initialization
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();


        DetermineSpawnPos(spawn_type);

        //assign dictionary items to instance properties     
        AssignBuiltInVars();

        ////////////call the start function/////////////
        entity_start.Call(entity_table);

        //assign dictionary items to instance properties     
        AssignBuiltInVars();

        //Behave according to Builtin vars
        ApplyBehaviour();

    }

    void DetermineSpawnPos(SPAWN_TYPES _spawn_type)
    {

        switch (_spawn_type)
        {
            case SPAWN_TYPES.TYPE_SCREEN_CENTER:
                spawn_pos = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, cam.nearClipPlane));
                break;
            case SPAWN_TYPES.TYPE_MOUSE:
                spawn_pos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
                break;
            case SPAWN_TYPES.TYPE_PLAYER:
                player = GameObject.Find("Player");
                spawn_pos = player.transform.position;
                break;
        }
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

        if (TryGetValue(entity_table, "centerX", out val))
            centerX = val.Number;

        if (TryGetValue(entity_table, "centerY", out val))
            centerY = val.Number;

        if (TryGetValue(entity_table, "collidable", out val))
            collidable = val.Boolean;

        if (TryGetValue(entity_table, "height", out val))
            height = val.Number;

        if (TryGetValue(entity_table, "width", out val))
            width = val.Number;

        if (TryGetValue(entity_table, "layer", out val))
            layer = val.Number;

        if (TryGetValue(entity_table, "left", out val))
            left = val.Number;

        if (TryGetValue(entity_table, "right", out val))
            right = val.Number;

        if (TryGetValue(entity_table, "top", out val))
            top = val.Number;

        if (TryGetValue(entity_table, "bottom", out val))
            bottom = val.Number;

        if (TryGetValue(entity_table, "originX", out val))
            originX = val.Number;

        if (TryGetValue(entity_table, "originY", out val))
            originY = val.Number;

        if (TryGetValue(entity_table, "type", out val))
            type = val.String;

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

    //apply defined behaviour
    void ApplyBehaviour()
    {
        B_Origin();
        B_Position();
        B_Sprite();
        B_Hitbox();

    }

    void B_Origin()
    {
        //GUI.DrawTexture()

    }
    
    void B_Position()
    {
        //update pos
        Vector3 pos = new Vector3((float)x, (float)y);
        transform.position = spawn_pos + pos;

    }

    void B_Sprite()
    {
        //update sprite
        if(sprite != null)
        spriteRend.sprite = Sprite.Create(sprite, new Rect(0f, 0f, sprite.width, sprite.height), new Vector2(0.5f, 0.5f), Global.PPU);

    }

    void B_Hitbox()
    {
        //GUI.DrawTexture()

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
