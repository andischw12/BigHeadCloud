using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] public AvatarArrayEnum type;
    [SerializeField] public string Title;
    [SerializeField] public int num;
    [SerializeField] Button myButton;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] public int price;
    [SerializeField] public Image ImageRenderer;
    [SerializeField] KidAvatarSelector CurrentKidAvatar;
    [SerializeField] public  bool IsNew;
    // Start is called before the first frame update

    private void Start()
    {
        myButton.onClick.AddListener(SelectItem);
        
        
        if(CurrentKidAvatar==null)
        print("CurrentKidAvatar is null");
        text.text = Title;
    }

    public void SelectItem() 
    {
        CurrentKidAvatar = FindObjectOfType<KidAvatarSelector>();
        if (type == AvatarArrayEnum.ChestGM || type == AvatarArrayEnum.LegsGm || type == AvatarArrayEnum.FeetGM)
        {
            if (num == CurrentKidAvatar.GetActiveAvatarInfo()[type.GetHashCode()]) // if its the same cloth package dont change material
                CurrentKidAvatar.SetAvatarDressItem(type,num, CurrentKidAvatar.GetActiveAvatarInfo()[type.GetHashCode()+1]); // same cloth
            else// if its a diffrent one so choose a random one 
            {
                CurrentKidAvatar.SetAvatarDressItem(type, num, 0); //diffrent cloths
              //  FindObjectOfType<InventoryMenuManager>().ChangeAvatarColorEffect();
            }
               

            FindObjectOfType<InventoryMenuManager>().ShowSelectors(type, num);

        }

        else 
        {
            CurrentKidAvatar.SetAvatarAccessoryItem(type, num); // change Avatr
            FindObjectOfType<InventoryMenuManager>().HideSelectors();
             


        }
        FamilyManager.instance.SetAvatarForActiveKid(CurrentKidAvatar.GetActiveAvatarInfo()); //Change kid array
        
    }
   
}
