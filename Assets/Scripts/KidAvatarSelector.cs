using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;


public class KidAvatarSelector : MonoBehaviour
{
    [SerializeField] public AvatarManager[] AvatarsArray; // All Avatars
    [SerializeField] AvatarManager ActiveAvatarManager;// Selected Avatar
    [SerializeField] Camera RawImageCam;
    [SerializeField] public  GameObject[] AccesoriesArr = new GameObject[4]; // 0-BoysHats 1-GirlHats 2-Glasses 4-Signates

    private void Awake()
    {
        AvatarManager[] tmpArr = GetComponentsInChildren<AvatarManager>();
        AvatarsArray = new AvatarManager[tmpArr.Length];
        for (int i = 0; i < AvatarsArray.Length; i++)
        {
            AvatarsArray[i] = tmpArr[i];
        }
        SetAccesories();
    }


    

    public void SetAccesories()
    {
        for (int i = 0; i < AvatarsArray.Length; i++)
        {
            AvatarsArray[i].Getready();
            AvatarsArray[i].JacketOff();
            if (AvatarsArray[i].GetType() == typeof(AvatarManagerBoys))
                AvatarsArray[i].SetAcceories(AccesoriesArr[0], AccesoriesArr[2], AccesoriesArr[3]);
            else 
                AvatarsArray[i].SetAcceories(AccesoriesArr[1], AccesoriesArr[2], AccesoriesArr[3]);
        }
    }

    public int[] SelectAvatarByPrefab(int num)
    {
        SelectAvatarPrafab(num);
        return GetActiveAvatarLook();
    }

    private void SelectAvatarPrafab(int num)
    {
        for (int i = 0; i < AvatarsArray.Length; i++)
        {
            if (i == num)
            {
                AvatarsArray[i].gameObject.SetActive(true);
                ActiveAvatarManager = AvatarsArray[i].GetComponent<AvatarManager>();
            }
            else
                AvatarsArray[i].gameObject.SetActive(false);
        }
    }
   
    public void SetActiveAvatarLook(int[] arr)
    {
        SelectAvatarPrafab(arr[10]);
        SetSignOn();
        Instantiate(RawImageCam,ActiveAvatarManager.HatsPrefab.transform.parent);
        print(arr[0]);
        ActiveAvatarManager.SetAvatarAccessoryItem(AvatarArrayEnum.Hats, arr[0]);
        ActiveAvatarManager.SetAvatarAccessoryItem(AvatarArrayEnum.Glasses, arr[1]);
        ActiveAvatarManager.SetAvatarAccessoryItem(AvatarArrayEnum.Signates, arr[2]);
        ActiveAvatarManager.SetAvatarDressItem(AvatarArrayEnum.ChestGM, arr[4], arr[5]);
        ActiveAvatarManager.SetAvatarDressItem(AvatarArrayEnum.LegsGm, arr[6], arr[7]);
        ActiveAvatarManager.SetAvatarDressItem(AvatarArrayEnum.FeetGM, arr[8], arr[9]);
    }

     
    public int[] GetActiveAvatarLook()
    {
        int[] toReturn = new int[System.Enum.GetNames(typeof(AvatarArrayEnum)).Length];
        toReturn[AvatarArrayEnum.Hats.GetHashCode()] = ActiveAvatarManager.GetAvatrItem(AvatarArrayEnum.Hats);
        toReturn[AvatarArrayEnum.Glasses.GetHashCode()] = ActiveAvatarManager.GetAvatrItem(AvatarArrayEnum.Glasses);
        toReturn[AvatarArrayEnum.Signates.GetHashCode()] = ActiveAvatarManager.GetAvatrItem(AvatarArrayEnum.Signates);
        toReturn[AvatarArrayEnum.ChestGM.GetHashCode()] = ActiveAvatarManager.GetAvatrItem(AvatarArrayEnum.ChestGM);
        toReturn[AvatarArrayEnum.ChestMat.GetHashCode()] = ActiveAvatarManager.GetAvatrItem(AvatarArrayEnum.ChestMat);
        toReturn[AvatarArrayEnum.LegsGm.GetHashCode()] = ActiveAvatarManager.GetAvatrItem(AvatarArrayEnum.LegsGm);
        toReturn[AvatarArrayEnum.LegsMat.GetHashCode()] = ActiveAvatarManager.GetAvatrItem(AvatarArrayEnum.LegsMat);
        toReturn[AvatarArrayEnum.FeetGM.GetHashCode()] = ActiveAvatarManager.GetAvatrItem(AvatarArrayEnum.FeetGM);
        toReturn[AvatarArrayEnum.FeetMat.GetHashCode()] = ActiveAvatarManager.GetAvatrItem(AvatarArrayEnum.FeetMat);
        toReturn[AvatarArrayEnum.AvatarPrefab.GetHashCode()] = GetActivePrefabNum();
        return toReturn;
    }


    public void SetAvatarDressItem(AvatarArrayEnum item, int num)
    {
        ActiveAvatarManager.SetAvatarAccessoryItem(item, num);
    }

    public void SetAvatarDressItem(AvatarArrayEnum item, int num, int mat)
    {
        ActiveAvatarManager.SetAvatarDressItem(item,num,mat);
    }

    public GameObject GetSignateGM() 
    {
        return ActiveAvatarManager.SignatePrefab;
    }
    public int GetActivePrefabNum()
    {
        for (int i = 0; i < AvatarsArray.Length; i++)
        {
            if (AvatarsArray[i].gameObject.activeInHierarchy)
                return i;
        }
        return -1;
    }

    public int[] GetRandomAvatar() 
    {
        GameObject tmp = Instantiate(this.gameObject);    
        tmp.GetComponent<KidAvatarSelector>().SelectAvatarPrafab(UnityEngine.Random.Range(0, 12));
        int[] toReturn = tmp.GetComponent<KidAvatarSelector>().GetActiveAvatarLook();
        toReturn[AvatarArrayEnum.Hats.GetHashCode()] = UnityEngine.Random.Range(0, tmp.GetComponent<KidAvatarSelector>().ActiveAvatarManager.Hats.Length);
        toReturn[AvatarArrayEnum.Glasses.GetHashCode()] = UnityEngine.Random.Range(0, tmp.GetComponent<KidAvatarSelector>().ActiveAvatarManager.Glasses.Length);
        toReturn[AvatarArrayEnum.Signates.GetHashCode()] = UnityEngine.Random.Range(0, tmp.GetComponent<KidAvatarSelector>().ActiveAvatarManager.Signates.Length);
        Destroy(tmp.gameObject);
        return toReturn;
    }

    public int[] GetRandomBotLook(BotConfiguration bot)
    {
        GameObject tmp = Instantiate(this.gameObject);
        tmp.GetComponent<KidAvatarSelector>().SelectAvatarPrafab(bot.BotPrefab);
        int[] toReturn = tmp.GetComponent<KidAvatarSelector>().GetActiveAvatarLook();
        toReturn[AvatarArrayEnum.Hats.GetHashCode()] = UnityEngine.Random.Range(0, 1);
        toReturn[AvatarArrayEnum.Glasses.GetHashCode()] = UnityEngine.Random.Range(0, 1);
        toReturn[AvatarArrayEnum.Signates.GetHashCode()] = UnityEngine.Random.Range(0, 1);
        Destroy(tmp.gameObject);
        return toReturn;
    }

    public void SetSignOn()
    {

        GetSignateGM().SetActive(true);
    }
    public void SetSignOff(float time) 
    {
        StartCoroutine(SingOffHelper(time));  
    }

    IEnumerator SingOffHelper(float time) 
    {
        yield return new WaitForSecondsRealtime(0.4f);
        GetSignateGM().SetActive(false);
    }

}
 
