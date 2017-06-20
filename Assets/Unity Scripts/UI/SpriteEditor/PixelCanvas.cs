using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelCanvas : MonoBehaviour
{
    
    [SerializeField]
    private Brush brush;
    [SerializeField]
    private GameObject background;

    private Texture2D tex2d;

    private Texture2D texBrush;

    private Sprite sprite;
    public Sprite _Sprite
    {
        get { return sprite; }
        set { sprite = value; }

    }
    
    private SpriteRenderer spriteRend;
    private BoxCollider2D box2d;

    //drawing
    private List<Vector2> points_registered;


    // Use this for initialization
    void Start ()
    {
        // Get renderer you want to probe
        spriteRend = GetComponent<SpriteRenderer>();

        box2d = GetComponent<BoxCollider2D>();

        //drawing
        points_registered = new List<Vector2>();


    }

	// Update is called once per frame
	void Update ()
    {
        try
        {

            if (tex2d != null)
            {
                ColorTexture();

                //create the edited sprite from tex2d 
                sprite = Sprite.Create(tex2d, new Rect(0f, 0f, tex2d.width, tex2d.height), new Vector2(0.5f, 0.5f), Global.PPU);

                spriteRend.sprite = sprite;
            }
        }
        catch { }
                       
    }
    

    public void SetCurrEditedTex(Texture2D tex)
    {
        tex2d = tex;
        tex2d.filterMode = FilterMode.Point;
    }


    public void AdjustColliderDimensions()
    {
        box2d.size = new Vector2(((float)tex2d.width)/ Global.PPU, ((float)tex2d.height) / Global.PPU);

        //Adjust background
        Texture2D tex_bg = new Texture2D(tex2d.width, tex2d.height, TextureFormat.RGBA32, false);
        Sprite spr_bg = Sprite.Create(tex_bg, new Rect(0f, 0f, tex2d.width, tex2d.height), new Vector2(0.5f, 0.5f), Global.PPU);
        background.GetComponent<SpriteRenderer>().sprite = spr_bg;
    }

    public void SetBrushTex(Vector2[,] points)
    {
        texBrush = new Texture2D(tex2d.width, tex2d.height, TextureFormat.RGBA32, false);
        texBrush.filterMode = FilterMode.Point;

        //clears out to all alpha
        var texColors = new Color32[tex2d.width * tex2d.height];
        for(int i= 0;i<texColors.Length; i++)
         texColors[i] = Color.clear;

        texBrush.SetPixels32(texColors);

        //apply brush dot
        for (int i = 0; i < points.GetLength(1); i++)
        {
            for (int j = 0; j < points.GetLength(0); j++)
            {
                texBrush.SetPixel(Mathf.RoundToInt(points[j,i].x), Mathf.RoundToInt(points[j, i].y), brush._Color);
            }
        }


        texBrush.Apply();

        Sprite spr_brush = Sprite.Create(texBrush, new Rect(0f, 0f, tex2d.width, tex2d.height), new Vector2(0.5f, 0.5f), Global.PPU);

        brush._Sprite = spr_brush;

    }
        

    void ColorTexture()
    {
        Vector2 mouse_pos = Input.mousePosition; // Mouse position

        int brush_size = brush.Size;
       
        Camera cam = Camera.main; // Camera to use for raycasting

        Ray ray = cam.ScreenPointToRay(mouse_pos);
    
        RaycastHit2D hit;
        hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f);


        if (hit.collider && hit.collider.tag.Equals("Canvas")) 
        {
            //deactivate brush
            brush.gameObject.SetActive(true);


            //get positionnof the pixel clicked
            // assumes an orthographic camera
            var pos = cam.ScreenToWorldPoint(mouse_pos);
            pos.z = transform.position.z;
            pos = transform.InverseTransformPoint(pos); //Transforms position from world space to local space.

            int xPixel = Mathf.RoundToInt(pos.x * Global.PPU);
            int yPixel = Mathf.RoundToInt(pos.y * Global.PPU);

            xPixel += tex2d.width / 2;
            yPixel += tex2d.height / 2;


            //points covered by the brush            
            Vector2[,] points = new Vector2[brush_size, brush_size];

            for (int i = 0; i < brush_size; i++)
            {
                for (int j = 0; j < brush_size; j++)
                {
                    points[j, i] = new Vector2(xPixel + j, yPixel + i);
                }
            }

            //Use point to set the brush
            SetBrushTex(points);

            if (Input.GetMouseButton(0))//if mouse butt is held then draw
            {
                for (int i = 0; i < points.GetLength(0); i++)
                {
                    for (int j = 0; j < points.GetLength(1); j++)
                    {
                        if (!points_registered.Contains(points[j, i]))
                            points_registered.Add(points[j, i]);
                    }
                }

                //draw linear interpolation from points registered
                DrawPoints(points_registered);
            }
        }
        else
        {
            //deactivate brush
            brush.gameObject.SetActive(false);
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
    

}
