using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] GameObject HatsPanel;
    [SerializeField] GameObject GlassesPanel;
    [SerializeField] GameObject ClothsPanel;
    [SerializeField] GameObject SignatePanel;
    [SerializeField] InventoryItem[] _AllItems;
    [SerializeField] GameObject ShopButton;
    [SerializeField] Material mat;

    public void SetStore()
    {
        _AllItems = FindObjectOfType<InventoryMenuManager>().AllItems;
        SetStorePanel(AvatarArrayEnum.Hats,HatsPanel);
        SetStorePanel(AvatarArrayEnum.Glasses, GlassesPanel);
        SetStorePanel(AvatarArrayEnum.ChestGM, ClothsPanel);
        SetStorePanel(AvatarArrayEnum.LegsGm, ClothsPanel);
        SetStorePanel(AvatarArrayEnum.FeetGM, ClothsPanel);
        SetStorePanel(AvatarArrayEnum.Signates,SignatePanel);

    }

    public void SetStorePanel(AvatarArrayEnum listItem,GameObject _panel) 
    {

        foreach (InventoryItem i in _AllItems)
        {
            if (i.type == listItem && i.gameObject.activeInHierarchy == true)
            {
                GameObject TmpShopButton = Instantiate(ShopButton, _panel.transform);
                TmpShopButton.GetComponent<ShopItem>().icon.sprite = i.ImageRenderer.sprite;
                TmpShopButton.GetComponent<ShopItem>().icon.material = mat;
                TmpShopButton.GetComponent<ShopItem>().Text.text = i.Title;
                TmpShopButton.GetComponent<ShopItem>().type = i.type;
                TmpShopButton.GetComponent<ShopItem>().itemNum = i.num;
                TmpShopButton.GetComponent<ShopItem>().price = i.price;
                TmpShopButton.GetComponent<ShopItem>().priceText.text = i.price.ToString();
                TmpShopButton.GetComponent<ShopItem>().NewMassage.SetActive(false);
                if (i.IsNew) // if the item is set as new show it at the begining and show the new icon thing
                {
                    TmpShopButton.transform.SetAsFirstSibling();
                    TmpShopButton.GetComponent<ShopItem>().NewMassage.SetActive(true);
                }
                else// if not - arrange it by price
                {
                    ShopItem[] tmpList = TmpShopButton.transform.parent.GetComponentsInChildren<ShopItem>();
                    for (int x = 0; x < tmpList.Length; x++)
                    {
                        if (TmpShopButton.GetComponent<ShopItem>().price <= tmpList[x].price && !tmpList[x].NewMassage.activeInHierarchy)
                        {
                            TmpShopButton.transform.SetSiblingIndex(tmpList[x].transform.GetSiblingIndex());
                            break;
                        }
                    }
                   
                }
            }
        }
    }
}
