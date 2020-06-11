using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class sunPathEffects : MonoBehaviour {

    //VR Trigger
    private bool sunChangingSPE;
    private bool matIsSame  = false;

    //RadWorld GameObjects
    private GameObject RadWorldGameObject;
    private int childCount;
    GameObject[] childVector;
    GameObject[] childVectorIns;


    //Material
    public Material transMat;
    Material[] curMat;
    public Material skyBoxMat;
    private Material defSkyMat;

    private Material levSkyMat;

    //SkySim
    public bool updateSky = true;

    //Sunpath
    private string latitude;
    private string longitude;
    private string meridian;
    private string month;
    private string day;
    private string hour;
    private string minute;
    private bool level3;
    private bool transBui;

    private GameObject RadWorldGameObjectIns;





    // Use this for initialization
    void Awake () {
        radVRSetAwake();
        childCount = RadWorldGameObject.transform.childCount;
        defSkyMat = RenderSettings.skybox;
        RenderSettings.skybox = skyBoxMat;

        childVector = new GameObject[childCount];
        childVectorIns = new GameObject[childCount];
        curMat = new Material[childCount];


        for (int childI = 0; childI<childCount; childI++)
        {
            childVector[childI] = RadWorldGameObject.transform.GetChild(childI).gameObject;
            curMat[childI] = childVector[childI].GetComponent<MeshRenderer>().material;

        }


    }


    // Update is called once per frame
    void Update()
    {
        radVRSetUpdate();
        if (level3 == !true) { RenderSettings.skybox = defSkyMat; }
        if (transBui == true)
           

        {
            MaterialChanger();
        }
       
    }






    void MaterialChanger()
    {
        
        if (sunChangingSPE)
        {
            
            RenderSettings.skybox = defSkyMat;
            for (int childJ = 0; childJ < childCount; childJ++)
            {

                
               childVector[childJ].GetComponent<MeshRenderer>().material = transMat;
                matIsSame = false;
//                childVector[childJ].GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }
        else
        {   
            
            if (matIsSame == false)// Gameobject own material and current material
            {
                if (childVector[0].GetComponent<MeshRenderer>().material == curMat[0])
                {
                    matIsSame = true;
                }
                else
                {
                    
                    for (int childJ = 0; childJ < childCount; childJ++)
                    {
                        childVector[childJ].GetComponent<MeshRenderer>().material = curMat[childJ];
 //                       childVector[childJ].GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;


                    }
                    if (level3 == true)
                    {
                        RenderSettings.skybox = skyBoxMat;
                        if (updateSky == true)
                        {
                            gameObject.GetComponent<importSky>().UpdateSkyRad();
                        }
                    }
                    else
                    {
                        RenderSettings.skybox = defSkyMat;
                    }

                    
                }
            }
        }
    }

    void radVRSetUpdate()
    {
        sunChangingSPE = GameObject.Find("Settings").GetComponent<radVRSettings>().sunChanging;
        month = GameObject.Find("Settings").GetComponent<radVRSettings>().month;
        day = GameObject.Find("Settings").GetComponent<radVRSettings>().day;
        hour = GameObject.Find("Settings").GetComponent<radVRSettings>().hour;
        minute = GameObject.Find("Settings").GetComponent<radVRSettings>().minute;
        level3 = GameObject.Find("Settings").GetComponent<radVRSettings>().level3;
        transBui = GameObject.Find("Settings").GetComponent<radVRSettings>().transBui;


    }
    void radVRSetAwake()
    {
        RadWorldGameObject = GameObject.Find("Settings").GetComponent<radVRSettings>().buildingGO;
        latitude = GameObject.Find("Settings").GetComponent<radVRSettings>().latitude;
        longitude = GameObject.Find("Settings").GetComponent<radVRSettings>().longitude;
    }

}
