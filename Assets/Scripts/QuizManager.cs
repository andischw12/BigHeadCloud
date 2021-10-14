using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{

    public static QuizManager instance = null;

    // Game Instance Singleton

    public static QuizManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }



#pragma warning disable 649
    //ref to the QuizGameUI script
    //[SerializeField] private QuizGameUI quizGameUI;
    //ref to the scriptableobject file
    [SerializeField] private QuizDataScriptable dataScriptable;
#pragma warning restore 649
    //questions data
    public List<QuestionAndy> questions;
    public List<QuestionGroup> groups;
    
    //current question data
    public QuestionAndy selectedQuetion = new QuestionAndy();
    [SerializeField]
    public QuestionMode QuestionMode;
    public int ChozenQuestion;
    public static string Question;
    public static string Answer1;
    public static string Answer2;
    public static string Answer3;
    public static string Answer4;
    public static string CorrectAnswer;
    public static int CorrectAnswerInt;
    private void Start()
    {

       



    }


   

    public void LoadQuestionList(int Level)
    {
        questions = new List<QuestionAndy>();
        groups.AddRange(dataScriptable.groups);
        questions.AddRange(groups[Level].questions);
        QuestionMode = QuestionMode.Null;
    }
    public void SelectQuestion()
    {
        ChozenQuestion = Random.Range(0, questions.Count);


        //set the selectedQuetion
        selectedQuetion = questions[ChozenQuestion];
        if (questions.Count != 1)
            questions.Remove(questions[ChozenQuestion]);

        //send the question to quizGameUI
        //quizGameUI.SetQuestion(selectedQuetion);
        QuestionMode = QuestionMode.WaitingForAnswer;
        Question = selectedQuetion.questionInfo;
        Answer1 = selectedQuetion.options[0];
        Answer2 = selectedQuetion.options[1];
        Answer3 = selectedQuetion.options[2];
        Answer4 = selectedQuetion.options[3];
        CorrectAnswer = selectedQuetion.correctAns;



    }

    public void SelectQuestion(int q)
    {
        ChozenQuestion = q;


        //set the selectedQuetion
        selectedQuetion = questions[ChozenQuestion];
        
        if (questions.Count != 1)
            questions.Remove(questions[ChozenQuestion]);
        
        //send the question to quizGameUI
        //quizGameUI.SetQuestion(selectedQuetion);
        
        QuestionMode = QuestionMode.WaitingForAnswer;
        //print(selectedQuetion.questionInfo);
        Question = selectedQuetion.questionInfo;
        Answer1 = selectedQuetion.options[0];
        Answer2 = selectedQuetion.options[1];
        Answer3 = selectedQuetion.options[2];
        Answer4 = selectedQuetion.options[3];
        CorrectAnswer = selectedQuetion.correctAns;



    }

    /// <summary>
    /// Method called to check the answer is correct or not
    /// </summary>
    /// <param name="selectedOption">answer string</param>
    /// <returns></returns>
    public bool Answer(string selectedOption)
    {
        //set default to false
        bool correct = false;
        //if selected answer is similar to the correctAns
        if (selectedQuetion.correctAns == selectedOption)
        {
            //Yes, Ans is correct
            correct = true;
            QuestionMode = QuestionMode.Right;
        }
        else
        {
            QuestionMode = QuestionMode.Wrong;
            //No, Ans is wrong
        }
        //call SelectQuestion method again after 1s
        Invoke("SelectQuestion", 0.4f);
        //return the value of correct bool
        return correct;
    }
}

//Datastructure for storeing the quetions data
[System.Serializable]
public class QuestionAndy
{
    public string questionInfo;         //question text
    public QuestionType questionType;   //type
    public Sprite questionImage;        //image for Image Type
    public AudioClip audioClip;         //audio for audio type
    public UnityEngine.Video.VideoClip videoClip;   //video for video type
    public List<string> options;        //options to select
    public string correctAns;           //correct option
                                        // public int CorrectAnsInt;
}

[System.Serializable]
public enum QuestionType
{
    TEXT,
    IMAGE,
    AUDIO,
    VIDEO
}
[System.Serializable]

public enum QuestionMode
{
    Null,
    Asking,
    WaitingForAnswer,
    Wrong,
    Right
}


