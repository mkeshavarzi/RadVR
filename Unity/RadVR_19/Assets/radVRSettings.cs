using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using VRTK;


public class radVRSettings : MonoBehaviour {

    public bool VROn = true;

    public bool level1 = false;
    public bool level2 = false;
    public bool level3 = false;

    public bool planeStatic = false;

    [Header("Action Buttons")]
    public string keySunMinUp = "t";
    public string keySunMinDown = "g";
    public string keySunDayUp = "y";
    public string keySunDayDown =  "h";
    public string keySimulationEx = "m";
    public string keyDFSimulationEx = "b";
    public string keyMainMenu = "o";

    [Header("Building Settings")]
    public GameObject buildingGO;


    [Header("Plane Settings")]
    public float planeX = 4;
    public float planeY = 4;
    public GameObject planeGO;
    Collider planeCollider;

    [Header("Legand")]


    public float aBounce = 2;
    public float gridSize= 1;

    public float levelHeight = 1;
    public float colMax = 1000;




    public enum LevelAlias
    {
        Level1,
        Level2,
        Level3,
    }

    public radVRSettings.LevelAlias levelStatus;

    [Header("DONT TOUCH")]
    public bool mainMenu = false;
    public bool sunChanging = false;
    public bool SunMinUp = false;
    public bool SunMinDown = false;
    public bool SunDayUp = false;
    public bool SunDayDown = false;
    public bool showChangeWindow = false;
    public bool materialChangeMode = false;

    public bool simulationEx = false;
    public bool DFsimulationEx = false;

    public bool colorChange;
    public float blueVal;
    public float redVal;
    public float lowVal= 0;
    public float highVal;

    public DateTime dateTimePreview;
    public GameObject sunPathObject;
    public GameObject radVR;

    public string latitude;
    public string longitude;
    public string meridian;
    public string month;
    public string day;
    public string hour;
    public string minute;

    private string simTimePrev;

    public bool snapTime = false;
    public bool gridView = true;
    public bool planeView = true;
    public bool numTagView = true;
    public bool transBui = false;
    public bool planeFollow = true;

    private VRTK_ControllerEvents contEvRight;
    private VRTK_ControllerEvents contEvLeft;
    public GameObject contRight;
    public GameObject contLeft;
    public GameObject contRightGO;
    public GameObject contLeftGO;
    public GameObject eyePosition;
    private Vector3 contRightPosition;
    private Quaternion contRightRotation;


    public GameObject RayCastGHOHit;

    private GameObject numTagGO;
    private GameObject gridGO;


    public Vector2 touchPadAx;

    // Use this for initialization
    void Awake () {

        if (VROn == true)
        {
            contEvRight = contRight.GetComponent<VRTK_ControllerEvents>(); //change this if settings have changed
            contEvLeft = contLeft.GetComponent<VRTK_ControllerEvents>(); //change this if settings0 have changed
            LevelUpdate();
        }
        highVal = 1000;
    }
	
	// Update is called once per frame
	void Update () {

        //TouchpadSettings
       //SunChanging Settings
        LevelUpdate();
        SunChange();
        PlaneUpdate();
        prevSunProp();
        prevSimulationTime();
        menuTrigger();
        simulationTrigger();

    }

    void PlaneUpdate()
    {
       planeGO.transform.localScale = new Vector3(planeX, planeY, 0.0001f);
        if (level1 == true)
        {
            planeGO.GetComponent<MeshRenderer>().enabled = true;
//            gridGO.SetActive(false);
 //           numTagGO.SetActive(false);
        }
        else
        {
            if (planeView == true)
            {
                planeGO.GetComponent<MeshRenderer>().enabled = true;
                if (planeFollow== true)
                {
                    
                    planeGO.transform.position = new Vector3(eyePosition.transform.position.x, planeGO.transform.position.y, eyePosition.transform.position.z);
                }
                else
                {
                    planeGO.transform.position = new Vector3(0, planeGO.transform.position.y, 0);
                }

            }
            else
            {
                planeGO.GetComponent<MeshRenderer>().enabled = false;
            }
           /* if (gridView == !false)
            {
                gridGO.SetActive(true);
            }
            else
            {
                gridGO.SetActive(false);
            }
            if (numTagView == !false)
            {
                numTagGO.SetActive(true);
            }
            else
            {
                numTagGO.SetActive(false);
            }
            */
        }

        
//        BoxCollider boxCol = planeGO.GetComponent<BoxCollider>();
//        boxCol.size = new Vector3(planeX, planeY, 0.0001f);

    }

    void simulationTrigger()
    {
        if (VROn == true)
        {
            if (contEvRight.buttonOnePressed) { simulationEx = true; } else { simulationEx = false; }
            if (contEvRight.buttonTwoPressed) { DFsimulationEx = true; } else { DFsimulationEx = false; }
        }
        else
        {
            if (Input.GetKeyUp(keySimulationEx)) { simulationEx = true; } else { simulationEx = false; }
            if (Input.GetKeyUp(keyDFSimulationEx)) { DFsimulationEx = true; } else { DFsimulationEx = false; }
        }
    }


        void menuTrigger()
        {
            if (VROn == true)
            {
                if (contEvRight.triggerPressed) { mainMenu = true; } else { mainMenu = false; }
            }
            else
            {
                if (Input.GetKeyUp(keyMainMenu)) { mainMenu = true; } else { mainMenu = false; }
            }
        }


    void SunChange()
    {
        if (VROn == true)
        {
            touchPadAx = contEvRight.GetTouchpadAxis();

            if (touchPadAx.x > 0.7 ) { SunMinUp = true; } else { SunMinUp = false; }
            if (touchPadAx.x < -0.7) { SunMinDown = true; } else { SunMinDown = false; }
            if (touchPadAx.y > 0.7) { SunDayUp = true; } else { SunDayUp = false; }
            if (touchPadAx.y < -0.7)  { SunDayDown = true; } else { SunDayDown = false; }

/*            if (contEvRight.triggerPressed) { SunMinUp = true; } else { SunMinUp = false; }
            if (contEvRight.gripPressed) { SunMinDown = true; } else { SunMinDown = false; }
            if (contEvLeft.triggerPressed) { SunDayUp = true; } else { SunDayUp = false; }
            if (contEvLeft.gripPressed) { SunDayDown = true; } else { SunDayDown = false; }

    */
        }
        else
        {

        

        if (Input.GetKey(keySunMinUp)) { SunMinUp = true; } else { SunMinUp = false; }
        if (Input.GetKey(keySunMinDown)) { SunMinDown = true; } else { SunMinDown = false; }
        if (Input.GetKey(keySunDayUp)) { SunDayUp = true; } else { SunDayUp = false; }
        if (Input.GetKey(keySunDayDown)) { SunDayDown = true; } else { SunDayDown = false; }
        }



        if (SunMinUp == true || SunMinDown == true || SunDayUp == true || SunDayDown == true) { sunChanging = true; } else { sunChanging = false; }
    }

    void prevSunProp()
    {
        dateTimePreview = sunPathObject.GetComponent<SunPathRadVR>().dateTimePreview;
        month = TwoDigStr(dateTimePreview.Month);
        day = TwoDigStr(dateTimePreview.Day);
        hour = TwoDigStr(dateTimePreview.Hour);
        minute = TwoDigStr(dateTimePreview.Minute);
        latitude = sunPathObject.GetComponent<SunPathRadVR>().latitude;
        longitude = sunPathObject.GetComponent<SunPathRadVR>().longitude;
        meridian = (float.Parse(sunPathObject.GetComponent<SunPathRadVR>().timeZone) * 15).ToString();
    }

    void prevSimulationTime()
    {
        simTimePrev = radVR.GetComponent<runRadiance>().lastSimulationTime;
    }




    string TwoDigStr(float num)
    {
        if (num < 10)
        {
            string numS = "0" + num.ToString();
            return numS;
        }
        else
        {
            string numS = num.ToString();
            return numS;
        }
    }

    void LevelUpdate()
    {
        if (level1 == true) { levelStatus =  LevelAlias.Level1; }
        if (level2 == true) { levelStatus = LevelAlias.Level2; }
        if (level3 == true) { levelStatus = LevelAlias.Level3; }

    }


}

