using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignsTextures : MonoBehaviour
{

    // 0-empty 1-A 2-B 3-C 4-D 5-?
    // Start is called before the first frame update
    [SerializeField] Sprite[] sprites;
 
 
    public void chooseAnswer(int answer)
    {
      
        for (int i = 0; i < sprites.Length; i++) 
        {
            if(i==answer-1)
                GetComponent<SpriteRenderer>().sprite = sprites[i];
        }
         
    }
 
}
