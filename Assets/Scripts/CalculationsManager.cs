using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculationsManager : MonoBehaviour
{
    public static CalculationsManager instance;
    [SerializeField] public int LastGameScore;
    public float LastGameSpeed;
    public int LastGameCorrectAnswers;
    public int LastGameWrongAnswers;
    public float LastGameTimePerQuestion;
    public int LastGamePointsPerQuestion;
    public int LastGameTotalNumOfQuestions;
    public bool AmIWinner;
    public bool PostGame;
    public bool TechnicalWIn;
    
    int minValToRretun;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

    }

    private void Start()
    {
        minValToRretun = 1;
    }
    
    public int GetPlayTime() 
    {
        return 2;
    }


    public int GetCaluclatedScore() 
    {
        float toReturn =  (float)LastGameScore / (LastGamePointsPerQuestion * LastGameTotalNumOfQuestions);
        return Mathf.Max((int)(toReturn *100f),minValToRretun);
    }


    public int GetCaluclatedSpeed()
    {
        float toReturn = LastGameSpeed / (LastGameTimePerQuestion * LastGameTotalNumOfQuestions);
        return Mathf.Max(100 -(int)(toReturn * 100f),minValToRretun);
    }

    public int GetCaluclatedCorrectAnswers()
    {
        float toReturn = (float)LastGameCorrectAnswers/LastGameTotalNumOfQuestions;
        
        return (int)(toReturn * 100f);
    }

    public int GetCaluclatedBonus()
    {
        
        return Mathf.Max(minValToRretun, (GetCaluclatedScore() + GetCaluclatedCorrectAnswers() + GetCaluclatedSpeed()) / 3);
    }

    public int CalculateGems() 
    {
        if (!AmIWinner)
           return Mathf.Max(GetCaluclatedBonus(),5) *2;
        int result = GetCaluclatedBonus();
        if (result < 51)
            return result * 4;
        else if (result < 71)
            return result * 5;
        else if (result < 81)
            return result * 6;
        else if (result < 91)
            return result * 7;
        else
            return result * 8;
    }



    public void ResetParameters() 
    {
        LastGameWrongAnswers = 0;
        LastGameTimePerQuestion = 0;
        LastGamePointsPerQuestion = 0;
        LastGameTotalNumOfQuestions = 0;
        LastGameCorrectAnswers = 0;
        LastGameScore = 0;
        LastGameSpeed = 0;
        AmIWinner = false;
       PostGame = false;
        TechnicalWIn = false;
    }
}
