using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour {

    [SerializeField]
    ScriptedEntityController scrEntCtrl;
    
    // Use this for initialization
	void Start ()
    {

        Camera cam = Camera.main;
        Vector3 pos = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0));
        pos.z = 0;

        transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {

        Camera cam = Camera.main;
        Vector3 pos = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0));
        pos.z = 0;

        transform.position = pos;


        //set mouse pos debug
        TextMesh mousePos = transform.GetChild(0).GetComponent<TextMesh>();

        GameObject scrEnt = scrEntCtrl.transform.GetChild(0).gameObject;

        mousePos.text = scrEnt.transform.position.ToString();

        //set player pos debug
        GameObject player = GameObject.Find("Player");

        TextMesh playerPos = transform.GetChild(1).GetComponent<TextMesh>();

        playerPos.text = player.transform.position.ToString();

    }
}
