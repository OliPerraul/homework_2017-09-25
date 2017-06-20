using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityName : NameField
{
        // Use this for initialization
    protected override void Start()
    {
        onEndEdit.AddListener(delegate { DoSaveName(); }); //add listen to event
    }

    //event handling method
    void DoSaveName()
    {
        SaveName<EntityScript>("obj_new", Global.entity_database);
    }

    protected override void SaveName<TVal>(string _default, Dictionary<string, TVal> database)
    {
        base.SaveName<TVal>(_default, database);

    }
}
