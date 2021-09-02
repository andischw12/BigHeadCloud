using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FlipText : MonoBehaviour
{
    [SerializeField] TMP_InputField PlayerNameInput;
    public void Start()
    {
        PlayerNameInput = GetComponent<TMP_InputField>();
        PlayerNameInput.onValueChanged.AddListener(delegate { ValueChangeCheck(PlayerNameInput.text); });
    }
    public int wait = 0;
    public string str;
    public string str0;
    public int sLength;


    public void ValueChangeCheck(string s)
    {


        List<string> possibleMatches = new List<string> { "/", "-", "?", "!", " ", "א", "ב", "ג", "ד", "ה", "ו", "ז", "ח", "ט", "י", "כ", "ל", "מ", "נ", "ס", "ע", "פ", "צ", "ק", "ר", "ש", "ת", "ם", "ן", "ץ", "ף", "ך", };
        if (s.Length > 1 && s.Length > sLength && possibleMatches.Contains(s.Substring(s.Length-1)))
        {

            if (wait == 0)
            {
                     
                str = s.Substring(s.Length - 1);
                str0 = s.Substring(0, s.Length - 1);
                sLength = s.Length;
                wait = 1;
                PlayerNameInput.text = str + "" + str0;
                wait = 0;
            }


            else
            {
                PlayerNameInput.text = "";

            }
             
             
        }



        
    }
    
    private void Update()
    {
        if (PlayerNameInput.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                PlayerNameInput.text = "";
                sLength = 0;
            }
        }
        


    }

}
