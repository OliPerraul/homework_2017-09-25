using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCoder : MonoBehaviour
{
    private InputField inputField;

    [SerializeField]
    private Color color_comments = Color.green;
    [SerializeField]
    private Color color_logic = Color.yellow;
    [SerializeField]
    private Color color_function = Color.blue;
    [SerializeField]
    private Color color_gameVars = Color.red;
    [SerializeField]
    private Color color_local = Color.red;
    [SerializeField]
    private Color color_nil = Color.red;
    [SerializeField]
    private Color color_self = Color.red;
    [SerializeField]
    private Color color_builtin_func = Color.red;
    [SerializeField]
    private Color color_sprite_names = Color.grey;
    [SerializeField]
    private Color color_entity_names = Color.grey;


    [SerializeField]
    private Text content;
    [SerializeField]
    private Text content_color;
    private string color_coded_text = "";




    // Use this for initialization
    void Start ()
    {
        inputField = GetComponent<InputField>();

        content_color.supportRichText = true;
        inputField.onValueChanged.AddListener(delegate { ColorCode(); }); //add listen to event
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (inputField.isFocused)//color code if necessary
        {
            if (content.text.Equals("") || (Input.anyKey))
            {
                ColorCode();
            }
        }
       
        //update text
        content_color.text = color_coded_text;

    }

    //color code the content of the input field
    public void ColorCode()
    {
        //color code "function"
        color_coded_text = ColorCodeComments(content.text, color_comments);

        //Built in functions (own)
        color_coded_text = ColorCodeKeyword("tablelength", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("new", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("add", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("getplayerx", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("getplayery", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("getmousex", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("getmousey", color_coded_text, color_builtin_func);
        //built in (math)
        color_coded_text = ColorCodeKeyword("abs", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("acos", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("asin", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("atan", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("ceil", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("cos", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("deg", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("exp", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("floor", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("fmod", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("huge", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("log", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("max", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("maxinteger", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("min", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("mininteger", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("modf", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("pi", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("rad", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("random", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("randomseed", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("sin", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("sqrt", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("tan", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("tointeger", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("type", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("ult", color_coded_text, color_builtin_func);
        //Lua function
        color_coded_text = ColorCodeKeyword("pairs", color_coded_text, color_builtin_func);
        color_coded_text = ColorCodeKeyword("ipairs", color_coded_text, color_builtin_func);
        
        color_coded_text = ColorCodeKeyword("if", color_coded_text, color_logic);
        color_coded_text = ColorCodeKeyword("then", color_coded_text, color_logic);
        color_coded_text = ColorCodeKeyword("end", color_coded_text, color_logic);
        color_coded_text = ColorCodeKeyword("for", color_coded_text, color_logic);
        color_coded_text = ColorCodeKeyword("do", color_coded_text, color_logic);
        color_coded_text = ColorCodeKeyword("in", color_coded_text, color_logic);
        color_coded_text = ColorCodeKeyword("else", color_coded_text, color_logic);

        color_coded_text = ColorCodeKeyword("function", color_coded_text, color_function);
        color_coded_text = ColorCodeKeyword("return", color_coded_text, color_function);
        color_coded_text = ColorCodeKeyword("nil", color_coded_text, color_nil);
        color_coded_text = ColorCodeKeyword("local", color_coded_text, color_local);
        color_coded_text = ColorCodeKeyword("self", color_coded_text, color_self);

        //entity vars
        color_coded_text = ColorCodeKeyword("x", color_coded_text, color_gameVars);
        color_coded_text = ColorCodeKeyword("y", color_coded_text, color_gameVars);
        color_coded_text = ColorCodeKeyword("type", color_coded_text, color_gameVars);
        color_coded_text = ColorCodeKeyword("visible", color_coded_text, color_gameVars);
        color_coded_text = ColorCodeKeyword("sprite", color_coded_text, color_gameVars);
        color_coded_text = ColorCodeKeyword("hbwidth", color_coded_text, color_gameVars);
        color_coded_text = ColorCodeKeyword("hbheight", color_coded_text, color_gameVars);
        color_coded_text = ColorCodeKeyword("hboffsetx", color_coded_text, color_gameVars);
        color_coded_text = ColorCodeKeyword("hboffsety", color_coded_text, color_gameVars);
        color_coded_text = ColorCodeKeyword("originx", color_coded_text, color_gameVars);
        color_coded_text = ColorCodeKeyword("originy", color_coded_text, color_gameVars);
        // color_coded_text = ColorCodeKeyword("world", color_coded_text, color_keyword_gameVars);

        //entity func
        color_coded_text = ColorCodeKeyword("start", color_coded_text, color_gameVars);
        color_coded_text = ColorCodeKeyword("update", color_coded_text, color_gameVars);


        //color code sprite names
        foreach (KeyValuePair<string, Texture2D> entry in Global.sprite_database)
        {
            color_coded_text = ColorCodeKeyword(entry.Key, color_coded_text, color_sprite_names);
        }

        //color code entity names
        foreach (KeyValuePair<string, EntityScript> entry in Global.entity_database)
        {
            color_coded_text = ColorCodeKeyword(entry.Key, color_coded_text, color_entity_names);
        }



    }//end

    //helper method (color single words)
    string ColorCodeKeyword(string keyword, string content, Color color)
    {
        string colorTag = "<color=" + ColorUtils.ColorToHexCode(color) + ">";

        int keyword_index = -1;

        while (keyword_index < content.Length)
        {
            keyword_index = content.IndexOf(keyword, keyword_index + 1);

            if (keyword_index == -1)
                break;

            //else insert color tag
            int offset = keyword.Length;

            //forward conditions
            if (keyword_index + offset != content.Length)//if not last word in the string
            {
                if (!(
            content[keyword_index + offset] == '.' ||
            content[keyword_index + offset] == ',' ||
            content[keyword_index + offset] == ' ' ||
            content[keyword_index + offset] == '[' ||
            content[keyword_index + offset] == ']' ||
            content[keyword_index + offset] == ':' ||
            content[keyword_index + offset] == ';' ||
            content[keyword_index + offset] == '(' ||
            content[keyword_index + offset] == ')' ||
            content[keyword_index + offset] == '%' ||
            content[keyword_index + offset] == '^' ||
            content[keyword_index + offset] == '&' ||
            content[keyword_index + offset] == '*' ||
            content[keyword_index + offset] == '{' ||
            content[keyword_index + offset] == '}' ||
            content[keyword_index + offset] == '|' ||
            content[keyword_index + offset] == '<' ||
            content[keyword_index + offset] == '>' ||
            content[keyword_index + offset] == '=' ||
            content[keyword_index + offset] == '?' ||
            content[keyword_index + offset] == '\t' ||
            content[keyword_index + offset] == '\r' ||
            content[keyword_index + offset] == '\n'))
                {
                    keyword_index += offset;//adjust index
                    continue; //if not any of these char ignore color code for given word
                }
            }

            //backward conditions
            if (keyword_index != 0) //if not first word in the string
            {
                if (!(content[keyword_index - 1] == ' ' ||
                content[keyword_index - 1] == '.' ||
                content[keyword_index - 1] == ',' ||
                content[keyword_index - 1] == '(' ||
                content[keyword_index - 1] == '[' ||
                content[keyword_index - 1] == '+' ||
                content[keyword_index - 1] == '-' ||
                content[keyword_index - 1] == '%' ||
                content[keyword_index - 1] == '^' ||
                content[keyword_index - 1] == '#' ||
                content[keyword_index - 1] == '!' ||
                content[keyword_index - 1] == '*' ||
                content[keyword_index - 1] == '/' ||
                content[keyword_index - 1] == '{' ||
                content[keyword_index - 1] == '|' ||
                content[keyword_index - 1] == '&' ||
                content[keyword_index - 1] == '?' ||
                content[keyword_index - 1] == '<' ||
                content[keyword_index - 1] == '>' ||
                content[keyword_index - 1] == '=' ||
                content[keyword_index - 1] == '\t' ||
                content[keyword_index - 1] == '\r' ||
                content[keyword_index - 1] == '\n'))
                {
                    keyword_index += offset;//adjust index
                    continue; //if not any of these char ignore color code for given word
                }

            }

            offset += colorTag.Length;
            content = content.Insert(keyword_index, colorTag);//begin
            content = content.Insert(keyword_index + offset, "</color>");//close

            keyword_index += offset;//adjust index

        }

        return content;
    }

    string ColorCodeComments(string content, Color color)
    {
        string colorTag = "<color=" + ColorUtils.ColorToHexCode(color) + ">";

        string comment = "--";
        string closeTag = "</color>";

        int index = -1;

        while (index < content.Length)
        {
            index = content.IndexOf(comment, index + 1);

            if (index == -1)//not found
                break;

            //else insert color tag
            content = content.Insert(index, colorTag);//begin tag

            int newLine_index = content.IndexOf('\n', index);
            int endComment_index = 0;

            if (newLine_index != -1)
                endComment_index = newLine_index;
            else
                endComment_index = content.Length - 1;


            content = content.Insert(endComment_index + 1, "</color>");//close tag


            index = endComment_index + closeTag.Length;//adjust index
        }


        return content;
    }


}
