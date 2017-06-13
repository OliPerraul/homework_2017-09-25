using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    private Texture2D tex2d;
    
    private SpriteRenderer spriteRend;

    private Sprite sprite;

    public Sprite _Sprite
    {
        get { return sprite; }
        set
        {
            sprite = value;
        }
    }
    
    #region Brush properties
    
    private Color old_color = Color.red;

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

    [SerializeField]
    private int size;
    [SerializeField]
    private int max_size = 6;
    [SerializeField]
    private int min_size = 1;

    public int Size
    {
        get { return size; }
       
    }


    #endregion

    // Use this for initialization
    void Start()
    {
       color = Color.blue;
             
        // Get renderer you want to probe
        spriteRend = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        SetBrushSize();
        spriteRend.sprite = sprite;
        
    }

    void SetBrushSize()
    {
        if (size < max_size)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
            {
                size++;
            }
        }

        if (size > min_size)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
            {
                size--;
            }
        }

    }
    
    
 

}
