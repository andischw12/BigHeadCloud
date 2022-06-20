using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AvatarStuff;

public class KidAvatarSelector : MonoBehaviour
{
    [SerializeField] GameObject[] PrefabArr;
    [SerializeField]  KidAvatarManager ActiveManager;
    [SerializeField] int[] currentSelection;
    [SerializeField] Camera RawImageCam;
    


    void Start() 
    {
        KidAvatarManager[] tmpArr = GetComponentsInChildren<KidAvatarManager>();
        PrefabArr = new GameObject[PrefabArr.Length];
        for(int i= 0; i < PrefabArr.Length; i++) 
        {
            PrefabArr[i] = tmpArr[i].gameObject;
        }
    }

    public void SetAvatar(int[] arr)
    {
         
        SelectPrafab(arr[10]);
        SetSignOn();
        Instantiate(RawImageCam,ActiveManager.HatsPrefab.transform.parent);
        ActiveManager.SetAvatarAccessoryItem(AvatarArrayEnum.Hats, arr[0]);
        ActiveManager.SetAvatarAccessoryItem(AvatarArrayEnum.Glasses, arr[1]);
        ActiveManager.SetAvatarAccessoryItem(AvatarArrayEnum.Signates, arr[2]);
        ActiveManager.SetAvatarDressItem(AvatarArrayEnum.ChestGM, arr[4], arr[5]);
        ActiveManager.SetAvatarDressItem(AvatarArrayEnum.LegsGm, arr[6], arr[7]);
        ActiveManager.SetAvatarDressItem(AvatarArrayEnum.FeetGM, arr[8], arr[9]);
    }

     
    public int[] GetAvatarInfo()
    {
        while (!ActiveManager.AmIready()) { }
        int[] toReturn = new int[Enum.GetNames(typeof(AvatarArrayEnum)).Length];
        toReturn[AvatarArrayEnum.Hats.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarArrayEnum.Hats);
        toReturn[AvatarArrayEnum.Glasses.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarArrayEnum.Glasses);
        toReturn[AvatarArrayEnum.Signates.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarArrayEnum.Signates);
        //toReturn[AvatarInfoList.Capes.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarInfoList.Capes);
        toReturn[AvatarArrayEnum.ChestGM.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarArrayEnum.ChestGM);
        toReturn[AvatarArrayEnum.ChestMat.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarArrayEnum.ChestMat);
        toReturn[AvatarArrayEnum.LegsGm.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarArrayEnum.LegsGm);
        toReturn[AvatarArrayEnum.LegsMat.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarArrayEnum.LegsMat);
        toReturn[AvatarArrayEnum.FeetGM.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarArrayEnum.FeetGM);
        toReturn[AvatarArrayEnum.FeetMat.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarArrayEnum.FeetMat);
        toReturn[AvatarArrayEnum.AvatarPrefab.GetHashCode()] = GetActivePrefabNum();
        return toReturn;
    }

    public void SetAvatarAccessoryItem(AvatarArrayEnum item, int num)
    {
        ActiveManager.SetAvatarAccessoryItem(item, num);
    }

    public void SetAvatarDressItem(AvatarArrayEnum item, int num, int mat)
    {
        ActiveManager.SetAvatarDressItem(item,num,mat);
    }

    public GameObject GetSignateGM() 
    {
        return ActiveManager.SignatePrefab;
    }

    public void SelectPrafab(int num)
    {
        for (int i = 0; i < PrefabArr.Length; i++)
        {
            if (i == num)
            {
                PrefabArr[i].SetActive(true);
                ActiveManager = PrefabArr[i].GetComponent<KidAvatarManager>();
            }

            else
                PrefabArr[i].SetActive(false);
        }
    }

    public int GetActivePrefabNum()
    {
        for (int i = 0; i < PrefabArr.Length; i++)
        {
            if (PrefabArr[i].gameObject.activeInHierarchy)
                return i;
        }
        return -1;
    }


    public int[] GetRandomAvatar() 
    {
        GameObject tmp = Instantiate(this.gameObject);    
        tmp.GetComponent<KidAvatarSelector>().SelectPrafab(UnityEngine.Random.Range(0, 12));
        int[] toReturn = tmp.GetComponent<KidAvatarSelector>().GetAvatarInfo();
        toReturn[AvatarArrayEnum.Hats.GetHashCode()] = UnityEngine.Random.Range(0, tmp.GetComponent<KidAvatarSelector>().ActiveManager.Hats.Length);
        //toReturn[AvatarInfoList.Glasses.GetHashCode()] = UnityEngine.Random.Range(0, tmp.GetComponent<KidAvatarSelector>().ActiveManager.Glasses.Length);
        toReturn[AvatarArrayEnum.Signates.GetHashCode()] = UnityEngine.Random.Range(0, tmp.GetComponent<KidAvatarSelector>().ActiveManager.Signates.Length);
        Destroy(tmp.gameObject);
        return toReturn;
    }

    public int[] GetBotAvatar(BotConfiguration bot)
    {
        GameObject tmp = Instantiate(this.gameObject);
        tmp.GetComponent<KidAvatarSelector>().SelectPrafab(bot.BotPrefab);
        int[] toReturn = tmp.GetComponent<KidAvatarSelector>().GetAvatarInfo();
        toReturn[AvatarArrayEnum.Hats.GetHashCode()] = Math.Min(tmp.GetComponent<KidAvatarSelector>().GetComponentInChildren<KidAvatarManager>().Hats.Length-3,bot.BotPrefab +1);
        toReturn[AvatarArrayEnum.Glasses.GetHashCode()] = Math.Min(tmp.GetComponent<KidAvatarSelector>().GetComponentInChildren<KidAvatarManager>().Glasses.Length-1, bot.BotPrefab + 1);
        toReturn[AvatarArrayEnum.Signates.GetHashCode()] = Math.Min(tmp.GetComponent<KidAvatarSelector>().GetComponentInChildren<KidAvatarManager>().Signates.Length-1,CalculateSignetForBot(bot.BotPoints()));
        Destroy(tmp.gameObject);
        return toReturn;
    }

    private int CalculateSignetForBot(int botpoints) 
    {

        int temp = botpoints;
        while (temp >= 10)
        {
            temp /= 10;
        }
        return temp;
    }
    public int[] SelectAvatarByPrefab(int num)
    {
        SelectPrafab(num);
        return GetAvatarInfo();
    }

    public void SetSignOff(float time) 
    {
        StartCoroutine(SingOffHelper(time));  
    }

    public void SetSignOn()
    {
         
        GetSignateGM().SetActive(true);
    }

    IEnumerator SingOffHelper(float time) 
    {
        yield return new WaitForSecondsRealtime(0.4f);
        GetSignateGM().SetActive(false);

         
    }

}
 
