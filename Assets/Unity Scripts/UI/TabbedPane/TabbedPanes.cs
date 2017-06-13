using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabbedPanes : MonoBehaviour
{
    [SerializeField]
    private GameObject panel1;

    [SerializeField]
    private GameObject panel2;

    [SerializeField]
    private GameObject panel3;


    [SerializeField]
    private GameObject colorPalette;

    // Use this for initialization
    void Start ()
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
        panel3.SetActive(false);
        colorPalette.SetActive(false);
    }
    
    public void SetCurrentTab()
    {
        var go = EventSystem.current.currentSelectedGameObject;
        if (go != null)
        {
            switch (go.name)
            {
                case "Tab1":
                    panel1.SetActive(true);
                    panel2.SetActive(false);
                    panel3.SetActive(false);
                    colorPalette.SetActive(false);

                    break;

                case "Tab2":
                    panel1.SetActive(false);
                    panel2.SetActive(true);
                    panel3.SetActive(false);
                    colorPalette.SetActive(false);
                    break;

                case "Tab3":
                    panel1.SetActive(false);
                    panel2.SetActive(false);
                    panel3.SetActive(true);
                    colorPalette.SetActive(true);
                    break;

            }

        }             
    }



}
