using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System;
using VRTK;

public class UIChangeMenu : MonoBehaviour {

    public GameObject changeMenuActivator;
    private bool showChangeWindow;
    private bool mainMenu;

    public Button apply;
    public Button cancel;
    public Slider transSlider;

    private GameObject RayCastGHOHit;

    public float targetTrans;
    Renderer targetRend;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        radVRSetUpdate();
        ShowChangeWindow();
        ButtonListerners();
        SliderUpdate();
    }

    void radVRSetUpdate()
    {
        showChangeWindow = GameObject.Find("Settings").GetComponent<radVRSettings>().showChangeWindow;
        mainMenu = GameObject.Find("Settings").GetComponent<radVRSettings>().mainMenu;
        
    }
    void ShowChangeWindow()
    {

        if (mainMenu == false)
        {
            changeMenuActivator.SetActive(showChangeWindow);
        }
        if (mainMenu == true)
        {
            changeMenuActivator.SetActive(false);
        }
    }
    void ButtonListerners()
    {
        apply.onClick.AddListener(Apply);
        cancel.onClick.AddListener(Cancel);
    }

    void Apply()
    {
        GameObject.Find("Settings").GetComponent<radVRSettings>().showChangeWindow = false;
    }
    void Cancel()
    {
        GameObject.Find("Settings").GetComponent<radVRSettings>().showChangeWindow = false;
    }
    public void TransRefresh()
    {
        RayCastGHOHit = GameObject.Find("Settings").GetComponent<radVRSettings>().RayCastGHOHit;
        targetRend = RayCastGHOHit.GetComponent<Renderer>();
        targetTrans = targetRend.material.color.a;
        transSlider.value = (targetTrans*100);

       
    }
    public void SliderUpdate()
    {
        transSlider.onValueChanged.AddListener(delegate { TransUpdate(); });
    }

    public void TransUpdate()
    {
        //targetRend.material.EnableKeyword("_METALLICGLOSSMAP");
        //Range transvalue = new Range(0, (int)(transSlider.value / 100));
        //targetRend.material.SetFloat("_Metallic", (transSlider.value / 100));
        

        targetRend.material.shader = Shader.Find("Transparent/Diffuse");
        Color prevCol = targetRend.material.color;
        targetRend.material.SetColor("_Color", new Color(prevCol.r, prevCol.g, prevCol.b, (transSlider.value / 100)));
    }
}
