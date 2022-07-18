using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.Zone;


public class InventoryMenuManager : MonoBehaviour
{
    [SerializeField] Button NextMat;
    [SerializeField] Button PreMat;
    [SerializeField] int currentPanel;
    [SerializeField] AvatarArrayEnum CurrentGMtype ;
    [SerializeField] int CurrentNum;
    [SerializeField] int[] tmp;
    [SerializeField] public InventoryItem[] AllItems;
    [SerializeField] KidAvatarSelector currentKid;
    [SerializeField] GameObject[] boyGM;
    [SerializeField] GameObject[] girlGM;
    [SerializeField] RawImage AvatarImage;
    public bool IsPanelActive { get; set; }





    // Start is called before the first frame update
    void Start()
    {
       NextMat.onClick.AddListener(OnClickNextMat);
       PreMat.onClick.AddListener(OnClickPreMat); 
        currentKid = FindObjectOfType<KidAvatarSelector>();
        AllItems = GetComponentsInChildren<InventoryItem>();
        SetBoyOrGirl();
        FindObjectOfType<StoreManager>().SetStore();
        SetAvaliableItems();
        //SetAvaliableItems();


    }


    public void SetBoyOrGirl() 
    {
        if (currentKid.GetActivePrefabNum() > 15) // if its a girl prefab
            foreach (GameObject GM in boyGM)
                GM.SetActive(false);
        else
            foreach (GameObject GM in girlGM)
                GM.SetActive(false);
    }


    public void SetAvaliableItems() 
    {
        //while (AllItems.Length == 0) { }
        foreach (InventoryItem s in AllItems)  
        {
                if (FamilyManager.instance.GetStoreItemState(s.type, s.num) == 1)
                    s.gameObject.SetActive(true);
                else
                    s.gameObject.SetActive(false);
        }
    }


    public void OnPanelChange(int newPanel, string panelName) 
    {
        if (gameObject.GetComponentInParent<CanvasGroup>().alpha == 1 )
        {
            if (newPanel == 3 && panelName == "Inventory")
            {
                FindObjectOfType<KidAvatarSelector>().GetSignateGM().SetActive(true);
                FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().ResetTrigger("SignOff");
                FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetTrigger("Sign");
               // Debug.Log("i am working");

            }
            else  
            {
               // print("signoff");
                FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetTrigger("SignOff");
                FindObjectOfType<KidAvatarSelector>().SetSignOff(0f);
            }

            if (newPanel != 2)
            {
                HideSelectors();
            }
            currentPanel = newPanel;
        }
        
        
    }
    public void ShowSelectors(AvatarArrayEnum type,int ObjectNum) 
    {
        CurrentGMtype = type;
        CurrentNum = ObjectNum;
        NextMat.gameObject.SetActive(true);
        PreMat.gameObject.SetActive(true);
    }


    public void HideSelectors()
    {
        NextMat.gameObject.SetActive(false);
        PreMat.gameObject.SetActive(false);
    }
    void OnClickNextMat()
    {
        
        if (CurrentGMtype == AvatarArrayEnum.ChestGM)
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Avater_ClothesAndSkeenMaker>().Nextchestcolor(0);
        if (CurrentGMtype == AvatarArrayEnum.LegsGm)
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Avater_ClothesAndSkeenMaker>().Nextlegscolor(0);
        if (CurrentGMtype == AvatarArrayEnum.FeetGM) 
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Avater_ClothesAndSkeenMaker>().Nextfeetcolor(0);
        FamilyManager.instance.SetAvatarForActiveKid(currentKid.GetActiveAvatarInfo());
    }

    void OnClickPreMat()
    {
       
        if (CurrentGMtype == AvatarArrayEnum.ChestGM)
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Avater_ClothesAndSkeenMaker>().Nextchestcolor(1);
        if (CurrentGMtype == AvatarArrayEnum.LegsGm)
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Avater_ClothesAndSkeenMaker>().Nextlegscolor(1);
        if (CurrentGMtype == AvatarArrayEnum.FeetGM)
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Avater_ClothesAndSkeenMaker>().Nextfeetcolor(1);
        FamilyManager.instance.SetAvatarForActiveKid(currentKid.GetActiveAvatarInfo());
    }



}
     