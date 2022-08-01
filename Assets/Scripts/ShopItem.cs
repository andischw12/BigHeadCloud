using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI priceText; int price {get{return int.Parse(priceText.text);}}
    [SerializeField] public TextMeshProUGUI ItemName;
    [SerializeField] public TextMeshProUGUI itemLevel;
    [SerializeField] public AvatarArrayEnum type;
    [SerializeField] public Image ItemPic;
    [SerializeField] public int itemNum;

    

    // This is for the confirmation of buying the item
    [SerializeField] public Button myButton;
    [SerializeField] bool ImClicked;
    [SerializeField] public GameObject NewMassage;
    


    void Start()
    {
        FindObjectOfType<NotificationsManager>().CurrentSceneNotifications[2].confirmButton.onClick.AddListener(OnConfirmClick);
        FindObjectOfType<NotificationsManager>().CurrentSceneNotifications[2].cancelButton.onClick.AddListener(OnCancelClick);

       // myButton.onClick.AddListener(OnClick);
    }

    void OnClick() 
    {
       /*
        if (FamilyManager.instance.GetStoreItemState(type,itemNum) == 1)
            FindObjectOfType<NotificationsManager>().CurrentSceneNotifications[0].OpenWindow();
        else if (price > FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Gems))
            FindObjectOfType<NotificationsManager>().CurrentSceneNotifications[1].OpenWindow();
        else 
        {
            FindObjectOfType<NotificationsManager>().CurrentSceneNotifications[2].OpenWindow();
            ImClicked =true;
        }
             
    */

    }

    public void BuyItem() 
    {
        if (ImClicked) 
        {
            Debug.Log("im on click");
            FamilyManager.instance.SetStoreItemState(type,itemNum,1);
            int currentGemsAmount = FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Gems);
            int newGemsAmmount = Mathf.Max(currentGemsAmount - price, 0);
            FamilyManager.instance.SetActiveKidInfoValue(UserArrayEnum.Gems, newGemsAmmount);
            FamilyManager.instance.SetActiveKidInfoValue(UserArrayEnum.GemsSpent, FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.GemsSpent) + price);
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
