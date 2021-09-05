using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Photon.Pun;
using Michsky.UI.Zone;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI Wins;
    [SerializeField] TextMeshProUGUI Lose;
    [SerializeField] TextMeshProUGUI PlayTime;
    [SerializeField] TextMeshProUGUI Rank;
    [SerializeField] TextMeshProUGUI RankSmall;
    [SerializeField] TextMeshProUGUI GemsSpent;
    [SerializeField] TextMeshProUGUI Prize;
    [SerializeField] TextMeshProUGUI Gems;
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] RenderTexture UserFace;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        int[] currentAvatar = FamilyManager.instance.GetAvatarForActiveKid();
        FindObjectOfType<KidAvatarSelector>().SetAvatar(currentAvatar);
        FindObjectOfType<KidAvatarSelector>().GetSignateGM().SetActive(false);
        FindObjectOfType<KidAvatarSelector>().SetSignOff(0);
        FindObjectOfType<Animator>().applyRootMotion = false;
        UpdateStatistics();
        if (CalculationsManager.instance.PostGame) // if its after a game open in shop
            FindObjectOfType<MainPanelManager>().PanelAnim(2);
        FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Camera>().targetTexture = UserFace;
    }

    public void UpdateStatistics() 
    {
        Name.text = FamilyManager.instance.GetActiveKidFullName();
        Gems.text = FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Gems).ToString();
        GemsSpent.text = FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.GemsSpent).ToString();
        Wins.text = FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Wins).ToString();
        Lose.text = FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Lose).ToString();
        PlayTime.text = "תוקד " + FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.PlayTime).ToString();
    }

    public void Home() 
    {
        PhotonNetwork.Disconnect();
        Destroy(FindObjectOfType<PhotonLobby>().gameObject);
        Destroy(FindObjectOfType<PhotonRoom>().gameObject);
        Initiate.Fade(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(0)), Color.black, 2f);
    }
 

     
}
