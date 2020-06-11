using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System;
using VRTK;

public class ReflFollow : MonoBehaviour {

    private VRTK_ControllerEvents contEvRight;
    private VRTK_ControllerEvents contEvLeft;
    private GameObject contRightGO;
    private GameObject contLeftGO;
    private bool mainMenu;

    public GameObject reflectionSphere;
    public GameObject reflecProbe;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        radVRSetUpdate();

        if (mainMenu == true)
        {
            if(reflectionSphere.GetComponent<MeshRenderer>().enabled != true)
            {
                reflectionSphere.GetComponent<MeshRenderer>().enabled = true;
            }

            reflectionSphere.transform.position = contRightGO.transform.position;
            reflecProbe.transform.position = contRightGO.transform.position;
        }
        else
        {
            if (reflectionSphere.GetComponent<MeshRenderer>().enabled != false)
            {
                reflectionSphere.GetComponent<MeshRenderer>().enabled = false;
            }
        }


    }

    void radVRSetUpdate()
    {
        mainMenu = GameObject.Find("Settings").GetComponent<radVRSettings>().mainMenu;
        contRightGO = GameObject.Find("Settings").GetComponent<radVRSettings>().contRightGO;
        contLeftGO = GameObject.Find("Settings").GetComponent<radVRSettings>().contLeftGO;
    }

}
