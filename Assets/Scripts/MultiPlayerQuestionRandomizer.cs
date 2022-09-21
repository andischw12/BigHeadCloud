

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;

using Photon.Realtime;
using System.IO;
public class MultiPlayerQuestionRandomizer : MonoBehaviour
{
    [SerializeField] PhotonView PV;
    public static MultiPlayerQuestionRandomizer instance;
    int currentQuestioNumber = -1;
    public  int chosenEnviorment = -1;
    public int chosenEnviormentForRandom = -1;
    //public readonly static int[] randomArr = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22};
    public readonly static int[] randomArr = { (int)QuestionSubject.RoshHashana, (int)QuestionSubject.Kippur, (int)QuestionSubject.Sukkot };
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        if (PV.Owner.ActorNumber != 1)
            Destroy(this.gameObject);
        else
        instance = this;
    }
    /// <summary>
    /// This method get a chosen enviorment and set it up over the network for both players.
    /// </summary>
    public int SetEnviorment(QuestionSubject env)
    {
        if (PhotonNetwork.IsMasterClient && chosenEnviorment < 0) 
        {
            int chosen = Random.Range(0, randomArr.Length);
            PV.RPC("RandomEnviormentNumber", RpcTarget.AllBuffered, randomArr[chosen]);
            /*
            if (env == QuestionSubject.Random) 
            {
               
                int chosen = Random.Range(0, randomArr.Length);
                PV.RPC("RandomEnviormentNumber", RpcTarget.AllBuffered, randomArr[chosen]);
            }
                
            else
            PV.RPC("RandomEnviormentNumber", RpcTarget.AllBuffered, env.GetHashCode());
            */
        }
        return chosenEnviorment;
    }
    // the photon method changing the enivorment over the network
    [PunRPC]
    private void RandomEnviormentNumber(int number)
    {
        //Debug.Log("RandomEnviormentNumber" + " from randomizer");
        chosenEnviorment = number;
    }
    /// <summary>
    /// This method choose a random question and set it up over the network for both players.
    /// </summary>
    public int SetNewMPQuestion() 
    {
        if (PhotonNetwork.IsMasterClient && currentQuestioNumber<0)
            PV.RPC("RandomQuestionNumber", RpcTarget.AllBuffered, UnityEngine.Random.Range(0, QuizManager.instance.questions.Count));
        return currentQuestioNumber;
    }
    [PunRPC]private  void RandomQuestionNumber(int number) 
    {
       // Debug.Log("RandomQuestionNumber" +" from randomizer");
        currentQuestioNumber = number;
    }
    /// <summary>
    /// This method reseting the question state ready for new selection
    /// </summary>
    public void RestQuestion()
    {
        currentQuestioNumber = -1;
    }

    public void SetEnvOnRandomMode() 
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int chosen = Random.Range(0, randomArr.Length);
            PV.RPC("SetEnvOnRandomModeRPC", RpcTarget.AllBuffered, randomArr[chosen]);
        }
           
    }

    [PunRPC]
    private void SetEnvOnRandomModeRPC(int number)
    {
        //Debug.Log("Enviorment chosen: " + number);
        chosenEnviormentForRandom = number;
    }


}
