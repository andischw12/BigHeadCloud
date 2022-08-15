using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.SceneManagement;
using AG_WebGLFPSAccelerator;


public class LoginScreenManager : MonoBehaviour
{
    public static LoginScreenManager instance;
    [SerializeField] public PlayerSelectionButton[] AllPlayers;
    [SerializeField] GameObject[] All3DScene;
    [SerializeField] GameObject GameSelector3DScene;
    [SerializeField] Button AddPlayerButton;
    [SerializeField] Button EnterNameNextButton;
    [SerializeField] TextMeshProUGUI NameTxtInput;
    [SerializeField] RawImage AvatarImage;
    [SerializeField] Button NextAvatarButton;
    [SerializeField] Button PrevAvatarButton;
    WindowManager loginWindowManager;
    string newName;
    public delegate void TestDelegate(int i);
    [SerializeField] Transform[] AvatarTransformPlace;

    [SerializeField] GameObject AvatarPrefab;
    [SerializeField] public  GameObject[] AvatarInstances = new GameObject[4];
    [SerializeField] Transform AvatarPlayerChoosingTransform;
    [SerializeField] public GameObject AvatarPlayerChoosingInstance;
    public GameObject LogoGM;



    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this.gameObject); else instance = this; // singelton
    }

    void Start()
    {
#if (!UNITY_EDITOR && !DEVELOPMENT_BUILD)
            GameManager.pushWinnersJS();
#endif
        loginWindowManager = FindObjectOfType<WindowManager>();
        loginWindowManager.OpenFirstTab();
        //FindObjectOfType<WebGLFPSAccelerator>().dynamicResolutionSystem = false;
        AddPlayerButton.onClick.AddListener(AddKidUserButton);
        EnterNameNextButton.onClick.AddListener(OnEnterNameNextClick);
        PrevAvatarButton.onClick.AddListener(ChangeAvatarColorEffect);
        NextAvatarButton.onClick.AddListener(ChangeAvatarColorEffect);
        ShowHideAddNewPlayer();
        FindObjectOfType<HorizontalSelector>().defaultIndex = 0;
        SoundManager.instance.PlayMenuMusic();
        PlayerPrefs.SetInt("AutoConnectAndSearch", 0);
        PlayerPrefs.SetString("LastRoomName", "");// sets enmpty roomname for not playing again
       // StartCoroutine(CheckAndStartGLAccelerator(4f));
        // GameObject Avatar[] = new GameObject[AllPlayers.Length];
         
        for (int i =0;i<AllPlayers.Length;i++)
        {
            AvatarInstances[i] = Instantiate(AvatarPrefab,AvatarTransformPlace[i]);
            
            AvatarInstances[i].GetComponent<KidAvatarSelector>().SelectAvatarByPrefab(FamilyManager.instance._kidsUserArr[i].UserAvatarArr[AvatarArrayEnum.AvatarPrefab.GetHashCode()]);
            
            AllPlayers[i].PlayerAvatar = AvatarInstances[i].GetComponent<KidAvatarSelector>();
            // AvatarPrefab[i] = GameObject.Instantiate(AvatarPrefab, AvatarTransformPlace[i]) ;
        }

        AvatarPlayerChoosingInstance = Instantiate(AvatarPrefab, AvatarPlayerChoosingTransform);
        
        //AvatarPlayerChoosingInstance.GetComponent<KidAvatarSelector>().SelectAvatarByPrefab(FamilyManager.instance._kidsUserArr[i].UserAvatarArr[AvatarArrayEnum.AvatarPrefab.GetHashCode()]);
        ChangeAvatarChoosinginstancewithCorrectClothes();


        ShowActiveKidsButtons();


    }

    public  int GetFirstDigitLoop(int number)
    {
        return (number% 10);
    }

    void ChangeAvatarChoosinginstancewithCorrectClothes()
    {
        for (int i = 0; i < AvatarPlayerChoosingInstance.GetComponent<KidAvatarSelector>().AvatarsArray.Length; i++)
        {
            AvatarPlayerChoosingInstance.GetComponent<KidAvatarSelector>().SelectAvatarByPrefab(i);
           // AvatarPlayerChoosingInstance.GetComponent<KidAvatarSelector>().SetJacketOff();
            if (i < 16)// if boys
            {
                 
                AvatarPlayerChoosingInstance.GetComponent<KidAvatarSelector>().SetAvatarDressItem(AvatarArrayEnum.ChestGM, 6, GetFirstDigitLoop(i));
                AvatarPlayerChoosingInstance.GetComponent<KidAvatarSelector>().SetAvatarDressItem(AvatarArrayEnum.LegsGm, 2, GetFirstDigitLoop(i));
                AvatarPlayerChoosingInstance.GetComponent<KidAvatarSelector>().SetAvatarDressItem(AvatarArrayEnum.FeetGM, 1, GetFirstDigitLoop(i));
            }
            else
            {
                AvatarPlayerChoosingInstance.GetComponent<KidAvatarSelector>().SetAvatarDressItem(AvatarArrayEnum.ChestGM, 8, GetFirstDigitLoop(i));
                AvatarPlayerChoosingInstance.GetComponent<KidAvatarSelector>().SetAvatarDressItem(AvatarArrayEnum.LegsGm, 10, GetFirstDigitLoop(i));
                AvatarPlayerChoosingInstance.GetComponent<KidAvatarSelector>().SetAvatarDressItem(AvatarArrayEnum.FeetGM, 1, GetFirstDigitLoop(i));
            }


        }
    }


    public void ShowActiveKidsButtons( ) 
    {
        int tmp = 0;
        for (int i = 0; i < 4; i++) 
        {
            if (FamilyManager.instance.IsKidActive(i))
            {
                AllPlayers[i].GetComponent<PlayerSelectionButton>().PlayerName.text  = FamilyManager.instance.GetKidFirstName(i).Replace("?", "");
                AllPlayers[i].GetComponent<PlayerSelectionButton>().Rank.text = ProfileManager.GetRank(FamilyManager.instance._kidsUserArr[i].UserGeneralInfoArr[UserArrayEnum.Points.GetHashCode()]).ToString();

                ShowPlayerButton(i);
                All3DScene[i].SetActive(true);
                tmp++;
            }
            else 
            {
                HidePlayerButton(i);
                All3DScene[i].SetActive(false);
            }
                
        }
        if (tmp > 0)
            LogoGM.SetActive(false);
        else
            LogoGM.SetActive(true);
    }

    // on the first screen, clicking new user button
    void AddKidUserButton() 
    {
        //using only 1 instance for creation new user

       // for (int i = 0; i < AvatarInstances.Length; i++) // skip last one
         //   AvatarInstances[i].SetActive(false);
        //AvatarInstances[AvatarInstances.Length - 1].SetActive(true);
        // end

        loginWindowManager.OpenPanel(1);
        NameTxtInput.GetComponentInParent<TMP_InputField>().text = "";
        GameSelector3DScene.SetActive(true);
        foreach (GameObject gm in All3DScene)
            gm.SetActive(false);
        FindObjectOfType<LoginMenuAvatarSelectManager>().ChooseBoys();
    }


    public void OnCreatePlayerClick() 
    {
     FamilyManager.instance.CreateNewKidUser(newName, FindObjectOfType<KidAvatarSelector>().SelectAvatarByPrefab(FindObjectOfType<LoginMenuAvatarSelectManager>().current));

       
        ShowActiveKidsButtons();
        GameSelector3DScene.SetActive(false);
        loginWindowManager.OpenPanel(0);
        
       // for (int i = 0; i < AvatarInstances.Length; i++)
           // AvatarInstances[i].SetActive(true);
       
        //using only 1 instance for creation new user


        // end
    }


    public void HidePlayerButton(int player) 
    {
        AllPlayers[player].gameObject.SetActive(false);
        ShowHideAddNewPlayer();
    }

    public void ShowPlayerButton(int player)
    {
        AllPlayers[player].gameObject.SetActive(true);
        AllPlayers[player].PlayerAvatar.SelectAvatarByPrefab(FamilyManager.instance._kidsUserArr[player].UserAvatarArr[AvatarArrayEnum.AvatarPrefab.GetHashCode()]);
        AllPlayers[player].PlayerAvatar.SetActiveAvatarLook(FamilyManager.instance._kidsUserArr[player].UserAvatarArr);
         
        ShowHideAddNewPlayer();


    }

    void OnEnterNameNextClick()
    {
        if (NameTxtInput.text.Length > 2)
        {
            loginWindowManager.OpenPanel(2);
            newName = NameTxtInput.text.Replace("'","").Replace("\"", "").Replace("?", "");
        }
    }
    public void SelectPlayer(int player) 
    {
        print("Player is: " + player);
        FamilyManager.instance.SetActiveKid(player);
        CalculationsManager.instance.ResetParameters();
        Initiate.Fade(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1)), Color.black, 4f);
    }

    // private methods:
    private IEnumerator CheckAndStartGLAccelerator(float timeToWait)
    {
        //Application.targetFrameRate = 60;
        yield return new WaitForSeconds(timeToWait);
        if (FindObjectOfType<FPSCounter>().m_CurrentFps < 40)
            FindObjectOfType<WebGLFPSAccelerator>().dynamicResolutionSystem = true;
    }

    private void ChangeAvatarColorEffect()
    {
        StartCoroutine(ChangeAvatarColorEffectHelper());
    }
    private IEnumerator ChangeAvatarColorEffectHelper()
    {
        NextAvatarButton.interactable = false;
        PrevAvatarButton.interactable = false;
        float fade = 1f;
        AvatarImage.material.SetFloat("_Brightness", fade);
        while (fade <= 1.7f && fade >= 1)
        {
            fade += 0.45f;
            AvatarImage.material.SetFloat("_Brightness", fade);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        while (fade > 1f)
        {
            fade -= 0.45f;
            AvatarImage.material.SetFloat("_Brightness", fade);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        NextAvatarButton.interactable = true;
        PrevAvatarButton.interactable = true;
    }
    private void ShowHideAddNewPlayer()
    {
        if (FamilyManager.instance.GetNumberOfKidUsers() == 4)
            AddPlayerButton.gameObject.SetActive(false);
        else
            AddPlayerButton.gameObject.SetActive(true);
    }
 
}
