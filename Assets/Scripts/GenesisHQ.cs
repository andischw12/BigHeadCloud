using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AG_WebGLFPSAccelerator;

public class GenesisHQ : MonoBehaviour
{
    public GameObject HQ;

    private void Awake()
    {
        HQ.SetActive(false);
    }
    void Start()
    {
        
        try
        {
            if (FindObjectOfType<WebGLFPSAccelerator>().dpi == 1)
            {
                HQ.SetActive(true);
            }
        }
        catch { }
      
    }
     
}
