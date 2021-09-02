using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ButtonClicked : MonoBehaviour
{
    public PhotonPlayerManager PhotonPlayerM1;
    public PhotonPlayerManager PhotonPlayerM2;
    public void chooseAnswer(int answer) 
    {
        if (PhotonNetwork.IsMasterClient) 
        {

           
            PhotonPlayerM1.chooseAnswerMP(answer);
        }
           
        else
            PhotonPlayerM2.chooseAnswerMP(answer); 
    }
}
