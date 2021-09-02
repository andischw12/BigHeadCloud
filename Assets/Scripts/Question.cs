using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Threading;
public class Question
{
    public string question; //question text
    public string[] answers = new string[4]; //array of 4 answers
    public int finalAnswer; //1 means A, 2 means B, etc.
    protected int _correctAnswer; //1 means A, 2 means B, etc.
    public string correctAnswerText; //text of the correct answer
    public string synopsis; //short explanation of the correct answer which is shown after user gave his final answer
    public Animator[] answerAnimation = new Animator[4]; // animators for each answer
    public Question() { this.GetQuestion(); }
    public int CorrectAnswer { get { return _correctAnswer; } }
    protected void GetQuestion()
    {
        this.question = QuizManager.instance.selectedQuetion.questionInfo;
        this.answers = new string[] { QuizManager.instance.selectedQuetion.options[0], QuizManager.instance.selectedQuetion.options[1], QuizManager.instance.selectedQuetion.options[2], QuizManager.instance.selectedQuetion.options[3] };
        this._correctAnswer = int.Parse(QuizManager.instance.selectedQuetion.correctAns);
        this.synopsis = "what?";
        UIManager.instance.ShowQuestion(question, answers);
    }




    public void SetFinalAnswer(int answerNumber)
    {
        if ((answerNumber < 1) || (answerNumber > 4))
        {
            answerNumber = 1;
        }
        finalAnswer = answerNumber;
    }
    public bool IsAnswerCorrect()
    {
        return finalAnswer == _correctAnswer;
    }
    public bool IsAnswerCorrect(int choise)
    {
        return choise == _correctAnswer;
    }
}
