using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{

    [SerializeField] int prefabNum;
    [SerializeField] GameObject AvatarPrefab;
    public GameObject[] AvatarInstance;
   [SerializeField] Transform instance0;

    int Hatcounter = 0;

    int Glassescounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        AvatarPrefab.GetComponent<KidAvatarSelector>().PreperePrefabArr();
        int arr = AvatarPrefab.GetComponent<KidAvatarSelector>().PrefabArr.Length;
        //AvatarPrefab.GetComponent<KidAvatarSelector>().PreperePrefabArr();
    
       
        AvatarInstance = new GameObject[arr];
        AvatarInstance[0] = AvatarPrefab;
     
        AvatarInstance[0].GetComponent<KidAvatarSelector>().SelectAvatarByPrefab(0);

        for (int i =1;i<arr;i++)
        {
            AvatarInstance[i] = Instantiate(AvatarPrefab,instance0.position + i*Vector3.left,Quaternion.identity);
         //   AvatarInstance[i].GetComponent<KidAvatarSelector>().PreperePrefabArr();
          AvatarInstance[i].GetComponent<KidAvatarSelector>().SelectAvatarByPrefab(i);

          //  AvatarInstance[i].GetComponent<KidAvatarSelector>().SetAvatarDressItem(AvatarArrayEnum.Glasses, 0, 0);
           // AvatarInstance[i].get
        }
        //SetHats();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.H)) 
        {
          
            if (Hatcounter == AvatarPrefab.GetComponent<KidAvatarSelector>().AccesoriesArr[0].transform.childCount)
                Hatcounter = 0;
               
            SetHats();
            Hatcounter++;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (Glassescounter == AvatarPrefab.GetComponent<KidAvatarSelector>().AccesoriesArr[2].transform.childCount)
                Glassescounter = 0;
            SetGlasses();
            Glassescounter++;
        }

    }

    void SetHats() 
    {
        foreach (GameObject GM in AvatarInstance)
        {
            GM.GetComponentInChildren<KidAvatarSelector>().SetAvatarAccessoryItem(AvatarArrayEnum.Hats, Hatcounter);

        }
    }


    void SetGlasses()
    {
        foreach (GameObject GM in AvatarInstance)
        {
            GM.GetComponentInChildren<KidAvatarSelector>().SetAvatarAccessoryItem(AvatarArrayEnum.Glasses, Glassescounter);

        }
    }
}
