using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System;
using VRTK;

public class runRadiance : MonoBehaviour {

    //RadVRGlobalSettings
    private bool level1 = false;
    private bool level2 = false;
    private bool level3 = false;
    private bool simulationEx = false;
    private bool DFsimulationEx = false;


    private float topLeg;
    private float midLeg;
    private float lowLeg;

    private radVRSettings.LevelAlias levelStatus;

    public GameObject parentIGrid;
    public GameObject parentINumTag;
    private GameObject parentNumTag;
    private GameObject parentGrid;
    public GameObject radWorld;
    public GameObject sunPathObject;
    public DateTime dateTimePreview;
    private float gridSize;
    private string latitude;
    private string longitude;
    private string meridian;
    private string month;
    private string day;
    private string hour;
    private string minute;

    private string buildingGOS;

    private float startTime;
    private float endTime;
    public string lastSimulationTime;

  

    private GameObject numTag;

    private VRTK_ControllerEvents contEvRight;
    private VRTK_ControllerEvents contEvLeft;


    private string[] nodes;

    float surfHeight;

    Boolean previewSwitch;
    public Boolean resultTag;

    Process newProcess = new Process();

    private Boolean vrOn;

    private int nodeCount2;

    //Properties
    public string ambientBounces;

    // Use this for initialization
    private void Awake()
    {
        contEvRight = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().contRight.GetComponent<VRTK_ControllerEvents>();
        contEvLeft = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().contLeft.GetComponent<VRTK_ControllerEvents>();
        vrOn = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().vrOn;
    }

    void Start () {

        previewSwitch = false;
        numTag = GameObject.Find("NumTag");
        radVRSetUpdate();
         ExecuteRad("ill");// Sends to Radiance
        radWorld.transform.Rotate(new Vector3(1, 0, 0), -90);
//       gameObject.GetComponent<importSky>().UpdateSkyRad();//Let's Think about this

    }
	
	// Update is called once per frame
	void Update () {
        radVRSetUpdate();
        LevelUpdate();

    }

    void LevelUpdate()
    {
        if(levelStatus == radVRSettings.LevelAlias.Level1)
        {

        }
        if (levelStatus == radVRSettings.LevelAlias.Level2)
        {
            if ((simulationEx == true || DFsimulationEx == true) && newProcess.HasExited)
            {
                UnityEngine.Debug.Log("WWWWWWWWWWWWWWWWWWWWWWWWWWWWW");
                string simType = "ill";
                if (simulationEx == true) { simType = "ill"; }// Sends to Radiance
                if (DFsimulationEx == true) { simType = "DF"; }// Sends to Radiance
                DestroyPrevResults();
                gameObject.GetComponent<gridSettings>().WriteNodesUpdate(@"d:\RadVR\nodes.pts");
                ExecuteRad(simType);
                //gameObject.GetComponent<importSky>().UpdateSkyRad();
                if (previewSwitch == true)
                {
                    PreviewResults(@"d:\RadVR\nodes.pts", @"d:\RadVR\results.dat", simType);
                }
            }
        }
        if (levelStatus == radVRSettings.LevelAlias.Level3)
        {
            if ((simulationEx == true || DFsimulationEx == true) && newProcess.HasExited)
            {
                string simType = "ill";
                if (simulationEx == true) { simType = "ill"; }// Sends to Radiance
                if (DFsimulationEx == true) { simType = "ill"; }// Sends to Radiance
                DestroyPrevResults();
                gameObject.GetComponent<gridSettings>().WriteNodesUpdate(@"d:\RadVR\nodes.pts");
                ExecuteRad(simType);
                //gameObject.GetComponent<importSky>().UpdateSkyRad();
                if (previewSwitch == true)
                {
                    PreviewResults(@"d:\RadVR\nodes.pts", @"d:\RadVR\results.dat", simType);
                }
            }
        }
    }


    void DestroyPrevResults()
    {
        Destroy(parentGrid);
        Destroy(parentNumTag);
    }

    void ExecuteRad(string simType)
    {
        //Initialize Timer
        Stopwatch watch = new Stopwatch();
        watch.Start();


        //Date Time Implementation
        dateTimePreview = sunPathObject.GetComponent<SunPathRadVR>().dateTimePreview;
        month = TwoDigStr(dateTimePreview.Month);
        day = TwoDigStr(dateTimePreview.Day);
        hour = TwoDigStr(dateTimePreview.Hour);
        minute = TwoDigStr(dateTimePreview.Minute);
        latitude = sunPathObject.GetComponent<SunPathRadVR>().latitude;
        longitude = sunPathObject.GetComponent<SunPathRadVR>().longitude;
        meridian = (float.Parse(sunPathObject.GetComponent<SunPathRadVR>().timeZone) * 15).ToString();



        UnityEngine.Debug.Log(" gensky " + month + " " + day + " " + hour + ":" + minute + " +s -a " + latitude + " -o " + longitude + " -m " + meridian + " > aSky.rad");
        UnityEngine.Debug.Log("Month:" + month + "Day:" + day + "Time:" + hour + ":" + minute);
        

        // Radiance Command Prompt
        string command1 = @" start explorer";
        string command2 = @"D:";
        string command3 = @" cd D:\RadVR\";
        string c4 = @" obj2rad -f -m materialsToMap.map "+ buildingGOS + ".obj > aBuilding.rad";//was SCube6.obj or UncleanedSeattle01.obj
        string c5 = "";
        string c6 = "";

        if (simType == "ill")
        {
            c5 = @" gensky " + month + " " + day + " " + hour + ":" + minute + " +s -a " + latitude + " -o " + longitude + " -m " + meridian + " > aSky.rad";
            c6 = @" oconv aSky.rad material.rad aBuilding.rad > aScene.oct";
        }

        if (simType == "DF")
        {
            c5 = @" oconv cieOvercastSky.rad material.rad aBuilding.rad > aScene.oct";
            c6 = @" oconv cieOvercastSky.rad material.rad aBuilding.rad > aScene.oct";
        }

        string c7 = @" rtrace -I -h -dp 2048 -ms 0.063 -ds .2 -dt .05 -dc .75 -dr 3 -st .01 -lr 12 -lw .0005 -ab "+ambientBounces+@" -ad 1000 -as 256 -ar 300 -aa 0.1 D:\RadVR\aScene.oct < D:\RadVR\nodes.pts > D:\RadVR\results.dat";
        string c8 = @"exit";

        ProcessStartInfo newstartInfo = new ProcessStartInfo();
        newstartInfo.FileName = "cmd";
        newstartInfo.Verb = "runas";
        newstartInfo.RedirectStandardInput = true;
        newstartInfo.UseShellExecute = false; //The Process object must have the UseShellExecute property set to false in order to redirect IO streams.

        newProcess = new Process();

        newProcess.StartInfo = newstartInfo;
        newProcess.Start();
        StreamWriter write = newProcess.StandardInput; //Using the Streamwriter to write to the elevated command prompt.
        write.WriteLine(command1); //First command executes in elevated command prompt
                                   //     write.WriteLine(command2);
        write.WriteLine(command2);
        write.WriteLine(command3);
        write.WriteLine(c4);
        write.WriteLine(c5);
        write.WriteLine(c6);
        write.WriteLine(c7);
        write.WriteLine(c8);
        newProcess.WaitForExit();

        //Preview Switch
        previewSwitch = true;

        //Do things
       watch.Stop();
        lastSimulationTime = watch.Elapsed.TotalSeconds.ToString();



    }

    void PreviewResults(string nodeFilePath, string resultFilePath, string simType)
    {
        parentGrid = Instantiate(parentIGrid, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        parentNumTag = Instantiate(parentINumTag, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));


        // read nodes
        string[] nodes = File.ReadAllLines(nodeFilePath);
        Vector3[] nodeVector3 = new Vector3[nodes.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            string[] sublines = nodes[i].Split(' ');
            nodeVector3[i] = new Vector3((float)(double.Parse(sublines[0])*(-1)), (float)(double.Parse(sublines[2])), (float)(double.Parse(sublines[1]) * (-1)));
        }

        // read results
        string[] results = File.ReadAllLines(resultFilePath);
        float[] illuminanceResults = new float[results.Length];
        for (int i = 0; i < results.Length; i++)
        {
            string[] sublines = results[i].Split('	');
            illuminanceResults[i] = (float)((double.Parse(sublines[0]) * 0.265 + double.Parse(sublines[1]) * 0.67 + double.Parse(sublines[2]) * 0.065) * 179);
            if (simType == "DF")
            { illuminanceResults[i] = illuminanceResults[i] / 179; }
   //         UnityEngine.Debug.Log(illuminanceResults[i]);
        }

        //make and color nodes
        GameObject node = GameObject.CreatePrimitive(PrimitiveType.Cube);
 //       GameObject node = GameObject.CreatePrimitive(PrimitiveType.s);
        gridSize =  gameObject.GetComponent<gridSettings>().gridSize;
        gridSize = gridSize * 0.95f;
        node.transform.localScale = new Vector3(gridSize, 0.1f, gridSize);
        node.GetComponent<BoxCollider>().enabled = false;

        //find result max
        float illMax = illuminanceResults.Max();
        GameObject.Find("Settings").GetComponent<radVRSettings>().highVal = illMax;

        if (simType == "DF")
        {
            GameObject.Find("Settings").GetComponent<radVRSettings>().highVal = 10;
        }


        for (int i = 0; i < nodeVector3.Length; i++)
        {
            GameObject cubeHolder = new GameObject(Mathf.Round(illuminanceResults[i]).ToString());
            GameObject insCube = Instantiate(node, nodeVector3[i], new Quaternion(0, 0, 0, 0));
            GameObject.Find("Settings").GetComponent<colorNavigation>().colorChangeUpdate();
            
                        //Color cubeCol;
                        //float red;
                        //float green;
                        //float blue;
                        //float alpha = 0.3f;
                        //if (illuminanceResults[i] > 1000)
                        //{
                        //    cubeCol = new Color(1, 0, 0,alpha);
                        //}
                        //else
                        //{
                        //    if(illuminanceResults[i] > 500)
                        //    {

                        //        red = Math.Abs(illuminanceResults[i] - 500) / 500;
                        //        green = Math.Abs(500 - illuminanceResults[i] - 500) / 500;
                        //        blue = 0;
                        //        cubeCol = new Color(red, green, blue,alpha);
                        //    }
                        //    else
                        //    {
                        //        red = 0;
                        //        green = Math.Abs(illuminanceResults[i]) / 500;
                        //        blue = Math.Abs(500 - illuminanceResults[i]) / 500;
                        //        cubeCol = new Color(red, green, blue, alpha);
                        //    }
                        //}

                      
                        
            
            Renderer rend = insCube.GetComponent<Renderer>();
            //rend.material.color = cubeCol;


            rend.material.shader = Shader.Find("Transparent/Diffuse");
            insCube.transform.parent = cubeHolder.transform;
            cubeHolder.transform.parent = parentGrid.transform;

            GameObject numTagResNu = new GameObject("NumTag"+Mathf.Round(illuminanceResults[i]).ToString());
            GameObject numTagRes = Instantiate(numTag, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            Text numTagText = numTagRes.GetComponentInChildren<Text>();
            numTagText.text = Mathf.Round(illuminanceResults[i]).ToString();
            numTagRes.transform.parent = numTagResNu.transform;
            numTagResNu.transform.position = nodeVector3[i];
            numTagResNu.transform.Rotate(new Vector3(1, 0, 0), 90);
            numTagResNu.transform.parent = parentNumTag.transform;

            if (i == 0) { surfHeight = insCube.transform.position.z; }
        }

        //Rotate Objects & Turn off Cube
        node.SetActive(false);
  //      parentGrid.transform.Rotate (new Vector3(1, 0, 0), -90);
  //     parentGrid.transform.Rotate(new Vector3(0, 1, 0), 180);
  //      parentGrid.transform.position = new Vector3(0, surfHeight * 2f, 0);

        previewSwitch = false;

        //Timer
        parentGrid.transform.Translate(new Vector3(0, 0.01f, 0));
        parentNumTag.transform.Translate(new Vector3(0, 0.01f, 0));
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
    void radVRSetUpdate()
    {
        level1 = GameObject.Find("Settings").GetComponent<radVRSettings>().level1;
        level2 = GameObject.Find("Settings").GetComponent<radVRSettings>().level2;
        level3 = GameObject.Find("Settings").GetComponent<radVRSettings>().level3;
        simulationEx = GameObject.Find("Settings").GetComponent<radVRSettings>().simulationEx;
        DFsimulationEx = GameObject.Find("Settings").GetComponent<radVRSettings>().DFsimulationEx;
        levelStatus = GameObject.Find("Settings").GetComponent<radVRSettings>().levelStatus;
        buildingGOS = GameObject.Find("Settings").GetComponent<radVRSettings>().buildingGO.name;
        ambientBounces = GameObject.Find("Settings").GetComponent<radVRSettings>().aBounce.ToString();
    }
    
}

