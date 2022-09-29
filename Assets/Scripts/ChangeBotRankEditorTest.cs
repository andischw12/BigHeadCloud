using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class ChangeBotRankEditorTest : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        BotConfiguration[] arr = GetComponentsInChildren<BotConfiguration>();

        for(int i=0;i<arr.Length;i++)
        {
             arr[i]._botRank =i+1;
        }
    }

     
}
