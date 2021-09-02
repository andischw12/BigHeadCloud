using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using eWolf.BookEffectV2.Interfaces;
namespace eWolf.BookEffectV2
{
    public class BookControl : MonoBehaviour
    {
        Book mybook;
    // Start is called before the first frame update
    void Start()
        {
            mybook = GetComponent<Book>();


            Invoke("LateStart",0.5f);

        }

        // Update is called once per frame

        void LateStart() 
        {
            mybook.OpenBook();
            StartCoroutine(startAnimation());
        }
        

        IEnumerator  startAnimation() 
        {
            
                Debug.Log(mybook.CanTurnPageForward);
                yield return new WaitForSecondsRealtime(3);
                mybook.TurnPage();
                mybook.Details.Pages.Add(mybook.Details.LeftPage());
                mybook.Details.Pages.Add(mybook.Details.RightPage());
                StartCoroutine(startAnimation());









        }
    }
}
