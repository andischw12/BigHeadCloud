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
    
    public void SetAvatar(int[] arr)
    {
        SelectPrafab(arr[10]);
        SetSignOn();
        Instantiate(RawImageCam, ActiveManager.HatsPrefab.transform.parent);
        ActiveManager.SetAvatarAccessoryItem(AvatarInfoList.Hats, arr[0]);
        ActiveManager.SetAvatarAccessoryItem(AvatarInfoList.Glasses, arr[1]);
        ActiveManager.SetAvatarAccessoryItem(AvatarInfoList.Signates, arr[2]);
        ActiveManager.SetAvatarAccessoryItem(AvatarInfoList.Capes, arr[3]);
        ActiveManager.SetAvatarDressItem(AvatarInfoList.ChestGM, arr[4], arr[5]);
        ActiveManager.SetAvatarDressItem(AvatarInfoList.LegsGm, arr[6], arr[7]);
        ActiveManager.SetAvatarDressItem(AvatarInfoList.FeetGM, arr[8], arr[9]);
    }

     
    public int[] GetAvatarInfo()
    {
        while (!ActiveManager.AmIready()) { }
        int[] toReturn = new int[Enum.GetNames(typeof(AvatarInfoList)).Length];
        toReturn[AvatarInfoList.Hats.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarInfoList.Hats);
        toReturn[AvatarInfoList.Glasses.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarInfoList.Glasses);
        toReturn[AvatarInfoList.Signates.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarInfoList.Signates);
        toReturn[AvatarInfoList.Capes.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarInfoList.Capes);
        toReturn[AvatarInfoList.ChestGM.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarInfoList.ChestGM);
        toReturn[AvatarInfoList.ChestMat.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarInfoList.ChestMat);
        toReturn[AvatarInfoList.LegsGm.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarInfoList.LegsGm);
        toReturn[AvatarInfoList.LegsMat.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarInfoList.LegsMat);
        toReturn[AvatarInfoList.FeetGM.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarInfoList.FeetGM);
        toReturn[AvatarInfoList.FeetMat.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarInfoList.FeetMat);
        toReturn[AvatarInfoList.AvatarPrefab.GetHashCode()] = GetActivePrefabNum();
        return toReturn;
    }

    public void SetAvatarAccessoryItem(AvatarInfoList item, int num)
    {
        ActiveManager.SetAvatarAccessoryItem(item, num);
    }

    public void SetAvatarDressItem(AvatarInfoList item, int num, int mat)
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
        toReturn[AvatarInfoList.Hats.GetHashCode()] = UnityEngine.Random.Range(0, tmp.GetComponent<KidAvatarSelector>().ActiveManager.Hats.Length);
        //toReturn[AvatarInfoList.Glasses.GetHashCode()] = UnityEngine.Random.Range(0, tmp.GetComponent<KidAvatarSelector>().ActiveManager.Glasses.Length);
        toReturn[AvatarInfoList.Signates.GetHashCode()] = UnityEngine.Random.Range(0, tmp.GetComponent<KidAvatarSelector>().ActiveManager.Signates.Length);
        Destroy(tmp.gameObject);
        return toReturn;
    }

    public int[] GetBotAvatar(BotConfiguration bot)
    {
        GameObject tmp = Instantiate(this.gameObject);
        tmp.GetComponent<KidAvatarSelector>().SelectPrafab(bot.BotPrefab);
        int[] toReturn = tmp.GetComponent<KidAvatarSelector>().GetAvatarInfo();
        toReturn[AvatarInfoList.Hats.GetHashCode()] = Math.Min(tmp.GetComponent<KidAvatarSelector>().GetComponentInChildren<KidAvatarManager>().Hats.Length-3,bot.BotPrefab +1);
        toReturn[AvatarInfoList.Glasses.GetHashCode()] = Math.Min(tmp.GetComponent<KidAvatarSelector>().GetComponentInChildren<KidAvatarManager>().Glasses.Length-1, bot.BotPrefab + 1);
        toReturn[AvatarInfoList.Signates.GetHashCode()] = Math.Min(tmp.GetComponent<KidAvatarSelector>().GetComponentInChildren<KidAvatarManager>().Signates.Length-1,bot.BotPrefab + 1);
        Destroy(tmp.gameObject);
        return toReturn;
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
 
