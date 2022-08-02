using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] public AvatarArrayEnum type;
    [SerializeField] public int InventoryItemnum;
    [SerializeField] public  TextMeshProUGUI Title;
     KidAvatarSelector CurrentKidAvatar;
    [SerializeField] public Image ImageRenderer;
    [SerializeField] Button myButton;
    //[SerializeField] public string Title;



    //[SerializeField] public int price;


    //[SerializeField] public  bool IsNew;
    // Start is called before the first frame update

    private void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(SelectItem);
        if(CurrentKidAvatar==null)
            print("CurrentKidAvatar is null");
        
    }

    public void SelectItem() 
    {
        CurrentKidAvatar = FindObjectOfType<KidAvatarSelector>();
        if (type == AvatarArrayEnum.ChestGM || type == AvatarArrayEnum.LegsGm || type == AvatarArrayEnum.FeetGM)
        {
            if (InventoryItemnum == CurrentKidAvatar.GetActiveAvatarLook()[type.GetHashCode()]) // if its the same cloth package dont change material
                CurrentKidAvatar.SetAvatarDressItem(type,InventoryItemnum, CurrentKidAvatar.GetActiveAvatarLook()[type.GetHashCode()+1]); // same cloth
            else// if its a diffrent one so choose a random one 
            {
                CurrentKidAvatar.SetAvatarDressItem(type, InventoryItemnum, 0); //diffrent cloths
              //  FindObjectOfType<InventoryMenuManager>().ChangeAvatarColorEffect();
            }
               

            FindObjectOfType<InventoryMenuManager>().ShowSelectors(type, InventoryItemnum);

        }

        else 
        {
            CurrentKidAvatar.SetAvatarDressItem(type, InventoryItemnum); // change Avatr
            FindObjectOfType<InventoryMenuManager>().HideSelectors();
             


        }
        FamilyManager.instance.SetAvatarForActiveKid(CurrentKidAvatar.GetActiveAvatarLook()); //Change kid array
        
    }
   
}
