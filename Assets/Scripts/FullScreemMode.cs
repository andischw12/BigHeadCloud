using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FullScreemMode : MonoBehaviour
{
    [SerializeField]
    int Screenwith;
    [SerializeField]
    int Screenheight;
    private void Start()
    {
        int Screenwith = Screen.resolutions[Screen.resolutions.Length - 1].width;
        int Screenheight = Screen.resolutions[Screen.resolutions.Length - 1].height;
        //Debug.Log(Screenheight + Screenwith);
    }
    public void FullScreenButton()
    {
        //trying to fix resoulution
       // Debug.Log("i am shop");
        if (!Screen.fullScreen)
        {
            Screen.SetResolution(Screenwith, Screenheight, true);
        }
        else
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
        }
    }
}
