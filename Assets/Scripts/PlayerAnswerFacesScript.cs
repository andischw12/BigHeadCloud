using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnswerFacesScript : MonoBehaviour
{
    //public Transform[] children;

    [SerializeField]GameObject p1;
    [SerializeField]GameObject p2;
    // Start is called before the first frame update
    void Start()
    {
        //children = GetComponentsInChildren<Transform>();   
        
    }

    public void resetPlayerFaces()
    {
        p1.SetActive(false);
        p2.SetActive(false);
    }

    public void choosePlayerFace(int player) 
    { 
         if (player == 1) 
        {
           p1.SetActive(true);
          // p2.gameObject.SetActive(false);
        }
        else 
        {
           // p1.gameObject.SetActive(false);
            p2.gameObject.SetActive(true);
        }
    }


    /*
    public void chooseAnswerFace2(int player)
    {
        children[3].gameObject.SetActive(true);
        if (children[0].gameObject.activeInHierarchy == true || children[1].gameObject.activeInHierarchy == true)
        {
            children[0].gameObject.SetActive(false);
            children[1].gameObject.SetActive(false);
            children[2].gameObject.SetActive(true);
        }
        else if (player == 1)
        {
            children[0].gameObject.SetActive(true);
            children[1].gameObject.SetActive(false);
        }
        else if (player == 2)
        {
            children[0].gameObject.SetActive(false);
            children[1].gameObject.SetActive(true);
        }
    }
    */
}
