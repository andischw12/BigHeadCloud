using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FPS_check : MonoBehaviour
{
    public int fps = 0;
    public int framecounter;
    public framespersecondCS framespersecondCScript;
    public int result;
    public GameObject Icon;
    static bool done;
    static bool setActive;
    // Start is called before the first frame update
    void Start()
    {
        HighQualityOff();
        if (done && setActive)
        {
            HighQualityOn();
        }

        else 
        {
            HighQualityOff();

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            framecounter++;
            if (framecounter < 100)
            {
                fps = Mathf.RoundToInt(framespersecondCScript.FPS);
                if (fps > 40)
                {
                    HighQualityOn();
                   // Icon.SetActive(true);
                    setActive = true;
                }
            }
            else
            {
                done = true;
            }
        }
    }


    void HighQualityOn()
    {
        QualitySettings.antiAliasing = 4;
        FindObjectOfType<PostProcessLayer>().enabled = true;
    }

    void HighQualityOff()
    {
        QualitySettings.antiAliasing = 0;
        FindObjectOfType<PostProcessLayer>().enabled = false;

    }
}
     
   
