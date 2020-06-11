using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class textConsole : MonoBehaviour {

    Text textComp;
    public DateTime dateTimePreview;
    public GameObject sunPathObject;
    public GameObject radVR;

    private string latitude;
    private string longitude;
    private string meridian;
    private string month;
    private string day;
    private string hour;
    private string minute;

    private string simTimePrev;

    private radVRSettings.LevelAlias levelStatus;


    // Use this for initialization
    void Start () {
        textComp = gameObject.GetComponent<Text>();
        prevSunProp();
        

    }
	
	// Update is called once per frame
	void Update () {

        prevSunProp();
        prevSimulationTime();
        textComp.text = "DATE: "+month+"/"+day+"/2010   TIME: "+hour+":"+minute+"    LONG/LAT: "+longitude+"/"+latitude+"   SIMULATION TIME: "+ simTimePrev + "LevelStatus:" + levelStatus.ToString();
        radVRSetUpdate();

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
        simTimePrev =  radVR.GetComponent<runRadiance>().lastSimulationTime;
    }

    void radVRSetUpdate()
    {

        levelStatus = GameObject.Find("Settings").GetComponent<radVRSettings>().levelStatus;


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
}
