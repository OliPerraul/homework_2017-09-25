﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColorPalette : MonoBehaviour {

    //properties

    private string[] color_codes;

    private string colors_string = @"
#7C7C7C
#0000FC
#0000BC
#4428BC
#940084
#A80020
#A81000
#881400
#503000
#007800
#006800
#005800
#004058
#000000
#000000
#000000
#BCBCBC
#0078F8
#0058F8
#6844FC
#D800CC
#E40058
#F83800
#E45C10
#AC7C00
#00B800
#00A800
#00A844
#008888
#000000
#000000
#000000
#F8F8F8
#3CBCFC
#6888FC
#9878F8
#F878F8
#F85898
#F87858
#FCA044
#F8B800
#B8F818
#58D854
#58F898
#00E8D8
#787878
#000000
#000000
#FCFCFC
#A4E4FC
#B8B8F8
#D8B8F8
#F8B8F8
#F8A4C0
#F0D0B0
#FCE0A8
#F8D878
#D8F878
#B8F8B8
#B8F8D8
#00FCFC
#F8D8F8
#000000
#000000";



    // Use this for initialization
    void Start()
    {
        color_codes = colors_string.Split('\n');

        GameObject prefab = Resources.Load("UI/ColorButton") as GameObject;

        GameObject gobj;

        for (int i = 0; i < color_codes.Length; i++)
        {
            string color_code = color_codes[i];

            //remove carriage return
            color_code = color_code.Replace("\r", "");
            //remove hashtag
            color_code = color_code.Replace("#", "");

            if (!color_code.Equals(""))
            {
                gobj = Instantiate(prefab, transform.GetChild(0));

                gobj.name = color_code;

                Color color = ColorUtils.HexToColor(color_code);

                Image butIma = gobj.GetComponent<Image>();
                butIma.color = color;
            }
        }
	}
	

}