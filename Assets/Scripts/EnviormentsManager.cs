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
        for(int i =0;i<enviorments.Length; i++)
        {
            if (i == chosen)
                enviorments[i].SetActive(true);
            else
                enviorments[i].SetActive(false);
        }
    }
}
