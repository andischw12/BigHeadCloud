using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
 
using Photon.Realtime;
using System.IO;
 
 
public class GameProcess : MonoBehaviour
{
    public static GameProcess instance; // static variable which is used to get reference to GameProcess instance from every script
    public Question question;
    public int currentQuestionNumber = 0; // number of current question
    public bool[] isAnswerAvailable = new bool[4]; //some answers may be unavailable after using 50x50 lifeline
    [SerializeField] Question newQuestion;
    [SerializeField] Question[] questions;
     
 
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
     
    public void LoadQuestion()
    {
        StartCoroutine(LoadQuestionHelper());  
    }
    IEnumerator LoadQuestionHelper() 
    {
        FindObjectOfType<UlpanScreenManager>().SetVsPic();
        // Assignment.instance.TechMassage.text = "...רבחתמ";
        if (GameManager.instance.IsNewRandomMode()) 
        {
            if (currentQuestionNumber > 0)
            {
                MultiPlayerQuestionRandomizer.instance.SetEnvOnRandomMode();
                yield return new WaitUntil(() => MultiPlayerQuestionRandomizer.instance.chosenEnviormentForRandom > -1);
                int chosen = MultiPlayerQuestionRandomizer.instance.chosenEnviormentForRandom;
                FindObjectOfType<UlpanScreenManager>().SetText(GameManager.instance.GetSubjectText(chosen));
                QuestionSubjectManager.instance.ChooseQuestionSubject(chosen);
                GameManager.instance.SetQuiz(chosen);
                MultiPlayerQuestionRandomizer.instance.chosenEnviormentForRandom = -1;
            }
        }
        FindObjectOfType<UlpanScreenManager>().SetText(GameManager.instance.GetSubjectText(QuestionSubjectManager.instance.GetActiveQuestionSubject()));
        GameManager.instance.thisComputerPlayer.myPhotonPlayer.SetReadyForNewQuestion();
        yield return new WaitUntil(() => GameManager.instance.otherPlayer.myPhotonPlayer.ReadyForNewQuestion);
        GameManager.instance.player1.myPhotonPlayer.ReadyForNewQuestion = false;
        GameManager.instance.player2.myPhotonPlayer.ReadyForNewQuestion = false;
        //  Assignment.instance.TechMassage.text = "רבוחמ";
        yield return new WaitUntil(() => QuestionSelected());
        currentQuestionNumber++;
        
        GameManager.instance.StartQuestionProcess();
         
        for (int i = 0; i <= 3; i++)
        {
            isAnswerAvailable[i] = true;
        }
       
        question = new Question();
       
    }
    private bool QuestionSelected() 
    {
        if (MultiPlayerQuestionRandomizer.instance.SetNewMPQuestion() < 0)
            return false;
        QuizManager.instance.SelectQuestion(MultiPlayerQuestionRandomizer.instance.SetNewMPQuestion());
        MultiPlayerQuestionRandomizer.instance.RestQuestion();
        return true;
    }
}
