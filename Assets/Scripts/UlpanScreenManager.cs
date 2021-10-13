using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UlpanScreenManager : MonoBehaviour
{

    public static UlpanScreenManager instance;
    [SerializeField] Sprite defult;
    [SerializeField] TextMeshPro Tvtxt;
    [SerializeField] Sprite VsPic;

    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if(GetComponent<SpriteRenderer>()!=null)
        GetComponent<SpriteRenderer>().sprite  = defult;
    }

    public void SetDefultPic() 
    {
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().sprite = defult;
    }

    public void SetVsPic()
    {
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().sprite = VsPic;
    }

    public void SetText(string s) 
    {
        Tvtxt.text = s;
    }

}
