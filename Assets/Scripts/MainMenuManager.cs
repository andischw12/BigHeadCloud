using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Photon.Pun;
using Michsky.UI.Zone;
using Michsky.UI.ModernUIPack;

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
    [SerializeField] TextMeshProUGUI PointsTXT;
    [SerializeField] TextMeshProUGUI Gems;
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] RenderTexture UserFace;
    [SerializeField] TextMeshProUGUI SpecialPointsText;
    [SerializeField] Transform AvatarTransform;
    [SerializeField] GameObject AvatarPrefab;
    [SerializeField] public WindowManager MainMenuWindowsManager;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //int[] currentAvatar = FamilyManager.instance.GetAvatarForActiveKid();
        MainMenuWindowsManager = GetComponentInChildren<WindowManager>();
        FindObjectOfType<WindowManager>().OpenPanel(0);
        Instantiate(AvatarPrefab, AvatarTransform);
         

        FindObjectOfType<KidAvatarSelector>().SetActiveAvatarLook(FamilyManager.instance.GetAvatarForActiveKid());
   
        FindObjectOfType<KidAvatarSelector>().SetSignOff(0);

        FindObjectOfType<Animator>().applyRootMotion = false;
        UpdateStatistics();
        if (CalculationsManager.instance.PostGame) // if its after a game open in shop
            //FindObjectOfType<MainPanelManager>().PanelAnim(3);
        FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Camera>().targetTexture = UserFace;
       // FindObjectOfType<KidAvatarSelector>().GetSignateGM().SetActive(false);

    }

    public void UpdateStatistics() 
    {
        /*
        Name.text = FamilyManager.instance.GetActiveKidFullName();
        Gems.text = FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Gems).ToString();
        GemsSpent.text = FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.GemsSpent).ToString();
        Wins.text = FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Wins).ToString();
        Lose.text = FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Lose).ToString();
        Rank.text = FindObjectOfType<ProfileManager>().GetRank(FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Points)).ToString();
        PlayTime.text = "תוקד " + FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.PlayTime).ToString();
       // print(FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Points));
      
        PointsTXT.text = (FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Points) - ProfileManager.FIRST_RANK_POINTS).ToString();
         */
        Gems.text = FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Gems).ToString();
        Name.text = FamilyManager.instance.GetActiveKidFullName();
        FindObjectOfType<ProfileManager>().SetValues(FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Points));
    }

    public void SwitchUser() 
    {
        PhotonNetwork.Disconnect();
        Destroy(FindObjectOfType<PhotonLobby>().gameObject);
        Destroy(FindObjectOfType<PhotonRoom>().gameObject);
        Initiate.Fade(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(0)), Color.black, 2f);
    }

    public void OnOpenNewGameClick() 
    {
        if (FindObjectOfType<PhotonRoom>().enviorment == EnviormentList.Random || FindObjectOfType<PhotonRoom>().enviorment == EnviormentList.Shabat || FindObjectOfType<PhotonRoom>().enviorment == EnviormentList.Purim)
        {
            FindObjectOfType<PhotonLobby>().OnPlayWithFriendMasterClick();
        }
        else
            FindObjectOfType<MainPanelManager>().PanelAnim(5);
    }

    /*
    public void SetShabbatPointsTxt() 
    {
        SpecialPointsText.text = FamilyManager.instance.GetShabbatPoints() + " :תבש דוקינ" ;
    }

    public void HideShabbatPointsTxt() 
    {
        SpecialPointsText.text = "";
    }

    public void SetHanukkaPointsTxt()
    {
        SpecialPointsText.text = FamilyManager.instance.GetHanukkaPoints() + " :הכונח דוקינ";
    }

    public void HideHanukkaPointsTxt()
    {
        SpecialPointsText.text = "";
    }

    public void SetPurimPointsTxt()
    {
        SpecialPointsText.text = FamilyManager.instance.GetPurimPoints() + " :םירופ דוקינ";
    }

    public void HidePurimPointsTxt()
    {
        SpecialPointsText.text = "";
    }

    */
}
