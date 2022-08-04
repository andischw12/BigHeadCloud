using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using  Michsky.UI.ModernUIPack;
using Michsky.UI.Zone;
 
//[ExecuteInEditMode]
public class FontChanger : MonoBehaviour
{

     [SerializeField] TMP_FontAsset Source;
    [SerializeField] TextMeshProUGUI[] AllTextArr;
    [SerializeField] Button[] AllButtontArr;

    [SerializeField] GameObject ButtonPrefab;
    [SerializeField] ColorBlock Higlighted;
 

    void Start()
    {

       // ButtonChanger();
    }  

    void ButtonChanger() 
    {
        AllButtontArr = FindObjectsOfType<Button>(true);

        foreach (Button btn in AllButtontArr)
        {

             btn.colors = Higlighted;
            /*
            btn.GetComponent<Image>().enabled = false;
            btn.GetComponent<Image>().enabled = true;


            foreach (var comp in btn.gameObject.GetComponents<Component>())
            {
                if (comp is UIElementSound)
                {
                    DestroyImmediate(comp);
                }

                if (comp is AudioSource)
                {
                    DestroyImmediate(comp);
                }
            }
            */
            //btn.gameObject.AddComponent<UIElementSound>();


            /*
            if (btn.GetComponent<UIElementSound>() != null) 
            {
                btn.GetComponent<UIElementSound>().clickSound = ButtonPrefab.GetComponent<UIElementSound>().clickSound;
                btn.GetComponent<UIElementSound>().hoverSound = ButtonPrefab.GetComponent<UIElementSound>().hoverSound;
            }
            */
                
                //btn.colors = Higlighted;
                //btn.GetComponent<Image>().enabled = false;
               // btn.GetComponent<Image>().enabled = true;
           
            
        }
    }

    

    void TMProFontchange ()
    {
        AllTextArr = FindObjectsOfType<TextMeshProUGUI>(true);

        foreach (TextMeshProUGUI txt in AllTextArr)
        {
            txt.font = Source;
            txt.fontSize *= 150 / 100;
        }
    }


    


}
