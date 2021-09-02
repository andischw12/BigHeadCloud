using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UlpanScreenManager : MonoBehaviour
{
    public static UlpanScreenManager instance;
    [SerializeField] Sprite defult;
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

    public void SetPic(Sprite s)
    {
        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().sprite = s;
    }

}
