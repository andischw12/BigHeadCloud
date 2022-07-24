using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.IO;

public class PhotonPlayerManager : MonoBehaviour
{
    [SerializeField] protected GameObject Avatar;
    [SerializeField] PopupEmote myPopupEmote;
    public PhotonView PV;
    protected Player myPlayerAvatar;
    public virtual bool ReadyToStartGame { get; set; }
    public virtual bool ReadyForNewQuestion {get;set;}
    public virtual bool ReadyToStartTimer { get; set; }
    bool _myScoreIsUpdated;
    public virtual bool MycoreIsUpdated {get{ return _myScoreIsUpdated;}}





// Start is called before the first frame update
    protected virtual void Start()
    {
        Invoke("LateStart",0.5f);       
    }

    protected virtual void LateStart() 
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
            PV.RPC("CreatePhotonPlayer", RpcTarget.AllBufferedViaServer,(object) FamilyManager.instance.GetAvatarForActiveKid() , FamilyManager.instance.GetActiveKidFullName(), FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Points));
    }
    [PunRPC]protected void CreatePhotonPlayer(int[] AvatarArr,string name,int points,int botSmartness) //overloading bots
    {
        GameObject temp = Instantiate(Avatar,transform);
        temp.transform.localScale *= 8.5f; // make player correct size in scene
        temp.GetComponent<KidAvatarSelector>().SetActiveAvatarLook(AvatarArr);
        temp.gameObject.AddComponent<Player>();
        myPlayerAvatar = temp.GetComponent<Player>();
        myPlayerAvatar.myPhotonPlayer = this;
        myPlayerAvatar.gameObject.AddComponent<playerAI>();
        myPlayerAvatar.gameObject.GetComponent<playerAI>().HowSmartAmI = botSmartness;
        myPlayerAvatar.PointsForScore = points;
        myPlayerAvatar._myName = name;
    }

    
   [PunRPC] protected void CreatePhotonPlayer(int[] AvatarArr, string name,int points)
   {
        GameObject temp = Instantiate(Avatar, transform);
        temp.transform.localScale *= 8.5f; // make player correct size in scene
        temp.GetComponent<KidAvatarSelector>().SetActiveAvatarLook(AvatarArr);
        temp.gameObject.AddComponent<Player>();
        myPlayerAvatar = temp.GetComponent<Player>();
        myPlayerAvatar.myPhotonPlayer = this;
        myPlayerAvatar.PointsForScore = points;
        myPlayerAvatar._myName = name;
    }



    //Choose Answer over network
    public virtual void chooseAnswerMP(int num) 
    {
        if (PV.IsMine) 
            PV.RPC("ChooseAnswerRPC", RpcTarget.AllBufferedViaServer,num);
    }
    [PunRPC] void ChooseAnswerRPC(int num) 
    {
        if(PV.Owner.ActorNumber==1 && PhotonNetwork.IsMasterClient || PV.Owner.ActorNumber == 2 && !PhotonNetwork.IsMasterClient)
        {
            myPlayerAvatar.ChooseAnswer(num, true);
        }
        else
            myPlayerAvatar.ChooseAnswer(num, false);
    }


    // Ready to start game over netwoek
    public virtual void SetReadyToStartGame() 
    {
        if (PV.IsMine)
            PV.RPC("SetReadyToStartGameRPC", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC] void SetReadyToStartGameRPC()
    {
        Debug.Log("SetReadyToStartGameRPC " + myPlayerAvatar._myName);
        ReadyToStartGame = true;
    }
    
    // Ready for new question over network
    public virtual void SetReadyForNewQuestion()
    {
        if (PV.IsMine)
            PV.RPC("SetReadyForNewQuestionRPC", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    void SetReadyForNewQuestionRPC()
    {
        Debug.Log("SetReadyForNewQuestionRPC " + myPlayerAvatar._myName);
        ReadyForNewQuestion = true;
    }

    // Ready to start timer over network
    public virtual void SetReadyToStartTimer()
    {
        if (PV.IsMine)
            PV.RPC("SetReadyToStartTimerRPC", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    void SetReadyToStartTimerRPC()
    {
        Debug.Log("SetReadyToStartTimerRPC " + myPlayerAvatar._myName);
        ReadyToStartTimer = true;
    }


    // Stop My timer acroess network
    public void StopMyTimer(bool freezeTimerEffect)
    {
        if (PV.IsMine)
            PV.RPC("StopMyTimerRPC", RpcTarget.AllBufferedViaServer,freezeTimerEffect);
    }

    [PunRPC]
    void StopMyTimerRPC(bool freezeTimerEffect)
    {
        StartCoroutine(StopMyTimerProcess(freezeTimerEffect));
    }
    
    IEnumerator StopMyTimerProcess(bool freezeTimerEffect) 
    {
        myPlayerAvatar.playeTimer.PauseTimer();
        if (freezeTimerEffect) 
        {
            UI_Effects.instance.FreezeTimer(myPlayerAvatar.PlayerSide);
            UI_Effects.instance.StopTimeIsAlmostOver(myPlayerAvatar.PlayerSide);
        }
        yield return new WaitForSecondsRealtime(3);
        UI_Effects.instance.UnFreezeTimer(myPlayerAvatar.PlayerSide);
        if (myPlayerAvatar.CurrentAnswer == 0)// if user didnt answer while timer was stoped
        {
            myPlayerAvatar.playeTimer.StartTimer();
        }
    }
    //Update Score Over Network
    public virtual void UpdateScore(int _newScore)
    {
        if (PV.IsMine && !_myScoreIsUpdated)
            PV.RPC("UpdateScoreRPC", RpcTarget.AllBufferedViaServer,_newScore);
      
    }

    [PunRPC] void UpdateScoreRPC(int _newScore)
    {
        myPlayerAvatar.TotalScore = _newScore;
        _myScoreIsUpdated = true;
    }

    public void MyScoreIsNotUpdated() 
    {
        _myScoreIsUpdated = false;
    }

    public void ShowEmote(string str) 
    {
        if (PV.IsMine)
            PV.RPC("ShowEmoteRPC", RpcTarget.All, str);
    }
    [PunRPC] void ShowEmoteRPC(string str) 
    {
        if (!PV.IsMine)
            SoundManager.instance.PlaySoundEffect(SoundEffectsList.NewMassage);
        myPopupEmote.ShowEmote(str);
    }

}