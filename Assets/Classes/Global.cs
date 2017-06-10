using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    //Pixel per unit
    public static int PPU = 16;

    public static Dictionary<string, Texture2D> sprite_database = new Dictionary<string, Texture2D>();
    public static Dictionary<string, EntityScript> entity_database = new Dictionary<string, EntityScript>();

    //TODO INIT GLOBALS (GOBJ)
    public static Texture2D sprite_default = Resources.Load<Texture2D>("Sprites/UI/spr_default");
    public static EntityScript entity_default;

    


}