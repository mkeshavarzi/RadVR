using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System;
using VRTK;
public class UIMainMenu : MonoBehaviour {

    //RadVRGlobalSettings
    private float planeX;
    private float planeY;

    private float blueVal;
    private float redVal;
    private float lowVal;
    private float highVal;


    private bool level1 = false;
    private bool level2 = false;
    private bool level3 = false;
    private bool simulationEx = false;
    private bool DFsimulationEx = false;
    private bool colorChange;

    private radVRSettings.LevelAlias levelStatus;
    private bool mainMenu = true;
    public GameObject mainMenuActGO;
    public GameObject VRTKPointerGO;
    private bool snapTime;
    private bool transBui;
    private bool materialChangeMode;


    //Buttons
    [Header("BUTTONS")]
    private bool level1Bool;
    private bool level2Bool;
    private bool level3Bool;
    private bool gridView;
    private bool planeView;
    private bool numTagView;
    private bool snapMenuBool;
    private bool planeFollow;

    private float aBounce = 2;
    private float gridSize = 1;

    private float levelHeight = 1;
    private float colMax = 1000;


    private string latitude;
    private string longitude;
    private string timeZone;


    

    [Header("BUTTONSGameObjects")]
    [Header("Tabs")]
    public GameObject levelTab;
    public GameObject LTab1;
    public GameObject LTab2;
    public GameObject LTab3;

    [Header("LevelTabs")]
    public Toggle level1Bu;
    public Toggle level2Bu;
    public Toggle level3Bu;

    [Header("L1Tab")]
    public Toggle snapTimeBu;
    public Toggle transBuiBu;
    public Toggle materialChangeModeBu;
    public Button Dec09;
    public Button Dec12;
    public Button Dec15;
    public Button Mar09;
    public Button Mar12;
    public Button Mar15;
    public Button Jun09;
    public Button Jun12;
    public Button Jun15;





    [Header("L2Tab")]
    public Toggle gridViewBu;
    public Toggle planeViewBu;
    public Toggle numTagViewBu;
    public Toggle planefollowBu;

    public Slider planeXBu;
    public Slider planeYBu;
    public Slider gridSBu;

    public Slider topLegBu;
    public Slider lowLegBu;

    public Slider ABBu;

    public Button runBu;
    public Button DFrunBu;

    [Header("ButtonColors")]
    public Color enabledColor;
    public Color disabledColor;
    public Color highlightedColor;
    public Color pressedColor;



    ColorBlock enabledButCB;
    ColorBlock disabledButCB;





    // Use this for initialization
    void Start () {
        radVRSetAwake();
        enabledButCB.normalColor = enabledColor;
        enabledButCB.highlightedColor = highlightedColor;
        enabledButCB.pressedColor = pressedColor;

        disabledButCB.normalColor = disabledColor;
        disabledButCB.highlightedColor = highlightedColor;
        disabledButCB.pressedColor = pressedColor;

        MenuUpdate();

    }
	
	// Update is called once per frame
	void Update () {
 //       SimulationOff();
        radVRSetUpdate();
        ShowMenu();
        radVRSetUpdateSend();
        colValSet();

    }



    void radVRSetUpdate()
    {
        level1 = GameObject.Find("Settings").GetComponent<radVRSettings>().level1;
        level2 = GameObject.Find("Settings").GetComponent<radVRSettings>().level2;
        level3 = GameObject.Find("Settings").GetComponent<radVRSettings>().level3;
        simulationEx = GameObject.Find("Settings").GetComponent<radVRSettings>().simulationEx;
        levelStatus = GameObject.Find("Settings").GetComponent<radVRSettings>().levelStatus;
        mainMenu = GameObject.Find("Settings").GetComponent<radVRSettings>().mainMenu;
        snapTime = GameObject.Find("Settings").GetComponent<radVRSettings>().snapTime;
        materialChangeMode = GameObject.Find("Settings").GetComponent<radVRSettings>().materialChangeMode;
        DFsimulationEx = GameObject.Find("Settings").GetComponent<radVRSettings>().DFsimulationEx;
        lowVal = GameObject.Find("Settings").GetComponent<radVRSettings>().lowVal;
        highVal = GameObject.Find("Settings").GetComponent<radVRSettings>().highVal;
        aBounce = GameObject.Find("Settings").GetComponent<radVRSettings>().aBounce;
    }

    void colValSet()
    {
        lowLegBu.minValue = lowVal;
        lowLegBu.maxValue = (highVal + lowVal) / 2;
        topLegBu.minValue = (highVal + lowVal) / 2;
        topLegBu.maxValue = highVal;

        lowLegBu.transform.Find("TextMin").GetComponent<Text>().text = lowLegBu.minValue.ToString();
        topLegBu.transform.Find("TextMax").GetComponent<Text>().text = topLegBu.maxValue.ToString();
        lowLegBu.transform.Find("TextValue").GetComponent<Text>().text = lowLegBu.value.ToString();
        topLegBu.transform.Find("TextValue").GetComponent<Text>().text = topLegBu.value.ToString();
    }

    void ShowMenu()
    {
        if (mainMenu == true) { mainMenuActGO.SetActive(true); } else { mainMenuActGO.SetActive(false); }
        if (mainMenu == true) { VRTKPointerGO.GetComponent<VRTK_StraightPointerRenderer>().enabled = true; } else { VRTKPointerGO.GetComponent<VRTK_StraightPointerRenderer>().enabled = false; }
    }

    void MenuUpdate()
    {

        level1Bu.isOn = true;
        level2Bu.isOn = false;
        level3Bu.isOn = false;
        snapTimeBu.isOn = snapTime;
        planeViewBu.isOn = true;
        planefollowBu.isOn = true;
        gridViewBu.isOn = true;
        numTagViewBu.isOn = true;




    }

    /*    void snapMenuT() { snapMenuBool = !snapMenuBool;}
        void level1T() { level1Bool =!level1Bool; if (level1Bool == true) { level2Bool = false; level3Bool = false; } }
        void level2T() { level2Bool = !level2Bool; if (level2Bool == true) { level1Bool = false; level3Bool = false; } }
        void level3T() { level3Bool = !level3Bool; if (level3Bool == true) { level1Bool = false; level2Bool = false; } }

      */

    void level1T() { if (level1Bu.isOn == true) { level2Bu.isOn = false; level3Bu.isOn = false; LTab1.SetActive(true); LTab2.SetActive(false); } LTab3.SetActive(false); }
    void level2T() { if (level2Bu.isOn == true) { level1Bu.isOn = false; level3Bu.isOn = false; LTab1.SetActive(false); LTab2.SetActive(true); } LTab3.SetActive(false); }
    void level3T() { if (level3Bu.isOn == true) { level1Bu.isOn = false; level2Bu.isOn = false; LTab1.SetActive(false); LTab2.SetActive(false); } LTab3.SetActive(true); }

    void radVRSetUpdateSend()
    {

        level1Bu.onValueChanged.AddListener(delegate { level1T();});
        level2Bu.onValueChanged.AddListener(delegate { level2T(); });
        level3Bu.onValueChanged.AddListener(delegate { level3T(); });

        planeX = planeXBu.value;
        planeY = planeYBu.value;
        gridSize = gridSBu.value;
        aBounce = ABBu.value;

        redVal = topLegBu.value;
        blueVal = lowLegBu.value;


        colorChange = false;
        topLegBu.onValueChanged.AddListener(delegate { colorChangeT(); });
        lowLegBu.onValueChanged.AddListener(delegate { colorChangeT(); });

        level1 = level1Bu.isOn;
        level2 = level2Bu.isOn; 
        level3 = level3Bu.isOn;

        snapTime = snapTimeBu.isOn;
        transBui = transBuiBu.isOn;
        materialChangeMode = materialChangeModeBu.isOn;

        planeFollow = planefollowBu.isOn;
        gridView = gridViewBu.isOn;
        planeView = planeViewBu.isOn;
        numTagView = numTagViewBu.isOn;

        GameObject.Find("Settings").GetComponent<radVRSettings>().level1 = level1;
        GameObject.Find("Settings").GetComponent<radVRSettings>().level2 = level2;
        GameObject.Find("Settings").GetComponent<radVRSettings>().level3 = level3;
        GameObject.Find("Settings").GetComponent<radVRSettings>().planeX = planeX;
        GameObject.Find("Settings").GetComponent<radVRSettings>().planeY = planeY;
        GameObject.Find("Settings").GetComponent<radVRSettings>().snapTime = snapTime;
        GameObject.Find("Settings").GetComponent<radVRSettings>().transBui = transBui;
        GameObject.Find("Settings").GetComponent<radVRSettings>().materialChangeMode = materialChangeMode;
        GameObject.Find("Settings").GetComponent<radVRSettings>().planeFollow = planeFollow;
        GameObject.Find("Settings").GetComponent<radVRSettings>().gridView = gridView;
        GameObject.Find("Settings").GetComponent<radVRSettings>().planeView = planeView;
        GameObject.Find("Settings").GetComponent<radVRSettings>().numTagView = numTagView;
        GameObject.Find("Settings").GetComponent<radVRSettings>().gridSize = gridSize;
        GameObject.Find("Settings").GetComponent<radVRSettings>().blueVal = blueVal;
        GameObject.Find("Settings").GetComponent<radVRSettings>().redVal = redVal;
        GameObject.Find("Settings").GetComponent<radVRSettings>().colorChange = colorChange;
        GameObject.Find("Settings").GetComponent<radVRSettings>().aBounce = aBounce;


        runBu.onClick.AddListener(simulationExT);
        DFrunBu.onClick.AddListener(DFsimulationExT);


        Dec09.onClick.AddListener(Dec09T);
        Dec12.onClick.AddListener(Dec12T);
        Dec15.onClick.AddListener(Dec15T);
        Mar09.onClick.AddListener(Mar09T);
        Mar12.onClick.AddListener(Mar12T);
        Mar15.onClick.AddListener(Mar15T);
        Jun09.onClick.AddListener(Jun09T);
        Jun12.onClick.AddListener(Jun12T);
        Jun15.onClick.AddListener(Jun15T);


    }

    void SimulationOff()
    {
        simulationEx = false;
        GameObject.Find("Settings").GetComponent<radVRSettings>().simulationEx = simulationEx;
        DFsimulationEx = false;
        GameObject.Find("Settings").GetComponent<radVRSettings>().DFsimulationEx = DFsimulationEx;
    }

    void simulationExT()
    {
        simulationEx = true;
        GameObject.Find("Settings").GetComponent<radVRSettings>().simulationEx = simulationEx;
    }

    void DFsimulationExT()
    {
        DFsimulationEx = true;
        GameObject.Find("Settings").GetComponent<radVRSettings>().DFsimulationEx = DFsimulationEx;
    }

    void Dec09T() { GameObject.Find("SunPath").GetComponent<SunPathRadVR>().sunUpdate(new DateTime(2010, 12, 21, 9, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone)); }
    void Dec12T() { GameObject.Find("SunPath").GetComponent<SunPathRadVR>().sunUpdate(new DateTime(2010, 12, 21, 12, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone)); }
    void Dec15T() { GameObject.Find("SunPath").GetComponent<SunPathRadVR>().sunUpdate(new DateTime(2010, 12, 21, 15, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone)); }
    void Mar09T() { GameObject.Find("SunPath").GetComponent<SunPathRadVR>().sunUpdate(new DateTime(2010, 3, 21, 9, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone)); }
    void Mar12T() { GameObject.Find("SunPath").GetComponent<SunPathRadVR>().sunUpdate(new DateTime(2010, 3, 21, 12, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone)); }
    void Mar15T() { GameObject.Find("SunPath").GetComponent<SunPathRadVR>().sunUpdate(new DateTime(2010, 3, 21, 15, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone)); }
    void Jun09T() { GameObject.Find("SunPath").GetComponent<SunPathRadVR>().sunUpdate(new DateTime(2010, 6, 21, 9, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone)); }
    void Jun12T() { GameObject.Find("SunPath").GetComponent<SunPathRadVR>().sunUpdate(new DateTime(2010, 6, 21, 12, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone)); }
    void Jun15T() { GameObject.Find("SunPath").GetComponent<SunPathRadVR>().sunUpdate(new DateTime(2010, 6, 21, 15, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone)); }



    void colorChangeT()
    {
        colorChange = true;
        GameObject.Find("Settings").GetComponent<radVRSettings>().colorChange = colorChange;
    }


    void MenuHover()
    {
        if (snapMenuBool == true) { snapTimeBu.colors = enabledButCB; } else { snapTimeBu.colors = disabledButCB; }
        if (level1Bool == true) { level1Bu.colors = enabledButCB; } else { level1Bu.colors = disabledButCB; }
        if (level2Bool == true) { level2Bu.colors = enabledButCB; } else { level2Bu.colors = disabledButCB; }
        if (level3Bool == true) { level3Bu.colors = enabledButCB; } else { level3Bu.colors = disabledButCB; }

    }

    void radVRSetAwake()
    {
        latitude = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().latitude;
        longitude = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().longitude;
        timeZone = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().timeZone;

    }
}
