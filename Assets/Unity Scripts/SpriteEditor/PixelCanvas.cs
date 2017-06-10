using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelCanvas : MonoBehaviour
{
    [SerializeField]
    private int canvas_width = 128;
    [SerializeField]
    private int canvas_height = 128;
    public int Width { get { return spriteRend.sprite.texture.width; } }
    public int Height { get { return spriteRend.sprite.texture.height; } }

    [SerializeField]
    private Brush brush;

    private Texture2D tex2d;

    private Sprite sprite;
    public Sprite _Sprite
    {
        get { return sprite; }
        set { sprite = value; }

    }


    private SpriteRenderer spriteRend;


    //drawing
    private List<Vector2> points_registered;


    // Use this for initialization
    void Start ()
    {
        //default texture
        tex2d = new Texture2D(canvas_width, canvas_height, TextureFormat.RGBA32, false);
        tex2d.filterMode = FilterMode.Point;
        
        // Get renderer you want to probe
        spriteRend = GetComponent<SpriteRenderer>();

        //drawing
        points_registered = new List<Vector2>();


    }

	// Update is called once per frame
	void Update ()
    {
            ColorTexture();
        
            sprite = Sprite.Create(tex2d, new Rect(0f,0f,canvas_width,canvas_height), new Vector2(0.5f, 0.5f), Global.PPU);

            spriteRend.sprite = sprite;
       
    }


    public void SetCurrTex(Texture2D tex)
    {
        tex2d = tex;
        tex2d.filterMode = FilterMode.Point;

        canvas_width = tex2d.width;
        canvas_height = tex2d.height;
    }

 


    void ColorTexture()
    {
        Vector2 mouse_pos = Input.mousePosition; // Mouse position
       
        Camera cam = Camera.main; // Camera to use for raycasting

        Ray ray = cam.ScreenPointToRay(mouse_pos);
    
        RaycastHit2D hit;
        hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f);
           
             
        if ((hit.collider) && Input.GetMouseButton(0))
        {
            //get positionnof the pixel clicked
            // assumes an orthographic camera
            var pos = cam.ScreenToWorldPoint(mouse_pos);
            pos.z = transform.position.z;
            pos = transform.InverseTransformPoint(pos); //Transforms position from world space to local space.

            int xPixel = Mathf.RoundToInt(pos.x * Global.PPU);
            int yPixel = Mathf.RoundToInt(pos.y * Global.PPU);

            xPixel += canvas_width / 2;
            yPixel += canvas_height / 2;

             Vector2 point = new Vector2(xPixel, yPixel);
                             
            if (!points_registered.Contains(point))
                points_registered.Add(point);
                      

            //draw linear interpolation from points registered
             DrawPoints(points_registered);
                                 
        }

        //clear list of points registered on button release
        if (Input.GetMouseButtonUp(0))
        { 
            points_registered.Clear();
        }
       
    }

    //helper method of above
     void DrawPoints(List<Vector2> points_registered)
    {
        Vector2 point_between = points_registered[0];

        //first pixel
        tex2d.SetPixel((int) point_between.x, (int) point_between.y, brush._Color);
        tex2d.Apply();

        foreach (Vector2 point in points_registered)
        {
            
            //interpolate to get the points between points registered
             if (point == points_registered[0])//skip first
             continue;

            while (new Vector2(Mathf.RoundToInt(point_between.x), Mathf.RoundToInt(point_between.y)) != point)
            {
                point_between = Vector2.Lerp(point_between, point, .05f);
                
                tex2d.SetPixel(Mathf.RoundToInt(point_between.x), Mathf.RoundToInt(point_between.y), brush._Color); 

            }

            tex2d.Apply();

        }

      
    }




    //debug
    void PrintTexture(Texture2D tex)
    { //line by line
        string s = ""; 

        for (int y = canvas_height; y >= 0; y--)
        {
            for (int x = 0; x <= canvas_width; x++)
            {
               Color pixel = tex.GetPixel(x, y);
                if (pixel == Color.red)
                {
                    s += "1";
                }
                else
                    s += "0";
            }
            s += "\n";

        }

        Debug.Log(s);

    }

    


}
