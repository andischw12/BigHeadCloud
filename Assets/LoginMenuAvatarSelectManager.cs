using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginMenuAvatarSelectManager : MonoBehaviour
{

   public int startPrefab;
   public  int endPrafab;
   public  int current;


    private void Start()
    {// will be replaced:
        endPrafab = FindObjectOfType<KidAvatarSelector>().PrefabArr.Length;
        startPrefab = 0;
    }

    public void SelectFirstCharacther() 
    {
        FindObjectOfType<KidAvatarSelector>().SelectAvatarByPrefab(current = startPrefab);
    }

    public void Next() 
    {
        KidAvatarSelector kid =FindObjectOfType<KidAvatarSelector>();

        int tmp = kid.GetActivePrefabNum() + 1;
        if (tmp == kid.PrefabArr.Length)
            tmp = 0;
        kid.SelectAvatarByPrefab(current=tmp);
    }

    public void Pre()
    {
        KidAvatarSelector kid = FindObjectOfType<KidAvatarSelector>();
        int tmp = kid.GetActivePrefabNum() - 1;
        if (tmp == -1)
        {
            tmp = kid.PrefabArr.Length - 1;
        }
        kid.SelectAvatarByPrefab(current = tmp);
    }
}
