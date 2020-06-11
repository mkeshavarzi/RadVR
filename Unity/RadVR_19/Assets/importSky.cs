using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System;
using VRTK;

public class importSky : MonoBehaviour {


    public DateTime dateTimePreview;
    public GameObject sunPathObject;
    private GameObject radVR;

    private string latitude;
    private string longitude;
    private string meridian;
    private string month;
    private string day;
    private string hour;
    private string minute;

    private string simTimePrev;

    public string resolition;

    Process newProcess = new Process();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("p")){
            
            UpdateSkyRad();
        }
	}

    public void UpdateSkyRad()
    {
        prevSunProp();
        UnityEngine.Debug.Log("hour is:" + hour);

        try
        {
            //Pass the filepath and filename to the StreamWriter Constructor
            StreamWriter sw = new StreamWriter("D:\\MKeshavarzi\\RadVR\\Radiance Control\\Assets\\SkyImport\\skyInit.rad");

            //Write a line of text
            sw.WriteLine("!gensky "+month+ " " + day + " " + hour + ":"+minute+" +s -a "+latitude+ " -o " + longitude + " -m " + meridian + " |xform");
            sw.WriteLine("skyfunc glow skyglow");
            sw.WriteLine("0");
            sw.WriteLine("0");
            sw.WriteLine("4");
            sw.WriteLine(".09 .2 0.49 0");
            sw.WriteLine("");
            sw.WriteLine("skyglow source sky");
            sw.WriteLine("0");
            sw.WriteLine("0");
            sw.WriteLine("4");
            sw.WriteLine("0 0 1 180");
            sw.WriteLine("");
            sw.WriteLine("skyfunc glow ground_glow");
            sw.WriteLine("0");
            sw.WriteLine("0");
            sw.WriteLine("4");
            sw.WriteLine("0.099 0.099 0.11 0");
            sw.WriteLine("");
            sw.WriteLine("ground_glow source ground");
            sw.WriteLine("0");
            sw.WriteLine("0");
            sw.WriteLine("4");
            sw.WriteLine("0 0 -1 180");

            //Close the file
            sw.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }

        SkyRadCMD();
//        DynamicGI.UpdateEnvironment();

    }


    public void SkyRadCMD()
    {
        
        
        // Radiance Command Prompt
        string command1 = @" start explorer";
        string command3 = @" cd D:\MKeshavarzi\RadVR\Radiance Control\Assets\SkyImport";

        string c4 = @" oconv skyInit.rad > sky_sceneInit.oct";
        string c5 = @" 	rpict -t 15 -vf sky_viewCubeBox.vf -af sky_view.amb -x " + resolition + "-y " + resolition+" - ps 4 -pt .10 -pj .9 -dj .5 -ds .25 -dt .25 -dc .5 -dr 1 -dp 256 -st .5 -ab 4 -aa .2 -ar 256 -ad 2048 -as 1024 -lr 6 -lw .01 sky_sceneInit.oct > sky_view.overtured";
        string c6 = @" 	del sky_view.overture";
        string c7 = @" 	@ECHO+";
        string c8 = @"	@ECHO Running final image calculation no.1 of 1.";

        string c9 = @"	rpict -t 15 -vf sky_viewCubeBox.vf -af sky_view.amb -x " + resolition + " -y " + resolition + "  -ps 4 -pt .10 -pj .9 -dj .5 -ds .25 -dt .25 -dc .5 -dr 1 -dp 256 -st .5 -ab 4 -aa .2 -ar 256 -ad 2048 -as 1024 -lr 6 -lw .01 sky_sceneInit.oct > sky_view.unf";
        string c10 = @"	pfilt -r .6 -x /2 -y /2 sky_view.unf > sky_viewInit.hdr";
        string c11 = @"	del sky_view.unf";
        string c12 = @"exit";

        ProcessStartInfo newstartInfo = new ProcessStartInfo();
        newstartInfo.FileName = "cmd";
        newstartInfo.Verb = "runas";
        newstartInfo.RedirectStandardInput = true;
//        newstartInfo.CreateNoWindow = true;
        newstartInfo.UseShellExecute = false; //The Process object must have the UseShellExecute property set to false in order to redirect IO streams.

        newProcess = new Process();

        newProcess.StartInfo = newstartInfo;
        newProcess.Start();
        StreamWriter write = newProcess.StandardInput; //Using the Streamwriter to write to the elevated command prompt.
        write.WriteLine(command1); //First command executes in elevated command prompt
                                   //     write.WriteLine(command2);
        write.WriteLine(command3);
        write.WriteLine(c4);
        write.WriteLine(c5);
        write.WriteLine(c6);
        write.WriteLine(c7);
        write.WriteLine(c8);
        write.WriteLine(c9);
        write.WriteLine(c10);
        write.WriteLine(c11);
        write.WriteLine(c12);
        newProcess.WaitForExit();
    }



    void prevSunProp()//copied from textConsole.cs
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

    string TwoDigStr(float num)//copied from textConsole.cs
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
