using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject HatsPanel;
    [SerializeField] GameObject GlassesPanel;
    [SerializeField] GameObject ClothsPanel;
    [SerializeField] GameObject SignatePanel;


    [SerializeField] GameObject BoysClothesPrefabList;
    [SerializeField] GameObject GirlsClothesPrefabList;


    [SerializeField] InventoryItem[] _AllItems;
    [SerializeField] GameObject ShopButton;
    [SerializeField] Material mat;
    
    [SerializeField] Sprite[] BoyHatsSprites;
    [SerializeField] string[] BoyHatsNames;
    [SerializeField] Sprite[] GirlHatsSprites;
    [SerializeField] string[] GirlHatsNames;
    [SerializeField] Sprite[] GlassesSprites;
    [SerializeField] string[] GlassesNames;
    [SerializeField] Sprite[] BoyClothesSprites;
    [SerializeField] string[] BoyClothesNames;
    [SerializeField] Sprite[] GirlClothesSprites;
    [SerializeField] string[] GirlClothesNames;
    [SerializeField] Sprite[] SignatesSprites;
    [SerializeField] string[] SignatesNames;

    [SerializeField] RangeInt Price;
    [SerializeField] RangeInt RankeNeeded;
    public RangeInt HatsRangePrice = new RangeInt(1000, 99000);
    public RangeInt GlassesRangePrice = new RangeInt(1000, 50000);
    public RangeInt ClothesRangePrice = new RangeInt(1000, 50000);
    public RangeInt SignatesRangePrice = new RangeInt(1000, 50000);
    public RangeInt HatsRangeRankNeeded = new RangeInt(1, 50);
    public RangeInt GlassesRangeRankNeeded = new RangeInt(1, 25);
    public RangeInt ClothesRangeRankNeeded = new RangeInt(1, 30);
    public RangeInt SignatesRangeRankNeeded = new RangeInt(1, 20);

    private void Start()
    {

        SetShopPanels();
        SetInventoryPanels();


    }

    public void SetShopPanels() 
    {
        PreparePanel(HatsPanel, BoyHatsSprites, BoyHatsNames, HatsRangeRankNeeded, HatsRangePrice, AvatarArrayEnum.Hats);
        PreparePanel(GlassesPanel, GlassesSprites, GlassesNames, GlassesRangeRankNeeded, GlassesRangePrice, AvatarArrayEnum.Glasses);
        PrepareClothesPanel();
        PreparePanel(SignatePanel, SignatesSprites, SignatesNames, SignatesRangeRankNeeded, SignatesRangePrice, AvatarArrayEnum.Signates);
        
    }


    public void SetInventoryPanels() 
    {
        InventoryMenuManager _tmpInventoryManager = FindObjectOfType<InventoryMenuManager>();
        _tmpInventoryManager.SetInventoryPanel(HatsPanel.GetComponentsInChildren<ShopItem>(), AvatarArrayEnum.Hats);
        _tmpInventoryManager.SetInventoryPanel(GlassesPanel.GetComponentsInChildren<ShopItem>(), AvatarArrayEnum.Glasses);
        _tmpInventoryManager.SetInventoryPanel(ClothsPanel.GetComponentsInChildren<ShopItem>(), AvatarArrayEnum.ChestGM);// add if boys if girl
        _tmpInventoryManager.SetInventoryPanel(SignatePanel.GetComponentsInChildren<ShopItem>(), AvatarArrayEnum.Signates);

    }


    //This methos Builds The store Panel
    void PreparePanel(GameObject _panelGM, Sprite[] _images,string[] _hatsNames,RangeInt _rankRangeInt, RangeInt _PriceRangeInt,AvatarArrayEnum _type) 
    {
        
        for (int i = 0; i < _images.Length;i++)
        {
            ShopItem tmp = Instantiate(ShopButton, _panelGM.transform).GetComponent<ShopItem>();
            tmp.ItemName.text = _hatsNames[i];
            tmp.priceText.text =(Mathf.RoundToInt ((_PriceRangeInt.start + ((float)i / (_images.Length+1) *  _PriceRangeInt.end))/1000)*1000).ToString() ;
            tmp.itemLevel.text = (Mathf.RoundToInt((_rankRangeInt.start + ((float)i / (_images.Length) * _rankRangeInt.end)) ) ).ToString();
            tmp.ItemPic.sprite = _images[i];
            tmp.type = _type;
        }
    }

    void PrepareClothesPanel() 
    {
        //if boys:
        Transform tmp;
        for(int i = 0; i < BoysClothesPrefabList.transform.childCount; i++) 
        {
            tmp = Instantiate(BoysClothesPrefabList.transform.GetChild(i),ClothsPanel.transform);
            tmp.GetComponent<ShopItem>().priceText.text = "3000"; 
        }

        //if girls:
        /*
        for (int i = 0; i < GirlsClothesPrefabList.transform.childCount; i++)
        {
            tmp = Instantiate(GirlsClothesPrefabList.transform.GetChild(i), ClothsPanel.transform);
            tmp.GetComponent<ShopItem>().priceText.text = "3000";
        }
        */
    }





    public void SetStore()
    {
        /*
        _AllItems = FindObjectOfType<InventoryMenuManager>().AllItems;
        SetStorePanel(AvatarArrayEnum.Hats,HatsPanel);
        SetStorePanel(AvatarArrayEnum.Glasses, GlassesPanel);
        SetStorePanel(AvatarArrayEnum.ChestGM, ClothsPanel);
        SetStorePanel(AvatarArrayEnum.LegsGm, ClothsPanel);
        SetStorePanel(AvatarArrayEnum.FeetGM, ClothsPanel);
        SetStorePanel(AvatarArrayEnum.Signates,SignatePanel);
        */

    }

    public void SetStorePanel(AvatarArrayEnum listItem,GameObject _panel) 
    {
        /*
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
      
        }*/
    }


    



}
