using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntityIcon : MonoBehaviour
{
    static string selected = "";

    static GameObject entityIcons;//container
    static GameObject prefab;

 
    public Sprite sprite;
    public string entity_name;

    private Button button;
    private EntityName entityName;

    void Start()
    {
        //add listener to on click event
        button.onClick.AddListener(delegate { EntityEditorController.SelectEntity(); });
        button.onClick.AddListener(delegate { SetSelectedOnClicked(); });
    }
    
    void Update()
    {
        //set entity name
        entity_name = entityName.text;

         //set icon image on the fly
        if (selected.Equals(gameObject.name))
        {
      
        }
    }
    
    void SetSelectedOnClicked()
    {
        selected = gameObject.name;
    }
    
    public static GameObject Create(string name, EntityScript entity)//custom init
    {
        #region Init Statics
        if (entityIcons == null)
            entityIcons = GameObject.Find("EntityIcons");
        if (prefab == null)
            prefab = Resources.Load("UI/EntityIcon") as GameObject;

        #endregion

        GameObject newObject = Instantiate(prefab, entityIcons.transform) as GameObject;

        //assign correct name
        newObject.name = prefab.name + entityIcons.transform.childCount;

        //access main component
        EntityIcon entityIcon = newObject.GetComponent<EntityIcon>();

        #region init instance properties
        entityIcon.button = entityIcon.GetComponent<Button>();
        entityIcon.entityName = entityIcon.GetComponentInChildren<EntityName>();

        entityIcon.entityName.text = name;

        //..
        //..
        #endregion

        return newObject;
    }


}
