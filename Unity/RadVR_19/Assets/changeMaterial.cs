using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class changeMaterial : MonoBehaviour {

    private GameObject contRight;
    private GameObject contLeft;
    private GameObject contRightGO;
    private GameObject contLeftGO;
    private VRTK_ControllerEvents contEvRight;
    private VRTK_ControllerEvents contEvLeft;

    private bool materialChangeMode = false;
    public bool showChangeWindow;

    public string tagName = "BElements";

    Ray matRay;
    LineRenderer matLineRenderer;
    RaycastHit matRaycastHit;

    private GameObject RayCastGHOHit= null;

    Material lineRendererMaterial;


    // Use this for initialization
    void Start () {
        radVRSetUpdate();
        contEvRight = contRight.GetComponent<VRTK_ControllerEvents>(); //change this if settings have changed
        contEvLeft = contLeft.GetComponent<VRTK_ControllerEvents>(); //change this if settings0 have changed
        LineRedererMaker();
        contEvLeft.TriggerPressed += new ControllerInteractionEventHandler(OnTriggerPress);

    }
	
	// Update is called once per frame
	void Update () {
        radVRSetUpdate();
        if (materialChangeMode == true)
        {
           rayUpdate();          
        }
        if(materialChangeMode != true)
        {
            HideRay();
        }
;
	}

    void radVRSetUpdate()
    {
        contRight = GameObject.Find("Settings").GetComponent<radVRSettings>().contRight;
        contLeft = GameObject.Find("Settings").GetComponent<radVRSettings>().contLeft;
        contRightGO = GameObject.Find("Settings").GetComponent<radVRSettings>().contRightGO;
        contLeftGO = GameObject.Find("Settings").GetComponent<radVRSettings>().contLeftGO;
        materialChangeMode = GameObject.Find("Settings").GetComponent<radVRSettings>().materialChangeMode;
    }

    void LineRedererMaker()
    {
        GameObject lineRendererGO = new GameObject("matLineRenderer");
        lineRendererGO.transform.parent = gameObject.transform;
        matLineRenderer = lineRendererGO.AddComponent<LineRenderer>();
        matLineRenderer.startWidth = 0;
        matLineRenderer.endWidth = 0;
        lineRendererMaterial = matLineRenderer.material;
        lineRendererMaterial.color = Color.yellow;
  


    }
    void rayUpdate()
    {

        matRay = new Ray(contLeftGO.transform.position, contLeftGO.transform.forward);

        if (Physics.Raycast(matRay, out matRaycastHit))
        {
            matLineRenderer.startWidth = 0.004f;
            matLineRenderer.endWidth = 0.004f;
            matLineRenderer.SetPosition(0, contLeftGO.transform.position);
            matLineRenderer.SetPosition(1, matRaycastHit.point);
            RayCastGHOHit = matRaycastHit.collider.gameObject;
        }
        else
        {
            RayCastGHOHit = null;
            matLineRenderer.startWidth = 0f;
            matLineRenderer.endWidth = 0f;
        }

        if (RayCastGHOHit != null)
        {
            Renderer rend = RayCastGHOHit.GetComponent<Renderer>();
            if(RayCastGHOHit.tag == tagName)
            {
                lineRendererMaterial = matLineRenderer.material;
                lineRendererMaterial.color = Color.green;
            }
            else
            {
                lineRendererMaterial = matLineRenderer.material;
                lineRendererMaterial.color = Color.yellow;
            }
        }
        
    }
    
        void radVRSetSend()
        {
        GameObject.Find("Settings").GetComponent<radVRSettings>().showChangeWindow = showChangeWindow;
        GameObject.Find("Settings").GetComponent<radVRSettings>().RayCastGHOHit = RayCastGHOHit;
    }

    private void OnTriggerPress(object sender, ControllerInteractionEventArgs e)
    {

        if ((RayCastGHOHit != null) && (RayCastGHOHit.tag == tagName))
        {
            showChangeWindow = true;
            radVRSetSend();
            GameObject.Find("MainMenu").GetComponent<UIChangeMenu>().TransRefresh();
        }
    }
    private void HideRay()
    {
        matLineRenderer.startWidth = 0;
        matLineRenderer.endWidth = 0;
    }
}



