using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    
    [ SerializeField] public float timer = 0f;
    public bool timerbool = false;
    [SerializeField] public float QuestionTime;
    bool Flag = false; // flag for the warning of 5 secs to happne ones

    //<--ishay//
      bool Game_ifTimerStopped = false;
    
     
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerbool)
        {
            timer = timer + Time.deltaTime;
            if(timer> QuestionTime) 
            {
                //GameProcess.instance.WrongAnswer(true); // sending true to the wronganswer method because time is over
                stopTimer();
                //ResetTimer();
            }
           
            if (Mathf.RoundToInt(timer) == QuestionTime - 3)// warning on 5 secs
            {
                if (!Flag)
                {
                    Warning();
                    Flag = true;
                }
            }
        }
        GetComponentInChildren<Text>().text = (Mathf.RoundToInt(QuestionTime - timer)).ToString(); // show the right number on the text
        
   /*      //<--ishay//
        if(GameManager.ifGameOut && timerbool){
            stopTimer();
            Game_ifTimerStopped = true;
        }
        if(!GameManager.ifGameOut && Game_ifTimerStopped){
            ResetTimer();
            Game_ifTimerStopped = false;

        }
        //ishay-->// */

    }
    void Warning() 
    {
        GetComponentInChildren<Text>().color = Color.green;
        Debug.Log("Only 5 secs left!");
        StartCoroutine(WarningEffect());
    }

    public void setQuestionTime(float time) { QuestionTime = time;}
    
    public void StartTimer() 
    {
     // if(GameManager.QuestionNum%3!=0){
      //if(!GameManager.ifGameOut){

        timerbool = true;
        if (Mathf.RoundToInt(QuestionTime - timer) > 5)
        {
            //GetComponentInChildren<Text>().color = Color.white;
        }
       // GetComponentInChildren<Animator>().SetTrigger("ShowTimer");
      //  GetComponentInChildren<Animator>().SetTrigger("StartCountdown");
      //}
    }

    public void stopTimer()
    {
        timerbool = false;
      //  GetComponentInChildren<Animator>().SetTrigger("HideTimer");
    }

    public void ResetTimer()
    {
        timer = 0f;
        Flag = false;
      
    }

    IEnumerator WarningEffect() 
    {
        for(int i = 0; i <5; i++)
        {
            GetComponent<AudioSource>().Play();
            yield return new WaitForSecondsRealtime(1);
        }
    }


}
