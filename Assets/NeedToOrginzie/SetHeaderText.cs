using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetHeaderText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HeaderText;
   public void SetHeaderTextFunc(string s) 
    {
        HeaderText.text = s;
    }
}
