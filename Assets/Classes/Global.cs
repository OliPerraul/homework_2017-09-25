using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public static class Global
{
    //Pixel per unit
    public static int PPU = 16;


    //
    public static bool FLAG = false;


    public static Dictionary<string, Texture2D> sprite_database = new Dictionary<string, Texture2D>();
    public static Dictionary<string, EntityScript> entity_database = new Dictionary<string, EntityScript>();

    //TODO INIT GLOBALS (GOBJ)
    public static Script main_script;
    public static Table world; 
    public static List<int> world_added = new List<int>();

    public static Texture2D sprite_default = Resources.Load<Texture2D>("Sprites/UI/spr_default");
    public static string default_entity_code =
@"= 
{
sprite = """",
type = """",
x = getmousex(),
y = getmousey(),

-- Use this for initialization    
start = function(self)
end,

-- Update is called once per frame
update = function(self)
end,

}";




}