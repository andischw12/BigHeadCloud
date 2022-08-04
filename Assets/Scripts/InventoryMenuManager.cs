using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.Zone;
using Michsky.UI.ModernUIPack;


public class InventoryMenuManager : MonoBehaviour
{

    [SerializeField] GameObject HatsPanel;
    [SerializeField] GameObject GlassesPanel;
    [SerializeField] GameObject ClothesPanel;
    [SerializeField] GameObject SignatePanel;


    [SerializeField] Button NextMat;
    [SerializeField] Button PreMat;
    [SerializeField] int currentPanel;
    [SerializeField] AvatarArrayEnum CurrentGMtype;
    [SerializeField] int CurrentNum;
    [SerializeField] int[] tmp;
    [SerializeField] public InventoryItem[] AllItems;
    [SerializeField] KidAvatarSelector currentKid;
    [SerializeField] GameObject[] boyGM;
    [SerializeField] GameObject[] girlGM;
    [SerializeField] RawImage AvatarImage;
    [SerializeField] GameObject InventoryItemPrefab;
    [SerializeField] public  WindowManager InventoryWindowManager;

    public bool IsPanelActive { get; set; }





    // Start is called before the first frame update
    void Start()
    {
        InventoryWindowManager = GetComponentInChildren<WindowManager>();
        NextMat.onClick.AddListener(OnClickNextMat);
        PreMat.onClick.AddListener(OnClickPreMat);
        currentKid = FindObjectOfType<KidAvatarSelector>();
        AllItems = GetComponentsInChildren<InventoryItem>();
        HideSelectors();
        // SetBoyOrGirl();
        //FindObjectOfType<ShopManager>().SetStore();
        // SetAvaliableItems();
        //SetAvaliableItems();
        

    }


    public void SetInventoryPanel(ShopItem[] _itemArr, AvatarArrayEnum _type)
    {
        GameObject panel;
        if (_type == AvatarArrayEnum.Hats)
            panel = HatsPanel;
        else if (_type == AvatarArrayEnum.Glasses)
            panel = GlassesPanel;
        else if (_type == AvatarArrayEnum.ChestGM|| _type == AvatarArrayEnum.LegsGm|| _type == AvatarArrayEnum.FeetGM)
            panel = ClothesPanel;
        else //if(_type == AvatarArrayEnum.FeetGM)
            panel = SignatePanel;

        //for(int i = 0; i < panel.transform.childCount; i++) {Destroy(panel.transform.GetChild(i).gameObject);}

        GameObject tmp;
        for (int i = 0; i < _itemArr.Length; i++)
        {

            tmp = Instantiate(InventoryItemPrefab, panel.transform);
            tmp.GetComponent<InventoryItem>().InventoryItemnum = _itemArr[i].shopItemNum;
            tmp.GetComponent<InventoryItem>().Title.text = _itemArr[i].ItemName.text;
            tmp.GetComponent<InventoryItem>().ImageRenderer.sprite= _itemArr[i].ItemPic.sprite;
            tmp.GetComponent<InventoryItem>().type = _itemArr[i].type;

           //  check if got the item
            if (FamilyManager.instance.GetStoreItemState(tmp.GetComponent<InventoryItem>().type, tmp.GetComponent<InventoryItem>().InventoryItemnum) == 0)
                tmp.SetActive(false);
             
            
        }

    }


    public void AddItemToInventory(AvatarArrayEnum _type, int num) 
    {
        GameObject panel;
        if (_type == AvatarArrayEnum.Hats)
            panel = HatsPanel;
        else if (_type == AvatarArrayEnum.Glasses)
            panel = GlassesPanel;
        else if (_type == AvatarArrayEnum.ChestGM || _type == AvatarArrayEnum.LegsGm || _type == AvatarArrayEnum.FeetGM)
            panel = ClothesPanel;
        else //if(_type == AvatarArrayEnum.FeetGM)
            panel = SignatePanel;

        panel.transform.GetChild(num).gameObject.SetActive(true);
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
            s.gameObject.SetActive(true);
             //andy to check -> this is the correct one:
             /*
            if (FamilyManager.instance.GetStoreItemState(s.type, s.num) == 1)
                    s.gameObject.SetActive(true);
                else
                    s.gameObject.SetActive(false);
             */
        }
    }


    public void OnPanelChange(bool b)
    {
         
            if (b)
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

             
       


    }


    public void OnPanelChange(int newPanel, string panelName) 
    {
       // if (gameObject.GetComponentInParent<CanvasGroup>().alpha == 1 )
        {
            if (newPanel == 3 && panelName == "Inventory")
            {
                
                FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().ResetTrigger("SignOff");
                FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetTrigger("Sign");
                FindObjectOfType<KidAvatarSelector>().SetSignOn();
                // Debug.Log("i am working");

            }
            else   
            {
               // print("signoff");
                FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetTrigger("SignOff");
                FindObjectOfType<KidAvatarSelector>().SetSignOff(0f);
            }
            /*
            if (newPanel != 2)
            {
                HideSelectors();
            }
            currentPanel = newPanel;
            */
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
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<AvatarManager>().Nextchestcolor(0);
        if (CurrentGMtype == AvatarArrayEnum.LegsGm)
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<AvatarManager>().Nextlegscolor(0);
        if (CurrentGMtype == AvatarArrayEnum.FeetGM) 
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<AvatarManager>().Nextfeetcolor(0);
        FamilyManager.instance.SetAvatarForActiveKid(currentKid.GetActiveAvatarLook());
    }

    void OnClickPreMat()
    {
       
        if (CurrentGMtype == AvatarArrayEnum.ChestGM)
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<AvatarManager>().Nextchestcolor(1);
        if (CurrentGMtype == AvatarArrayEnum.LegsGm)
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<AvatarManager>().Nextlegscolor(1);
        if (CurrentGMtype == AvatarArrayEnum.FeetGM)
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<AvatarManager>().Nextfeetcolor(1);
        FamilyManager.instance.SetAvatarForActiveKid(currentKid.GetActiveAvatarLook());
    }


}
     