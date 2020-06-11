using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialImp : MonoBehaviour {

    GameObject buildingGO;
    int childCount;

    // Use this for initialization
    void Start () {
        buildingGO = GameObject.Find("Settings").GetComponent<radVRSettings>().buildingGO;
        ApplyMaterial();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ApplyMaterial()
    {
        if (buildingGO != null)
        {
            childCount = buildingGO.transform.childCount;
            for (int bChild = 0; bChild<childCount; bChild++)
            {
                GameObject OChild = buildingGO.transform.GetChild(bChild).gameObject;
                string GOName = OChild.name;
                if (Resources.Load("Material/" + GOName) != null)
                {
                    Renderer GoChildRend;
                    GoChildRend = OChild.GetComponent<Renderer>();
                    Material GOMatO = Resources.Load("Material/" + GOName) as Material;
                    GoChildRend.material = GOMatO;
                }
            }
        }
    }
}
