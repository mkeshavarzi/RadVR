using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class gridSettings : MonoBehaviour {
    public float gridSize;
    public GameObject surface;
    public GameObject surfaceParent;
    public float divsionSize;
    Vector3 surfaceSize;
    Vector3 surfaceOrigin;
    string[] nodes;

    public bool spatialGird = false;
    public float spaceHeight = 6;
    public float heightDivision;

    private VRTK_ControllerEvents contEvRight;
    private VRTK_ControllerEvents contEvLeft;

    private Boolean vrOn;


    // Use this for initialization
    void Awake () {
        //        WriteNodesAwake(@"d:\RadVR\nodes.pts");
        WriteNodesUpdate(@"d:\RadVR\nodes.pts");
        contEvRight = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().contRight.GetComponent<VRTK_ControllerEvents>();
        contEvLeft = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().contLeft.GetComponent<VRTK_ControllerEvents>();
        vrOn = GameObject.Find("SunPath").GetComponent<SunPathRadVR>().vrOn;

        

    }

    void Start()
    {
        WriteNodesUpdate(@"d:\RadVR\nodes.pts");
    }

        // Update is called once per frame
        void Update () {

        radVRSetUpdate();

            if (Input.GetKeyUp("n"))
            {
                WriteNodesUpdate(@"d:\RadVR\nodes.pts");

            }
        

	}
    void WriteNodesAwake(string filePath)
    {
        surface.GetComponent<BoxCollider>().enabled = true;
        surfaceSize = surface.GetComponent<BoxCollider>().bounds.size;
        Debug.Log(surfaceSize);
        surfaceOrigin = surface.transform.position;

        nodes = GenNodes(surfaceSize, surfaceOrigin, gridSize);
        System.IO.File.WriteAllLines(filePath, nodes);
        surface.GetComponent<BoxCollider>().enabled = true;

    }

    public void WriteNodesUpdate(string filePath) // Due to the YX Mapping
    {
        surface.GetComponent<BoxCollider>().enabled = true;
        surfaceSize = surface.GetComponent<BoxCollider>().bounds.size;
        surfaceOrigin = surface.GetComponent<Transform>().position;

        

        surfaceSize = new Vector3(surfaceSize.x, surfaceSize.z, surfaceSize.y);//This part too
        surfaceOrigin = new Vector3(surfaceOrigin.x, surfaceOrigin.z, surfaceOrigin.y);// This part


       // Debug.Log(surfaceSize);
       // Debug.Log(surfaceOrigin);


        nodes = GenNodes(surfaceSize, surfaceOrigin, gridSize);
        System.IO.File.WriteAllLines(filePath, nodes);
        surface.GetComponent<BoxCollider>().enabled = true;


    }

    string[] GenNodes(Vector3 dim, Vector3 org, float gSize)
    {
        //Debug.Log("HEY I'M Working");
        int nodeSize = 0;
        int xRange = (int)Mathf.Round(dim.x / divsionSize);
        int yRange = (int)Mathf.Round(dim.y/ divsionSize);

        for (float i = (-xRange); i < xRange; i = i + gridSize)
        {
            for (float j = (-yRange); j < yRange; j = j + gridSize)
            {
                for (float z = org.z; z < org.z + 1+ spaceHeight; z++)
                {
                    nodeSize++;
                }
            }
        }

        string[] nodesLoop = new string[nodeSize];
        int nodeCount = 0;
        for (float i = (-xRange); i< xRange; i= i+ gridSize)
        {
            for (float j = (-yRange); j< yRange; j = j + gridSize)
            {
                for (float z = org.z; z<org.z+ 1+  spaceHeight; z ++ )
                {
                    nodesLoop[nodeCount] = (-(i+org.x)).ToString() + " " + (-(j+org.y)).ToString() + " " + z.ToString() + " 0 0 1";
                    nodeCount++;
                }

            }
        }

        //Debug.Log("NODESIZE"+nodeSize);

        return nodesLoop;
        
    }

    void ChangeSurfHeight()
    {
        if (contEvLeft.buttonTwoPressed)
        {
            surfaceParent.transform.Translate(new Vector3(0, 0, 0.05f));
        }
        if (contEvLeft.buttonOnePressed)
        {
            surfaceParent.transform.Translate(new Vector3(0, 0, -0.05f));
        }
    }



    void radVRSetUpdate()
    {
        gridSize = GameObject.Find("Settings").GetComponent<radVRSettings>().gridSize;


    }
}
