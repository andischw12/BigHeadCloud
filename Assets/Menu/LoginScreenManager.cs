using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.SceneManagement;


public class LoginScreenManager : MonoBehaviour
{
    public static LoginScreenManager instance;
    [SerializeField] PlayerSelectionButton[] AllPlayers;
    [SerializeField] Button AddPlayerButton;
    [SerializeField] Button EnterNameNextButton;
    [SerializeField] TextMeshProUGUI NameTxtInput;
    [SerializeField] RawImage AvatarImage;
    [SerializeField] Button NextAvatarButton;
    [SerializeField] Button PrevAvatarButton;

    string newName;



    // Start is called before the first frame update

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
         
        AddPlayerButton.onClick.AddListener(AddKidUserButton);
        EnterNameNextButton.onClick.AddListener(EnterNameNextClick);
        PrevAvatarButton.onClick.AddListener(ChangeAvatarColorEffect);
        NextAvatarButton.onClick.AddListener(ChangeAvatarColorEffect);
        CheckAddButtonStatus();
        ShowActiveKidsButtons();
        FindObjectOfType<HorizontalSelector>().defaultIndex = 0;
        SoundManager.instance.PlayMenuMusic();
        SetWavingAnimation();
        PlayerPrefs.SetInt("AutoConnectAndSearch", 0);


    }

    public void SetWavingAnimation() 
    {
        
        //    FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetBool("Waving", true);
        
      
       
    }

    void CheckAddButtonStatus() 
    {
        if (FamilyManager.instance.GetNumberOfKidUsers() == 4)
            AddPlayerButton.gameObject.SetActive(false);
        else
            AddPlayerButton.gameObject.SetActive(true);
    }
 

    public void ShowActiveKidsButtons() 
    {
        for (int i = 0; i < 4; i++) 
        {
            if (FamilyManager.instance.IsKidActive(i))
            {
                AllPlayers[i].GetComponent<ButtonManager>().buttonText = FamilyManager.instance.GetKidFirstName(i);
                 
                ShowPlayerButton(i);
                
            }
            else
                HidePlayerButton(i);
                
        }
    }

    void AddKidUserButton() 
    {
        FindObjectOfType<WindowManager>().OpenPanel(1);
        NameTxtInput.GetComponentInParent<TMP_InputField>().text = "";
    }


    public void CreateKidUserButton() 
    {
         
        FamilyManager.instance.CreateNewKidUser(newName, FindObjectOfType<KidAvatarSelector>().SelectAvatarByPrefab(FindObjectOfType<HorizontalSelector>().index));
         
        //newName = "";
        ShowActiveKidsButtons();
        FindObjectOfType<WindowManager>().OpenPanel(0);
        
    }

    // Update is called once per frame
    public void HidePlayerButton(int p) 
    {
        
        AllPlayers[p].gameObject.SetActive(false);
        CheckAddButtonStatus();
    }

    public void ShowPlayerButton(int p)
    {
        AllPlayers[p].gameObject.SetActive(true);
        CheckAddButtonStatus();
    }

    void EnterNameNextClick()
    {
        if (NameTxtInput.text.Length > 2)
        {
            FindObjectOfType<WindowManager>().OpenPanel(2);
            //newName = NameTxtInput.text;
            //Ishay
            newName = NameTxtInput.text.Replace("'","").Replace("\"","");
        }
            
    }

    public void ChangeAvatarColorEffect() 
    {
        StartCoroutine(ChangeAvatarColorEffectHelper());
    }

    IEnumerator ChangeAvatarColorEffectHelper() 
    {
        NextAvatarButton.interactable = false;
        PrevAvatarButton.interactable = false;
        float fade = 1f;
        AvatarImage.material.SetFloat("_Brightness",fade);
        while (fade <= 2 && fade >= 1)
        {
            fade += 0.45f;
            AvatarImage.material.SetFloat("_Brightness", fade);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        while (fade>1f)
        {
            fade -= 0.45f;
            AvatarImage.material.SetFloat("_Brightness", fade);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        NextAvatarButton.interactable = true;
        PrevAvatarButton.interactable = true;


    }


    public void SelectPlayer(int player) 
    {

        FamilyManager.instance.SetActiveKid(player);
        
        CalculationsManager.instance.ResetParameters();
        Initiate.Fade(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1)), Color.black, 4f);


    }




}
