using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudioButtonManager : MonoBehaviour
{
    
    [SerializeField] GameObject ButtonPrefab;
    [SerializeField] StudioMenuButton[] StudioArr;
    [SerializeField] string[] StudioNames;
    [SerializeField] public static int[] NeededRanks = { 1, 15, 30 };
    [SerializeField] public static float[] BonusMulti = {100f,150f,200f};
    [SerializeField] Sprite[] StudioSprites;

    void Start()
    {
        CreateButtons();
    }


    void CreateButtons() 
    {
        StudioArr = new StudioMenuButton[StudioNames.Length];
        for(int i = 0; i < StudioNames.Length; i++) 
        {
            StudioArr[i] = Instantiate(ButtonPrefab,this.transform).GetComponent<StudioMenuButton>();
            StudioArr[i].SetStudioButton(StudioSprites[i],StudioNames[i],NeededRanks[i],BonusMulti[i]);
            StudioArr[i].EnviormentNum = i;
            if (ProfileManager.GetRankFromServer() < NeededRanks[i]) 
            {
                StudioArr[i].MakeButtonNotAvaliable();
            }
            else 
            {
                StudioArr[i].MakeButtonAvaliable();
                StudioArr[i].AddListner();
            }
                
        }
    }

     
}
