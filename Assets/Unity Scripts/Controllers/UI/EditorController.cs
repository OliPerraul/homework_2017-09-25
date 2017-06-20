using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorController : MonoBehaviour {

    private static PixelCanvas pixelCanvas;
    [SerializeField]
    protected Button butt_new;
    [SerializeField]
    protected Button butt_delete;
    

    //event handling method
    //prev: previous key of the sprite
    //new: new key of the sprite
    public static void ChangeNameInDatabase<TVal>(string prev, string _new, Dictionary<string,TVal> database)///CHANGE DATABASE VALUE
    {
        TVal tmp;
        database.TryGetValue(prev, out tmp);

        database.Remove(prev);
        database.Add(_new, tmp);
    }

 
}
