using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI priceText;
    [SerializeField] public Image icon;
    [SerializeField] public TextMeshProUGUI Text;
    [SerializeField] public AvatarInfoList type;
    [SerializeField] public int itemNum;
    [SerializeField] public int price;
    [SerializeField] public Button myButton;
    [SerializeField] bool ImClicked;
    [SerializeField] public GameObject NewMassage;
    


    void Start()
    {
        FindObjectOfType<NotificationsManager>().CurrentSceneNotifications[2].confirmButton.onClick.AddListener(OnConfirmClick);
        FindObjectOfType<NotificationsManager>().CurrentSceneNotifications[2].cancelButton.onClick.AddListener(OnCancelClick);
        myButton.onClick.AddListener(OnClick);
    }

    void OnClick() 
    {
       
        if (FamilyManager.instance.GetStoreItemState(type,itemNum) == 1)
            FindObjectOfType<NotificationsManager>().CurrentSceneNotifications[0].OpenWindow();
        else if (price > FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Gems))
            FindObjectOfType<NotificationsManager>().CurrentSceneNotifications[1].OpenWindow();
        else 
        {
            FindObjectOfType<NotificationsManager>().CurrentSceneNotifications[2].OpenWindow();
            ImClicked =true;
        }
             
 

    }

    public void BuyItem() 
    {
        if (ImClicked) 
        {
            Debug.Log("im on click");
            FamilyManager.instance.SetStoreItemState(type,itemNum,1);
            int currentGemsAmount = FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Gems);
            int newGemsAmmount = Mathf.Max(currentGemsAmount - price, 0);
            FamilyManager.instance.SetActiveKidInfoValue(UserInfoList.Gems, newGemsAmmount);
            FamilyManager.instance.SetActiveKidInfoValue(UserInfoList.GemsSpent, FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.GemsSpent) + price);
            MainMenuManager.instance.UpdateStatistics();
            FindObjectOfType<InventoryMenuManager>().SetAvaliableItems();
            ImClicked = false;
            FindObjectOfType<StarsEffect>().Play();
        }
        
    }

    public void OnConfirmClick() 
    {
        BuyItem();
    }
    public void OnCancelClick()
    {
        ImClicked = false;
    }
}
