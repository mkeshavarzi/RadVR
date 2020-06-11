using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class flyScript : MonoBehaviour {

    public GameObject controller;
    public GameObject cameraRig;
    public GameObject eyePos;
    public GameObject controllerScript;
    VRTK_ControllerEvents controllerEv;
    public GameObject arrow;
    private Vector3 transformVec;

    private bool mainMenu = false;

    // Use this for initialization
    void Start () {
        controllerEv = controllerScript.GetComponent<VRTK_ControllerEvents>();

	}
	
	// Update is called once per frame
	void Update () {
        radVRSetUpdate();
        transformVec = controller.transform.position - eyePos.transform.position;

        if ((mainMenu == false) && (controllerEv.gripPressed))
        {
           
            transformVec = Vector3.Scale(transformVec.normalized, new Vector3(0.05f, 0.05f, 0.05f));
            cameraRig.transform.Translate(transformVec);
        }

        if (controllerEv.gripTouched)
        {
            ShowArrow();
        }
        if (controllerEv.gripTouched == false){
            HideArrow();
        }


	}

    void ShowArrow()
    {
        arrow.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        arrow.transform.position = eyePos.transform.position + Vector3.Scale(eyePos.transform.forward.normalized, new Vector3(1f, 1f, 1f));
        arrow.transform.up = -(transformVec);
    }

    void HideArrow(){
        arrow.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }

    void radVRSetUpdate()
    {
        mainMenu = GameObject.Find("Settings").GetComponent<radVRSettings>().mainMenu;
    }
}
