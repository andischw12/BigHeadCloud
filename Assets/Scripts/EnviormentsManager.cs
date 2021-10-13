using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnviormentList {Random = -1,Bereshit,ShmotVaikra,BamidbarDvarim,Tanak,Avot,Israel,Mada,Tarbut,RoshHashana,Kippur,Sukkot,Shabat}

public class EnviormentsManager : MonoBehaviour
{
    public static EnviormentsManager instance;
    [SerializeField] GameObject[] enviorments;
    public int EnviormentsCount {get{return enviorments.Length;}} 
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    public void ChooseEnviorment(int chosen) 
    {
        if (GameManager.instance.IsNewRandomMode()&& GameProcess.instance.currentQuestionNumber>0)
            Cameras.instance.SelectCamera(Cameras.instance.CameraArray.Length - 1);
        for (int i = 0; i < enviorments.Length; i++)
        {
            if (enviorments[i].activeInHierarchy)
            {
                enviorments[i].SetActive(false);
                break;
            }
        }
        enviorments[chosen].SetActive(true);

        
    }

    public int GetActiveEnv() 
    { 
        for(int i=0; i<enviorments.Length;i++)
        {
            if (enviorments[i].activeInHierarchy)
                return i;
        }
        return -1;
    }
}
