using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignatesControl : MonoBehaviour
{
    // Start is called before the first frame update


    public void CheckSignates() 
    {
        if (GetComponent<CanvasGroup>().alpha != 0 && transform.GetComponentInParent<CanvasGroup>().alpha!=0) 
        {
            FindObjectOfType<KidAvatarSelector>().GetSignateGM().SetActive(true);
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().ResetTrigger("SignOff");
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetTrigger("Sign");
        }
        else
        {
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetTrigger("SignOff");
            FindObjectOfType<KidAvatarSelector>().SetSignOff(0f);
        }
    }


}
