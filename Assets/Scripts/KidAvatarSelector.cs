using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AvatarStuff;

public class KidAvatarSelector : MonoBehaviour
{
    [SerializeField] public GameObject[] PrefabArr;
    [SerializeField]   KidAvatarManager ActiveAvatar;
    [SerializeField] int[] currentSelection;
    [SerializeField] Camera RawImageCam;


 
    
    public void PreperePrefabArr() 
    {
        KidAvatarManager[] tmpArr = GetComponentsInChildren<KidAvatarManager>();
        PrefabArr = new GameObject[tmpArr.Length];
        for (int i = 0; i < PrefabArr.Length; i++)
        {
            PrefabArr[i] = tmpArr[i].gameObject;
        }
     }

    public int[] SelectAvatarByPrefab(int num)
    {
        SelectAvatarPrafab(num);
        return GetActiveAvatarInfo();
    }

    void SelectAvatarPrafab(int num)
    {
        for (int i = 0; i < PrefabArr.Length; i++)
        {
            if (i == num)
            {
                PrefabArr[i].SetActive(true);
                ActiveAvatar = PrefabArr[i].GetComponent<KidAvatarManager>();
            }
            else
                PrefabArr[i].SetActive(false);
        }
    }
   


    
    public void SetActiveAvatarLook(int[] arr)
    {
        SelectAvatarPrafab(arr[10]);
        SetSignOn();
        Instantiate(RawImageCam,ActiveAvatar.HatsPrefab.transform.parent);
        ActiveAvatar.SetAvatarAccessoryItem(AvatarArrayEnum.Hats, arr[0]);
        ActiveAvatar.SetAvatarAccessoryItem(AvatarArrayEnum.Glasses, arr[1]);
        ActiveAvatar.SetAvatarAccessoryItem(AvatarArrayEnum.Signates, arr[2]);
        ActiveAvatar.SetAvatarDressItem(AvatarArrayEnum.ChestGM, arr[4], arr[5]);
        ActiveAvatar.SetAvatarDressItem(AvatarArrayEnum.LegsGm, arr[6], arr[7]);
        ActiveAvatar.SetAvatarDressItem(AvatarArrayEnum.FeetGM, arr[8], arr[9]);
    }

     
    public int[] GetActiveAvatarInfo()
    {
        while (!ActiveAvatar.AmIready()) { }
        int[] toReturn = new int[Enum.GetNames(typeof(AvatarArrayEnum)).Length];
        toReturn[AvatarArrayEnum.Hats.GetHashCode()] = ActiveAvatar.GetAvatrItem(AvatarArrayEnum.Hats);
        toReturn[AvatarArrayEnum.Glasses.GetHashCode()] = ActiveAvatar.GetAvatrItem(AvatarArrayEnum.Glasses);
        toReturn[AvatarArrayEnum.Signates.GetHashCode()] = ActiveAvatar.GetAvatrItem(AvatarArrayEnum.Signates);
        //toReturn[AvatarInfoList.Capes.GetHashCode()] = ActiveManager.GetAvatrItem(AvatarInfoList.Capes);
        toReturn[AvatarArrayEnum.ChestGM.GetHashCode()] = ActiveAvatar.GetAvatrItem(AvatarArrayEnum.ChestGM);
        toReturn[AvatarArrayEnum.ChestMat.GetHashCode()] = ActiveAvatar.GetAvatrItem(AvatarArrayEnum.ChestMat);
        toReturn[AvatarArrayEnum.LegsGm.GetHashCode()] = ActiveAvatar.GetAvatrItem(AvatarArrayEnum.LegsGm);
        toReturn[AvatarArrayEnum.LegsMat.GetHashCode()] = ActiveAvatar.GetAvatrItem(AvatarArrayEnum.LegsMat);
        toReturn[AvatarArrayEnum.FeetGM.GetHashCode()] = ActiveAvatar.GetAvatrItem(AvatarArrayEnum.FeetGM);
        toReturn[AvatarArrayEnum.FeetMat.GetHashCode()] = ActiveAvatar.GetAvatrItem(AvatarArrayEnum.FeetMat);
        toReturn[AvatarArrayEnum.AvatarPrefab.GetHashCode()] = GetActivePrefabNum();
        return toReturn;
    }

    public void SetAvatarAccessoryItem(AvatarArrayEnum item, int num)
    {
        ActiveAvatar.SetAvatarAccessoryItem(item, num);
    }

    public void SetAvatarDressItem(AvatarArrayEnum item, int num, int mat)
    {
        ActiveAvatar.SetAvatarDressItem(item,num,mat);
    }

    public GameObject GetSignateGM() 
    {
        return ActiveAvatar.SignatePrefab;
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
        tmp.GetComponent<KidAvatarSelector>().SelectAvatarPrafab(UnityEngine.Random.Range(0, 12));
        int[] toReturn = tmp.GetComponent<KidAvatarSelector>().GetActiveAvatarInfo();
        toReturn[AvatarArrayEnum.Hats.GetHashCode()] = UnityEngine.Random.Range(0, tmp.GetComponent<KidAvatarSelector>().ActiveAvatar.Hats.Length);
        //toReturn[AvatarInfoList.Glasses.GetHashCode()] = UnityEngine.Random.Range(0, tmp.GetComponent<KidAvatarSelector>().ActiveManager.Glasses.Length);
        toReturn[AvatarArrayEnum.Signates.GetHashCode()] = UnityEngine.Random.Range(0, tmp.GetComponent<KidAvatarSelector>().ActiveAvatar.Signates.Length);
        Destroy(tmp.gameObject);
        return toReturn;
    }

    public int[] GetBotAvatar(BotConfiguration bot)
    {
        GameObject tmp = Instantiate(this.gameObject);
        tmp.GetComponent<KidAvatarSelector>().SelectAvatarPrafab(bot.BotPrefab);
        int[] toReturn = tmp.GetComponent<KidAvatarSelector>().GetActiveAvatarInfo();
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
 
