using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UlpanScreenManager : MonoBehaviour
{

    //public static UlpanScreenManager instance;
    [SerializeField] Material defult;
    [SerializeField] TextMeshPro Tvtxt;
    [SerializeField] Material EmptyBackground;
    [SerializeField] int ScreenMaterialInMeshArr;

    // Start is called before the first frame update
    /*
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        
    }
    */
    void Start()
    {
        SetDefultPic();


    }

    public void SetDefultPic() 
    {
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().materials[ScreenMaterialInMeshArr] = defult;
    }

    public void SetVsPic()
    {
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().materials[ScreenMaterialInMeshArr] = EmptyBackground;
    }

    public void SetText(string s) 
    {
        Tvtxt.text =  s;
    }

}
