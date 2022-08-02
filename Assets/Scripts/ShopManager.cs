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
    bool isGirl;

    private void Start()
    {
        isGirl = true;
        SetShopPanels();
        SetInventoryPanels();


    }

    public void SetShopPanels() 
    {
        if(isGirl)
            PreparePanel(HatsPanel, GirlHatsSprites, GirlHatsNames, HatsRangeRankNeeded, HatsRangePrice, AvatarArrayEnum.Hats);
        else
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
            //tmp.priceText.text =(Mathf.RoundToInt ((_PriceRangeInt.start + ((float)i / (_images.Length+1) *  _PriceRangeInt.end))/1000)*1000).ToString() ;
            //tmp.itemLevel.text = (Mathf.RoundToInt((_rankRangeInt.start + ((float)i / (_images.Length) * _rankRangeInt.end)) ) ).ToString();
            tmp.priceText.text = CalculatePrice(i,_type).ToString();
            tmp.itemLevel.text = CalculateRank(i,_type).ToString();
            tmp.ItemPic.sprite = _images[i];
            tmp.type = _type;
        }
    }

        //need to fix the formulas and just change the first parameter of the pow
    int CalculatePrice(int x, AvatarArrayEnum type) 
    {
        if (type == AvatarArrayEnum.Glasses)
            return (1000 * Mathf.Min(4, x)) + (1000 * (int)(0.7f * Mathf.Pow(1.15f, 1.104f * x)));

        else if (type == AvatarArrayEnum.Signates)
            return (1000 * Mathf.Min(4, x)) + (1000 * (int)(0.7f * Mathf.Pow(1.9f, 1.104f * x)));

        else if (type == AvatarArrayEnum.ChestGM)
            return (1000 * Mathf.Min(4, x)) + (1000 * (int)(0.7f * Mathf.Pow(1.25f, 1.104f * x)));

        //hats boys and girls
        if(isGirl)
            return (1000 * Mathf.Min(4, x)) + (1000 * (int)(0.7f * Mathf.Pow(1.19f, 1.104f * x)));
        else
            return (1000*Mathf.Min(4,x)) +(1000* (int)(0.7f * Mathf.Pow(1.115f,1.104f*x)));
    }

    int CalculateRank(int x, AvatarArrayEnum type)
    {
       

        if (type == AvatarArrayEnum.Glasses)
            return Mathf.Min(50,1 + (int)(0.75f * Mathf.Pow(1.15f, 1.104f * x)));

        else if (type == AvatarArrayEnum.Signates)
            return Mathf.Min(50,1+ (int)(0.75f * Mathf.Pow(1.9f, 1.104f * x)));

        else if(type == AvatarArrayEnum.ChestGM)
            return Mathf.Min(50, 1 + (int)(0.75f * Mathf.Pow(1.25f, 1.104f * x)));

        //hats boys and girls
        if (isGirl)
            return Mathf.Min(50, 1 + (int)(0.75f * Mathf.Pow(1.175f, 1.104f * x)));
        else
            return Mathf.Min(50, 1 +(int)(0.75f * Mathf.Pow(1.1f, 1.104f * x)));

    }

    void PrepareClothesPanel() 
    {
        //if boys:
        Transform tmp;

        GameObject BoyOrGirl = GirlsClothesPrefabList;
        for(int i = 0; i < BoyOrGirl.transform.childCount; i++) 
        {
            tmp = Instantiate(BoyOrGirl.transform.GetChild(i),ClothsPanel.transform);
            tmp.GetComponent<ShopItem>().priceText.text = CalculatePrice(i, AvatarArrayEnum.ChestGM).ToString();
            tmp.GetComponent<ShopItem>().itemLevel.text = CalculateRank(i,AvatarArrayEnum.ChestGM).ToString();
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
