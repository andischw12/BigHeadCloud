using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginMenuAvatarSelectManager : MonoBehaviour
{

   public int startPrefab;
   public  int endPrafab;
   public  int current;
   KidAvatarSelector MenuAvatar;


    private void Start()
    {// will be replaced:
        endPrafab = FindObjectOfType<KidAvatarSelector>().PrefabArr.Length;
        startPrefab = 0;
        MenuAvatar = LoginScreenManager.instance.AvatarPlayerChoosingInstance.GetComponentInChildren<KidAvatarSelector>();
    }

    public void SelectFirstCharacther() 
    {
        MenuAvatar.SelectAvatarByPrefab(current = startPrefab);
    }

    public void Next() 
    {
         
        int tmp = current + 1;
        if (tmp == endPrafab)
            tmp = startPrefab;
        MenuAvatar.SelectAvatarByPrefab(current=tmp);
    }

    public void Pre()
    {
         
        int tmp = current - 1;
        if (tmp == startPrefab - 1)
        {
            tmp = endPrafab;
        }
        MenuAvatar.SelectAvatarByPrefab(current = tmp);
    }


    public void ChooseBoys()
    {
        startPrefab = 0;
        endPrafab = 16;
        current = startPrefab;
        Next();
        Pre();
    }

    public void ChooseGirls()
    {
        current = startPrefab;
        startPrefab = 16;
        endPrafab = 32;
        current = startPrefab;

        Next();
        Pre();
    }
}
