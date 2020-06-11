using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class InstantSimulationPrev : MonoBehaviour {

    private bool colorChange;
    private float blueVal = 0;
    private float greenVal= 5;
    private float redVal = 10;

    GameObject[] cubeHolder;
    GameObject[] boxHolder;

    public GameObject GridHolderGO;

    // Use this for initialization

    void Start()
    {

        colorChangeInstant();




    }



    public void colorChangeInstant()
    {
        GameObject parentIGrid = GridHolderGO;
        if (parentIGrid != null)
        {
            for (int simCount = 0; simCount < parentIGrid.transform.childCount; simCount++)
            {
                float simVal = float.Parse(parentIGrid.transform.GetChild(simCount).name);
                Color cubeCol;
                float red;
                float green;
                float blue;
                float alpha = 1f;

                if (simVal > redVal)
                {
                    cubeCol = new Color(1, 0, 0, alpha);
                }
                else
                {
                    if (simVal > greenVal)
                    {

                        float domain = Math.Abs(redVal - greenVal);
                        //                   red = Math.Abs(redVal - simVal) / domain;
                        red = 1;
                        green = 1 - (Math.Abs(simVal - greenVal) / domain);
                        blue = 0;
                        cubeCol = new Color(red, green, blue, alpha);
                    }
                    else
                    {
                        if (simVal > blueVal)
                        {
                            float domain = Math.Abs(greenVal - blueVal);
                            //red=0;
                            red = (Math.Abs(simVal) / domain);
                            green = (Math.Abs(simVal) / domain);
                            blue = 1 - (Math.Abs(simVal) / domain);
                            cubeCol = new Color(red, green, blue, alpha);
                        }
                        else
                        {
                            red = 0;
                            green = 0;
                            blue = 1;
                            cubeCol = new Color(red, green, blue, alpha);
                        }
                    }
                }
                Renderer rend = parentIGrid.transform.GetChild(simCount).transform.GetChild(0).GetComponent<Renderer>();
                rend.material.color = cubeCol;
                rend.material.shader = Shader.Find("Transparent/Diffuse");
            }
        }

    }
}

