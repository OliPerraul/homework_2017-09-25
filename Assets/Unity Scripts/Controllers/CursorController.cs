using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{


    [SerializeField]
    private Texture2D tex_cursorSelect;

    [SerializeField]
    private Texture2D tex_cursorDefault;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        ChangeCursor(StandaloneInputModuleMod.GetPointerEventData(-1));

	}


    void ChangeCursor(PointerEventData eventData)
    {
      
        List<GameObject> gobjs = eventData.hovered;

        foreach (GameObject gobj in gobjs)
        {
            if (gobj.GetComponent<Button>() != null)
            {
                Cursor.SetCursor(tex_cursorSelect, Vector2.zero, CursorMode.Auto);
                return;
            }
        }
        Cursor.SetCursor(tex_cursorDefault, Vector2.zero, CursorMode.Auto);

    }



}
