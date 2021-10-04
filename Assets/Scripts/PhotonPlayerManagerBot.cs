using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerManagerBot : PhotonPlayerManager
{
    public override bool MycoreIsUpdated { get { return true; } }
    public override bool ReadyToStartGame {get{return true;}}
    public override bool ReadyForNewQuestion { get { return true; } }

    public override bool ReadyToStartTimer { get { return true; } }


    [SerializeField] BotConfiguration[] BotArr;
    [SerializeField] BotConfiguration SelectedBot;
    protected override void LateStart()
    {
        BotArr = GetComponentsInChildren<BotConfiguration>();
        //CreatePhotonPlayer(new int[]{0,0,3,0,6,1,0,6,3,3,8});
        SelectedBot = BotArr[Random.Range(0,BotArr.Length)];
        //CreatePhotonPlayer(Avatar.GetComponent<KidAvatarSelector>().GetBotAvatar(SelectedBot), SelectedBot.BotName, SelectedBot.BotSmartness) ;
        CreatePhotonPlayer(Avatar.GetComponent<KidAvatarSelector>().GetBotAvatar(SelectedBot),SelectedBot.BotName, SelectedBot.BotSmartness * 50*SelectedBot.BotPrefab, SelectedBot.BotSmartness);

    }

    public override void UpdateScore(int _newScore)
    {
        myPlayerAvatar.TotalScore = _newScore;
    }
     




}
