using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAI : MonoBehaviour
{
    public void startAiSesseion() { StartCoroutine(startAiSesseionHelper()); }
    int _howSmartAmI; 
    public int HowSmartAmI {get{return _howSmartAmI; } set { _howSmartAmI = value;}} //0-100
    IEnumerator startAiSesseionHelper()
    {
        int time = Random.Range(2, 12);
        yield return new WaitForSecondsRealtime(time);
       // if(GetComponent<BotPlayer>().playeTimer.GetTimerValue() != 1) // if time is not over
             GetComponent<Player>().ChooseAnswer(ClalucateAnswer(),false);
    }


    int ClalucateAnswer()
    {
        int[] arr = new int[100];
        int correctAnsewr = GameProcess.instance.question.CorrectAnswer;
        int i = 0;
        for (; i < HowSmartAmI; i++)
            arr[i] = correctAnsewr;
        for (int j = i; j < arr.Length; j++)
            arr[j] = PickWrongAnswer();

        return  arr[Random.Range(0,arr.Length)];
    }

    int PickWrongAnswer() 
    {
        int toReturn = 0;
        while (toReturn==0 || toReturn== GameProcess.instance.question.CorrectAnswer) 
        {
            toReturn = Random.Range(1, 5);
        }
        return toReturn;
    }
 
}
