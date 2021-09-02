using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorColorChanger : MonoBehaviour
{
    float duration = 1.5f;
    private float t = 0;
    bool isReset = false;
    int index = 0;
    
   [SerializeField] Color[] colorArr;
    [SerializeField] Color myColor;

    void Update()
    {
        ColorChangerr();
        GetComponent<MeshRenderer>().materials[1].SetColor("_EmissionColor", myColor);
    }

    
    void ColorChangerr()
    {
        if (index == colorArr.Length - 1)
            index = 0;

        myColor = Color.Lerp(colorArr[index], colorArr[index+1], t);

        if (t < 1)
        {
            t += Time.deltaTime / duration;
        }
        else 
        {
            
            
            index++;
            t = 0;
        }

        

    }
}
