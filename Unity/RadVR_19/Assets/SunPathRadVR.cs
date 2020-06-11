using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SunPathRadVR : MonoBehaviour {

    public Boolean vrOn = true;

    //RadVRGlobalSettings
    private bool sunChanging = false;
    private bool SunMinUp = false;
    private bool SunMinDown = false;
    private bool SunDayUp = false;
    private bool SunDayDown = false;
    public bool snapTime = false;


    public int monthOfYear = 9;
    public int dayOfMonth = 21;
    public string localTimeHours;
    public string localTimeMinutes;
    public string timeZone;
    public string latitude;
    public string longitude;

    public GameObject sunPosTest; 

    private Double latitudeDouble;
    private Double longitudeDouble;
    private Double localTimeHoursDouble;
    private Double localTimeMinutesDouble;
    private Double dayOfYearDouble;
    private Double timeZoneDouble; //Still indicates days
    private Double julianDay;
    private Double julianDay2010;
    private Double julianCentury;
    private Double meanObliqEcliptic;
    private Double obliqCorr;
    private Double varY;
    private Double geomMeanLongSun;
    private Double geomMeanAnomSun;
    private Double eccentEarthOrbit;
    private Double eqOfTime;
    private Double trueSolarTime;
    private Double hourAngle;
    private Double sunEqOfCtr;
    private Double sunTrueLong;
    private Double sunAppLong;
    private Double sunDeclin;

    private Double zenith;
    private Double solarElevationAngle;
    private Double azimuth;

    private Quaternion sunDir;
    private Vector3 sunDirVec;

    public GameObject sun;

    private float zenithF;
    private float solarElevationAngleF;
    private float azimuthF;

    private float hours;
    private float day;

    public bool hideErConflic;

    private float solarElevationAngleFBefore;
    private float azimuthFBefore;
    private float solarElevationAngleFChange;
    private float azimuthFChange;
    private float zForSake;

    private DateTime dateOfYear;
    public DateTime dateTimePreview;

    public Material DarkRed;
    public Material DarkBlue;
    public Material LightRed;
    public Material LightBlue;

    float SPstartWidth = 0.1f;
    float SPendWidth = 0.1f;
    float SPHoverMul = 2;

    public float sunPathRadius = 25;


    private VRTK_ControllerEvents contEvRight;
    private VRTK_ControllerEvents contEvLeft;
    public GameObject contRight;
    public GameObject contLeft;
    public GameObject eyePosition;
    private Vector3 contRightPosition;
    private Quaternion contRightRotation;

    public int resWidth = 500;
    public int resHeight = 500;
    private bool takeHiResShot = false;
    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }



    // Use this for initialization
    private void Awake()
    {

        if (vrOn == true){      
            contEvRight = contRight.GetComponent<VRTK_ControllerEvents>(); //change this if settings have changed
            contEvLeft = contLeft.GetComponent<VRTK_ControllerEvents>(); //change this if settings0 have changed
        }


        dateOfYear = new DateTime(2010, monthOfYear, dayOfMonth, int.Parse(localTimeHours), int.Parse(localTimeMinutes), 0);
        localTimeHoursDouble = Double.Parse(dateOfYear.TimeOfDay.Hours.ToString());
        localTimeMinutesDouble = Double.Parse(dateOfYear.TimeOfDay.Minutes.ToString());
        dayOfYearDouble = Double.Parse(dateOfYear.DayOfYear.ToString());
        timeZoneDouble = Double.Parse(timeZone);
        latitudeDouble = Double.Parse(latitude);
        longitudeDouble = Double.Parse(longitude);
        julianDay2010 = 2455197.50;

        solarElevationAngleFBefore = 0;
        azimuthFBefore = 0;
        zForSake = 0;

        SunPathMonth(latitudeDouble, longitudeDouble, timeZoneDouble);
        SunPathHour(latitudeDouble, longitudeDouble, timeZoneDouble);
        HideError();
        sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
    }
	
	// Update is called once per frame
	void Update ()
    {
        radVRSetUpdate();
        KeyBoardUpdate();

        if (vrOn == true)
        {
            vrtkUpdate();
            SunPathEyePosUpdate();
        }

    }


    public void sunUpdate(DateTime dateOfYearSU, double latitudeDoubleSU, double longitudeDoubleSU, double timeZoneDoubleSU)
    {

        //Update Time
        localTimeHoursDouble = Double.Parse(dateOfYearSU.TimeOfDay.Hours.ToString());
        localTimeMinutesDouble = Double.Parse(dateOfYearSU.TimeOfDay.Minutes.ToString());
        dayOfYearDouble = Double.Parse(dateOfYearSU.DayOfYear.ToString());

        dateTimePreview = dateOfYearSU;

        // Julian Day
        julianDay = (((localTimeMinutesDouble / 60) + localTimeHoursDouble - timeZoneDoubleSU) / 24) + dayOfYearDouble + julianDay2010;

        // Julian Century
        julianCentury = (julianDay - 2451545) / 36525;

        meanObliqEcliptic = 23 + (26 + ((21.448 - julianCentury * (46.815 + julianCentury * (0.00059 - julianCentury * 0.001813)))) / 60) / 60;
        obliqCorr = meanObliqEcliptic + 0.00256 * Math.Cos((Math.PI / 180) * (125.04 - 1934.136 * julianCentury));
        varY = Math.Tan((Math.PI / 180) * (obliqCorr / 2)) * Math.Tan((Math.PI / 180) * (obliqCorr / 2));
        geomMeanLongSun = 280.46646 + julianCentury * (36000.76983 + julianCentury * 0.0003032) % 360;
        geomMeanAnomSun = 357.52911 + julianCentury * (35999.05029 - 0.0001537 * julianCentury);
        eccentEarthOrbit = 0.016708634 - julianCentury * (0.000042037 + 0.0000001267 * julianCentury);
        eqOfTime = 4 * (Math.PI / 180) / (varY * Math.Sin(2 * (Math.PI / 180) * (geomMeanLongSun)) - 2 * eccentEarthOrbit * Math.Sin((Math.PI / 180) * (geomMeanAnomSun)) + 4 * eccentEarthOrbit * varY * Math.Sin((Math.PI / 180) * (geomMeanAnomSun)) * Math.Cos(2 * (Math.PI / 180) * (geomMeanLongSun)) - 0.5 * varY * varY * Math.Sin(4 * (Math.PI / 180) * (geomMeanLongSun)) - 1.25 * eccentEarthOrbit * eccentEarthOrbit * Math.Sin(2 * (Math.PI / 180) * (geomMeanAnomSun)));
        eqOfTime = 4 * (varY * Math.Sin(2 * (Math.PI / 180) * (geomMeanLongSun)) - 2 * eccentEarthOrbit * Math.Sin((Math.PI / 180) * (geomMeanAnomSun)) + 4 * eccentEarthOrbit * varY * Math.Sin((Math.PI / 180) * (geomMeanAnomSun)) * Math.Cos(2 * (Math.PI / 180) * (geomMeanLongSun)) - 0.5 * varY * varY * Math.Sin(4 * (Math.PI / 180) * (geomMeanLongSun)) - 1.25 * eccentEarthOrbit * eccentEarthOrbit * Math.Sin(2 * (Math.PI / 180) * (geomMeanAnomSun))) / (Math.PI / 180);
        trueSolarTime = ((((localTimeMinutesDouble / 60) + localTimeHoursDouble) / 24) * 1440 + eqOfTime + 4 * longitudeDoubleSU - 60 * timeZoneDoubleSU) % 1440;
       
        //hourAngle:
        if (trueSolarTime / 4 < 0)
        {
            hourAngle = trueSolarTime / 4 + 180;
        }
        else
        {
            hourAngle = trueSolarTime / 4 - 180;
        }

        sunEqOfCtr = Math.Sin((Math.PI / 180) * (geomMeanAnomSun)) * (1.914602 - julianCentury * (0.004817 + 0.000014 * julianCentury)) + Math.Sin((Math.PI / 180) * (2 * geomMeanAnomSun)) * (0.019993 - 0.000101 * julianCentury) + Math.Sin((Math.PI / 180) * (3 * geomMeanAnomSun)) * 0.000289;
        sunTrueLong = geomMeanLongSun + sunEqOfCtr;
        sunAppLong = sunTrueLong - 0.00569 - 0.00478 * Math.Sin((Math.PI / 180) * (125.04 - 1934.136 * julianCentury));
        sunDeclin = (Math.Asin(Math.Sin((Math.PI / 180) * (obliqCorr)) * Math.Sin((Math.PI / 180) * (sunAppLong)))) / (Math.PI / 180);

        zenith = (Math.Acos(Math.Sin((Math.PI / 180) * (latitudeDoubleSU)) * Math.Sin((Math.PI / 180) * (sunDeclin)) + Math.Cos((Math.PI / 180) * (latitudeDoubleSU)) * Math.Cos((Math.PI / 180) * (sunDeclin)) * Math.Cos((Math.PI / 180) * (hourAngle)))) / (Math.PI / 180);
        solarElevationAngle = 90 - zenith;
        //      = DEGREES(ACOS(SIN((Math.PI / 180) *($B$2)) * SIN((Math.PI / 180) *(T2)) + COS((Math.PI / 180) *($B$2)) * COS((Math.PI / 180) *(T2)) * COS((Math.PI / 180) *(AC2))))

        //azimuth
        if (hourAngle > 0)
        {
            azimuth = (((Math.Acos(((Math.Sin((Math.PI / 180) * (latitudeDoubleSU)) * Math.Cos((Math.PI / 180) * (zenith))) - Math.Sin((Math.PI / 180) * (sunDeclin))) / (Math.Cos((Math.PI / 180) * (latitudeDoubleSU)) * Math.Sin((Math.PI / 180) * (zenith))))) / (Math.PI / 180)) + 180) % 360;
        }
        else
        {
            azimuth = (540 - (Math.Acos(((Math.Sin((Math.PI / 180) * (latitudeDoubleSU)) * Math.Cos((Math.PI / 180) * (zenith))) - Math.Sin((Math.PI / 180) * (sunDeclin))) / (Math.Cos((Math.PI / 180) * (latitudeDoubleSU)) * Math.Sin((Math.PI / 180) * (zenith))))) / (Math.PI / 180)) % (360);
        };

        zenithF = (float)zenith;
        azimuthF = (float)azimuth;
        solarElevationAngleF = (float)solarElevationAngle;

        solarElevationAngleFChange = solarElevationAngleF - solarElevationAngleFBefore;
        azimuthFChange = azimuthF - azimuthFBefore;

        sun.transform.eulerAngles = new Vector3(solarElevationAngleF, azimuthF, zForSake);

        Vector3 sunPosVec;
        double SPVx = Math.Sin(Math.PI / 180 * zenithF) * Math.Cos(Math.PI / 180 * azimuthF) * sunPathRadius;
        double SPVy = Math.Sin(Math.PI / 180 * zenithF) * Math.Sin(Math.PI / 180 * azimuthF) * sunPathRadius;
        double SPVz = Math.Cos(Math.PI / 180 * zenithF) * sunPathRadius;
        sunPosVec = new Vector3(-(float)SPVy, (float)SPVz, -(float)SPVx);

        if (vrOn == true)
        {
            sunPosTest.transform.position = sunPosVec + eyePosition.transform.position;
        }
        else
        {
            sunPosTest.transform.position = sunPosVec;
        }

        solarElevationAngleFBefore = solarElevationAngleF;
        azimuthFBefore = azimuthF;
  

    }

    void KeyBoardUpdate()
    {
        if (SunMinUp)
        {
            dateOfYear = dateOfYear.AddMinutes(2);

            if (snapTime == false)
            {
                sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
                SunPathLineUpdate();
            }
            if (snapTime== true)
            {
                if (dateOfYear.Minute == 0)
                {
                    dateOfYear = dateOfYear.AddDays(21 - (dateOfYear.Day));
                    sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
                    SunPathLineUpdate();
                }
            }
        }


        if (SunMinDown)
        {
            dateOfYear = dateOfYear.AddMinutes(-2);

            if (snapTime == false)
            {
                sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
                SunPathLineUpdate();
            }
            if (snapTime == true)
            {
                if (dateOfYear.Minute == 0)
                {
                    dateOfYear = dateOfYear.AddDays(21 - (dateOfYear.Day));
                    sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
                    SunPathLineUpdate();
                }
            }
        }

        if (SunDayUp)
        {
            dateOfYear = dateOfYear.AddDays(1);

            if (snapTime == false)
            {
                sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
                SunPathLineUpdate();
            }
            if (snapTime == true)
            {
                if (dateOfYear.Day == 21)
                {
                    dateOfYear = dateOfYear.AddMinutes(0-(dateOfYear.Minute));
                    sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
                    SunPathLineUpdate();
                }
            }
        }


        if (SunDayDown)
        {
            dateOfYear = dateOfYear.AddDays(-1);

            if (snapTime == false)
            {
                sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
        //        SunPathLineUpdate();
            }
            if (snapTime == true)
            {
                if (dateOfYear.Day == 21)
                {
                    dateOfYear = dateOfYear.AddMinutes(0 - (dateOfYear.Minute));
                    sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
           //         SunPathLineUpdate();
                }
            }
        }

    }
    void vrtkUpdate()
    {
 /*       if (contEvRight.triggerPressed)
        {
            dateOfYear = dateOfYear.AddMinutes(2);
            sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
            SunPathLineUpdate();
        }
        if (contEvRight.gripPressed)
        {
            dateOfYear = dateOfYear.AddMinutes(-2);
            sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
            SunPathLineUpdate();
        }

        if (contEvLeft.triggerPressed)
        {
            dateOfYear = dateOfYear.AddDays(2);
            sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
            SunPathLineUpdate();
        }

        if (contEvLeft.gripPressed)
        {
            dateOfYear = dateOfYear.AddDays(-2);
            sunUpdate(dateOfYear, latitudeDouble, longitudeDouble, timeZoneDouble);
            SunPathLineUpdate();
        }
     */
    }

    

    public void TakeHiResShot()
    {
        takeHiResShot = true;
    }



    public Vector3 SunPostion(DateTime dateOfYearSP, double latitudeDoubleSP, double longitudeDoubleSP, double timeZoneDoubleSP)
    {

        sunUpdate(dateOfYearSP, latitudeDoubleSP, longitudeDoubleSP, timeZoneDoubleSP);
        //        sunPosTest.transform.position = new Vector3(0 , 20 * (float)(Math.Sin((Math.PI / 180) * (solarElevationAngleF))), (20 * (float)(Math.Cos((Math.PI / 180) * (solarElevationAngleF)))-20));
        //        sunPosTest.transform.position = new Vector3( -20* (float)(Math.Sin((Math.PI / 180) * (azimuthF))), +20 , -20 * (float)(Math.Cos((Math.PI / 180) * (azimuthF))));

        Vector3 sunPosVec;
        double SPVx = Math.Sin(Math.PI / 180 * zenithF) * Math.Cos(Math.PI / 180 * azimuthF)* sunPathRadius;
        double SPVy = Math.Sin(Math.PI / 180 * zenithF) * Math.Sin(Math.PI / 180 * azimuthF)* sunPathRadius;
        double SPVz = Math.Cos(Math.PI / 180 * zenithF) * sunPathRadius;
        sunPosVec = new Vector3(-(float)SPVy, (float)SPVz, -(float)SPVx);

        return sunPosVec;
    }


    public void SunPathMonth(double latitudeDoubleSPM, double longitudeDoubleSPM, double timeZoneDoubleSPM)
    {
        GameObject sunPathMonthParent = new GameObject("SunPathMonth");
        for (int month = 1; month<13; month++)
        {
            GameObject sunPathMonthGO = new GameObject("SunPathMonth"+month.ToString());
            float red = (1-(Math.Abs((month - 6f) / 6f)) * (255f / 255f));
            float green = (1-(Math.Abs((month - 6f) / 6f)) * (69f / 255f));
            float blue = ((Math.Abs((month - 6f) / 6f)) * (195f / 255f));
            Color lineCol = new Color(red, green, blue);
            Vector3 col = new Vector3(red, green, blue);
            //Debug.Log("MONTH" + month.ToString() + col);
            LineRenderer lineRenderer = sunPathMonthGO.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
            lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            lineRenderer.startColor = lineCol;
            lineRenderer.endColor = lineCol;
            lineRenderer.startWidth = SPstartWidth;
            lineRenderer.endWidth = SPendWidth;
            lineRenderer.SetVertexCount(24*30);
            Vector3[] sunPathMonthV3 = new Vector3[24*30];


            for (int hour = 0; hour < 24; hour++)
            {
                int monthNum = 21;
                if (month == 1) { monthNum = 21; }
                if (month == 2) { monthNum = 18; }
                if (month == 3) { monthNum = 20; }
                if (month == 4) { monthNum = 19; }
                if (month == 5) { monthNum = 21; }
                if (month == 6) { monthNum = 21; }
                if (month == 7) { monthNum = 21; }
                if (month == 8) { monthNum = 23; }
                if (month == 9) { monthNum = 22; }
                if (month == 10) { monthNum = 22; }
                if (month == 11) { monthNum = 21; }
                if (month == 12) { monthNum = 22; }
                for (int minuteFrac = 0; minuteFrac<30; minuteFrac++)
                {


                    DateTime sunPathMonthDT = new DateTime(2010, month, monthNum, hour, minuteFrac*2, 0);
                    sunPathMonthV3[(hour*30)+minuteFrac] = SunPostion(sunPathMonthDT, latitudeDoubleSPM, longitudeDoubleSPM, timeZoneDoubleSPM);
                }
            }

            lineRenderer.SetPositions(sunPathMonthV3);
            lineRenderer.useWorldSpace = false;
            sunPathMonthGO.transform.parent = sunPathMonthParent.transform;
        }
    }


    public void SunPathHour(double latitudeDoubleSPH, double longitudeDoubleSPH, double timeZoneDoubleSPH)
    {
        GameObject sunPathHourParent = new GameObject("SunPathHour");
        for (int hour = 0; hour < 24; hour++)
        {
            GameObject sunPathHourGO = new GameObject("SunPathHour"+hour.ToString());
            LineRenderer lineRenderer = sunPathHourGO.AddComponent<LineRenderer>();
            lineRenderer.material = DarkBlue;
            lineRenderer.startWidth = SPstartWidth;
            lineRenderer.endWidth = SPendWidth;
            lineRenderer.SetVertexCount(12*28);
            Vector3[] sunPathHourV3 = new Vector3[12*28];


            for (int month = 1; month < 13; month++)
            {
                for (int dayFrac = 1; dayFrac < 29; dayFrac++)
                {
                    DateTime sunPathHourDT = new DateTime(2010, month, dayFrac, hour, 0, 0);
                    sunPathHourV3[(month - 1)*28+(dayFrac-1)] = SunPostion(sunPathHourDT, latitudeDoubleSPH, longitudeDoubleSPH, timeZoneDoubleSPH);
                }
            }

            lineRenderer.SetPositions(sunPathHourV3);
            lineRenderer.useWorldSpace = false;
            sunPathHourGO.transform.parent = sunPathHourParent.transform;
        }
    }

    void HideError()
    {
        if (hideErConflic == true)
        {

            GameObject.Find("SunPathMonth1").GetComponent<LineRenderer>().enabled = !hideErConflic;
            GameObject.Find("SunPathMonth2").GetComponent<LineRenderer>().enabled = !hideErConflic;
            GameObject.Find("SunPathMonth3").GetComponent<LineRenderer>().enabled = !hideErConflic;
            GameObject.Find("SunPathMonth4").GetComponent<LineRenderer>().enabled = !hideErConflic;
            GameObject.Find("SunPathMonth5").GetComponent<LineRenderer>().enabled = !hideErConflic;

        }
    }

    public void SunPathLineUpdate()
    {
     /*   int month;
        int hour;
        month = dateOfYear.Month - 1;
        hour = dateOfYear.Hour;



        if (month > 0)
        {
            GameObject.Find("SunPathMonth" + ((month - 1) + 1).ToString()).GetComponent<LineRenderer>().material = DarkRed;
            GameObject.Find("SunPathMonth" + (month+1).ToString()).GetComponent<LineRenderer>().material = LightRed;
            GameObject.Find("SunPathMonth" + (((month + 1) % 12) + 1).ToString()).GetComponent<LineRenderer>().material = LightRed;
            GameObject.Find("SunPathMonth" + (((month + 2) % 12) + 1).ToString()).GetComponent<LineRenderer>().material = DarkRed;

            GameObject.Find("SunPathMonth" + ((month - 1) + 1).ToString()).GetComponent<LineRenderer>().startWidth = SPstartWidth;
            GameObject.Find("SunPathMonth" + (month + 1).ToString()).GetComponent<LineRenderer>().startWidth = SPstartWidth * SPHoverMul;
            GameObject.Find("SunPathMonth" + (((month + 1) % 12) + 1).ToString()).GetComponent<LineRenderer>().startWidth = SPstartWidth * SPHoverMul;
            GameObject.Find("SunPathMonth" + (((month + 2) % 12) + 1).ToString()).GetComponent<LineRenderer>().startWidth = SPstartWidth;

            GameObject.Find("SunPathMonth" + ((month - 1) + 1).ToString()).GetComponent<LineRenderer>().endWidth = SPstartWidth;
            GameObject.Find("SunPathMonth" + (month + 1).ToString()).GetComponent<LineRenderer>().endWidth = SPstartWidth * SPHoverMul;
            GameObject.Find("SunPathMonth" + (((month + 1) % 12) + 1).ToString()).GetComponent<LineRenderer>().endWidth = SPstartWidth * SPHoverMul;
            GameObject.Find("SunPathMonth" + (((month + 2) % 12) + 1).ToString()).GetComponent<LineRenderer>().endWidth = SPstartWidth;
        }
        else
        {
            GameObject.Find("SunPathMonth" + 12.ToString()).GetComponent<LineRenderer>().material = DarkRed;
            GameObject.Find("SunPathMonth" + (month + 1).ToString()).GetComponent<LineRenderer>().material = LightRed;
            GameObject.Find("SunPathMonth" + (((month + 1) % 12) + 1).ToString()).GetComponent<LineRenderer>().material = LightRed;
            GameObject.Find("SunPathMonth" + (((month + 2) % 12) + 1).ToString()).GetComponent<LineRenderer>().material = DarkRed;

            GameObject.Find("SunPathMonth" + 12.ToString()).GetComponent<LineRenderer>().startWidth = SPstartWidth;
            GameObject.Find("SunPathMonth" + (month + 1).ToString()).GetComponent<LineRenderer>().startWidth = SPstartWidth * SPHoverMul;
            GameObject.Find("SunPathMonth" + (((month + 1) % 12) + 1).ToString()).GetComponent<LineRenderer>().startWidth = SPstartWidth * SPHoverMul;
            GameObject.Find("SunPathMonth" + (((month + 2) % 12) + 1).ToString()).GetComponent<LineRenderer>().startWidth = SPstartWidth;

            GameObject.Find("SunPathMonth" + 12.ToString()).GetComponent<LineRenderer>().endWidth = SPstartWidth;
            GameObject.Find("SunPathMonth" + (month + 1).ToString()).GetComponent<LineRenderer>().endWidth = SPstartWidth * SPHoverMul;
            GameObject.Find("SunPathMonth" + (((month + 1) % 12) + 1).ToString()).GetComponent<LineRenderer>().endWidth = SPstartWidth * SPHoverMul;
            GameObject.Find("SunPathMonth" + (((month + 2) % 12) + 1).ToString()).GetComponent<LineRenderer>().endWidth = SPstartWidth;
        }

        GameObject.Find("SunPathHour" + (hour - 1).ToString()).GetComponent<LineRenderer>().material = DarkBlue;
        GameObject.Find("SunPathHour" + hour.ToString()).GetComponent<LineRenderer>().material = LightBlue;
        GameObject.Find("SunPathHour" + ((hour+1)%24).ToString()).GetComponent<LineRenderer>().material = LightBlue;
        GameObject.Find("SunPathHour" + ((hour + 2)%24).ToString()).GetComponent<LineRenderer>().material = DarkBlue;

    */
    }

    void SunPathEyePosUpdate()
    {
        GameObject.Find("SunPathHour").transform.position = eyePosition.transform.position;
        GameObject.Find("SunPathMonth").transform.position = eyePosition.transform.position;
        GameObject.Find("SunPathTag").transform.position = eyePosition.transform.position;

    }


    void radVRSetUpdate()
    {
       sunChanging = GameObject.Find("Settings").GetComponent<radVRSettings>().sunChanging;
        SunMinUp = GameObject.Find("Settings").GetComponent<radVRSettings>().SunMinUp;
        SunMinDown = GameObject.Find("Settings").GetComponent<radVRSettings>().SunMinDown;
        SunDayUp = GameObject.Find("Settings").GetComponent<radVRSettings>().SunDayUp;
        SunDayDown = GameObject.Find("Settings").GetComponent<radVRSettings>().SunDayDown;
        snapTime = GameObject.Find("Settings").GetComponent<radVRSettings>().snapTime;
    }
}
