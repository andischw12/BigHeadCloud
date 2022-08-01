using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class PicChanger : MonoBehaviour
{
 

    [SerializeField] Sprite[] SpriteArr;
    [SerializeField] string[] NameArr;

    // Start is called before the first frame update
    void Start()
    {
        InventoryItem[] SourceArr = GetComponentsInChildren<InventoryItem>();

        SpriteArr = new Sprite[SourceArr.Length];
        NameArr = new string[SourceArr.Length];

        for(int i =0;i< SourceArr.Length; i++) 
        {
            SpriteArr[i] = SourceArr[i].ImageRenderer.sprite;
            NameArr[i] = SourceArr[i].Title;
        }





    }

}
