using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    private Texture2D tex2d;

    private Sprite sprite;

    private SpriteRenderer spriteRend;

    #region Brush properties

    [SerializeField]
    private Color color = Color.red;

    public Color _Color
    {
        get { return color; }
        set
        {
            if (color != null)
                old_color = color;
            color = value;
        }
    }
    private Color old_color = Color.red;

    [SerializeField]
    private int size;

    #endregion

    // Use this for initialization
    void Start()
    {
        _Color = Color.blue;

        //default texture
        tex2d = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        tex2d.filterMode = FilterMode.Point;
             
        // Get renderer you want to probe
        spriteRend = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
      
        SnapToPixelGrid();

        DisplayBrushTip();

       // Vector3 pos = transform.position;
       // transform.position = new Vector3(pos.x, pos.y, 0);
    }



    void SnapToPixelGrid()
    {
        Vector2 mouse_pos = Input.mousePosition; // Mouse position

        Camera cam = Camera.main; // Camera to use for raycasting

        //get positionnof the pixel clicked
        // assumes an orthographic camera
        var pos = cam.ScreenToWorldPoint(mouse_pos);
        pos.z = 0; //kill z;

        //snap
        //rounds to nearest multiple of .0625 (1/PPU)
        pos.x = MathUtils.mRound(pos.x, 1f/Global.PPU);
        pos.y = MathUtils.mRound(pos.y, 1f/Global.PPU);

        transform.position = pos;

    }

    
    void DisplayBrushTip()
    {
        if ((color != old_color) ||  sprite == null)//if color is changed, change sprite
        {
            tex2d.SetPixel(0, 0, color);
            tex2d.Apply();

            sprite = Sprite.Create(tex2d, new Rect(0f, 0f, tex2d.width, tex2d.height), Vector2.zero, Global.PPU);

            spriteRend.sprite = sprite;
        }

    
    }

}
