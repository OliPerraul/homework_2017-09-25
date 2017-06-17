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

    static EntityEditorController entityEditorCtrl;

    public Sprite sprite;
    public string entity_name;

    private Button button;
    private InputField inputField;
    private EntityName entityName;

    void Start()
    {
        //add listener to on click event
        button.onClick.AddListener(delegate { entityEditorCtrl.SelectEntity(); });
        button.onClick.AddListener(delegate { SetSelectedOnClicked(); });
    }


    void Update()
    {
        //set entity name
        entity_name = entityName.name;

         //set icon image on the fly
        if (selected.Equals(gameObject.name))
        {
      
        }
    }


    void SetSelectedOnClicked()
    {
        selected = gameObject.name;
    }


    public static GameObject Create()//custom init
    {
        #region Init Statics
        if (entityIcons == null)
            entityIcons = GameObject.Find("EntityIcons");
        if (prefab == null)
            prefab = Resources.Load("UI/EntityIcon") as GameObject;
        if (entityEditorCtrl == null)
            entityEditorCtrl = GameObject.Find("EntityEditorController").GetComponent<EntityEditorController>();

        #endregion

        GameObject newObject = Instantiate(prefab, entityIcons.transform) as GameObject;

        //assign correct name
        newObject.name = prefab.name + entityIcons.transform.childCount;

        //access main component
        EntityIcon entityIcon = newObject.GetComponent<EntityIcon>();

        #region init instance properties
        entityIcon.button = entityIcon.GetComponent<Button>();
        entityIcon.inputField = entityIcon.GetComponentInChildren<InputField>();
        entityIcon.entityName = entityIcon.GetComponentInChildren<EntityName>();
        //..
        //..
        #endregion

        return newObject;
    }


}
