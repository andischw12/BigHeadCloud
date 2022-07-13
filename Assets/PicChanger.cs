using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicChanger : MonoBehaviour
{

    [SerializeField] Sprite[] IconsSource;
    [SerializeField] Image[] TargetArr;

    // Start is called before the first frame update
    void Start()
    {
        Image[] tmpArr = GetComponentsInChildren<Image>();


        TargetArr = new Image[IconsSource.Length];
        for(int i = 0; i < tmpArr.Length; i++) 
        {
            if(tmpArr[i].gameObject.transform.name == "Icon") 
            {
                print("icon found");
            }
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
