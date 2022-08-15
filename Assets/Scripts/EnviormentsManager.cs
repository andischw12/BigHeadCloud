using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnviormentList {Random = -1,Bereshit,ShmotVaikra,BamidbarDvarim,Tanak,Avot,Israel,Mada,Tarbut,RoshHashana,Kippur,Sukkot,Shabat,Hanuka,Purim,tfila,mizvot,shirim,numbers,israelHistory,PlacesInIsrael,geography,Biology,food}

public class EnviormentsManager : MonoBehaviour
{
    public static EnviormentsManager instance;
    //[SerializeField] GameObject[] enviorments;
    int _chosen = 1;
    //public int EnviormentsCount {get{return enviorments.Length;}} 
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    public void ChooseEnviorment(int chosen) 
    {
        //return; // andy for test
        if (GameManager.instance.IsNewRandomMode()&& GameProcess.instance.currentQuestionNumber>0)
            Cameras.instance.SelectCamera(Cameras.instance.CameraArray.Length - 1);

        _chosen = chosen;
        /*
        for (int i = 0; i < enviorments.Length; i++)
        {
            if (enviorments[i].activeInHierarchy)
            {
                enviorments[i].SetActive(false);
                break;
            }
        }
        enviorments[chosen].SetActive(true); // andy cancel env false instead of true
        */
        
    }

    public int GetActiveEnv() 
    {
        //return -1;
        return _chosen;
        /*
        for(int i=0; i<enviorments.Length;i++)
        {
            if (enviorments[i].activeInHierarchy)
                return i;
        }
        return -1;
        */
    }
}
