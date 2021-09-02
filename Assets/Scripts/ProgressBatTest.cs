using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
public class ProgressBatTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(BarProcess());
    }
     

    IEnumerator BarProcess() 
    {
        yield return new WaitUntil(()=> FindObjectOfType<ProgressBar>().currentPercent > 70f);
        FindObjectOfType<ProgressBar>().isOn = false;

    }
}
