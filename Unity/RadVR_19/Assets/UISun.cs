using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Globalization;
public class UISun : MonoBehaviour {

    public GameObject sunLocationGO;//Position of the Sphere GO - Add in Scene
    public GameObject sunRotationGO;//position of Directional Light GameObject - Add in Scene

    Text textCompTime;
    Text textCompMon;
    Text textCompDay;

    public GameObject TimeTextGO;
    public GameObject MonTextGO;
    public GameObject DayTextGO;

    private string latitude;
    private string longitude;
    private string meridian;
    private string month;
    private string day;
    private string hour;
    private string minute;
    private string timeZone;

    private string simTimePrev;
    string monthName; 





    // Use this for initialization
    void Start () {
        radVRSetAwake();
        textCompTime = TimeTextGO.GetComponent<Text>();
        textCompMon = MonTextGO.GetComponent<Text>();
        textCompDay = DayTextGO.GetComponent<Text>();
        SunTagLabels();




    }
	
	// Update is called once per frame
	void Update () {

        //      GameObject.Find("SunPath").GetComponent<SunPathRadVR>().sunUpdate();


        radVRSetUpdate();
        gameObject.transform.rotation = sunRotationGO.transform.rotation;
        gameObject.transform.position = sunLocationGO.transform.position;
 //      string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(int.Parse(month));

        if (monthName != null)
        {
            textCompTime.text = hour + ":" + minute;
            textCompMon.text = monthName.ToUpper();
            textCompDay.text = day;
        }


       



    }

    void SunTagLabels()
    {
        GameObject sunPathTagParent = new GameObject("SunPathTag");
        Vector3 winterSolPos = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().SunPostion(new DateTime(2010, 12, 21, 12, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone));
        Vector3 SummerSolPos = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().SunPostion(new DateTime(2010, 6, 21, 12, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone));

        //UpperHours
        for(int hourL = 0; hourL <24; hourL++)
        {
            Vector3 hourPos = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().SunPostion(new DateTime(2010, 6, 21, hourL, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone));
              GameObject sunTagHourSt = new GameObject("UpSunTag" + hourL.ToString());
            GameObject sunTagHourGO = Instantiate(GameObject.Find("SunTag"), hourPos, new Quaternion(0, 0, 0, 0));
            GameObject sunTagHourGC =  sunTagHourGO.transform.GetChild(0).gameObject;
            GameObject sunTagHourGCC = sunTagHourGC.transform.GetChild(0).gameObject;
            Text textSunTagHour = sunTagHourGCC.GetComponent<Text>();
            textSunTagHour.text = hourL + ":00";
            sunTagHourGO.transform.parent = sunTagHourSt.transform;
            sunTagHourSt.transform.parent = sunPathTagParent.transform;
        }
        // Lower Hours
        for (int hourL = 0; hourL < 24; hourL++)
        {
            Vector3 hourPos = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().SunPostion(new DateTime(2010, 12, 21, hourL, 0, 0), double.Parse(latitude), double.Parse(longitude), double.Parse(timeZone));
            GameObject sunTagHourSt = new GameObject("DownSunTag" + hourL.ToString());
            GameObject sunTagHourGO = Instantiate(GameObject.Find("SunTag"), hourPos, new Quaternion(0, 0, 0, 0));
            GameObject sunTagHourGC = sunTagHourGO.transform.GetChild(0).gameObject;
            GameObject sunTagHourGCC = sunTagHourGC.transform.GetChild(0).gameObject;
            Text textSunTagHour = sunTagHourGCC.GetComponent<Text>();
            textSunTagHour.text = hourL + ":00";
            sunTagHourGO.transform.parent = sunTagHourSt.transform;
            sunTagHourSt.transform.parent = sunPathTagParent.transform;
        }

        //GiveSomeSpace
        for (int hourL = 0; hourL < 24; hourL++)
        {
            Vector3 vectorBetween = GameObject.Find("UpSunTag" + hourL.ToString()).transform.GetChild(0).gameObject.transform.position - GameObject.Find("DownSunTag" + hourL.ToString()).transform.GetChild(0).gameObject.transform.position;
            GameObject.Find("UpSunTag" + hourL.ToString()).transform.GetChild(0).gameObject.transform.Translate(vectorBetween.normalized);
            GameObject.Find("DownSunTag" + hourL.ToString()).transform.GetChild(0).gameObject.transform.Translate((-vectorBetween.normalized));
           // Debug.Log(vectorBetween);
        }





            GameObject.Find("SunTag").SetActive(false);
    }

    void radVRSetAwake()
    {
        latitude = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().latitude;
        longitude = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().longitude;
        timeZone = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().timeZone;

    }

    void radVRSetUpdate()
    {
        month = GameObject.Find("Settings").GetComponent<radVRSettings>().month;
        day = GameObject.Find("Settings").GetComponent<radVRSettings>().day;
        hour = GameObject.Find("Settings").GetComponent<radVRSettings>().hour;
        minute = GameObject.Find("Settings").GetComponent<radVRSettings>().minute;


    }

}
