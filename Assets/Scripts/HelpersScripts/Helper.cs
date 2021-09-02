using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Helper : MonoBehaviour
{
    
    [SerializeField]protected GameObject helperGameObject;
    [SerializeField] bool isUsed;
    [SerializeField]protected int ammount;
    [SerializeField] GameObject NotInteract;
    public int Ammount {get{return ammount;}}
    // public int PlayerSide;

    // Start is called before the first frame update
    void Awake()
    {
        helperGameObject = this.gameObject;
        
    }

    // Update is called once per frame
    public virtual void UseHelper()
    {
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.Click);
        ammount--;
        UpdateGui();
        SetUsed();
        CheckAndMakeGray();


    }

    public void MakeInteractble() 
    {
        NotInteract.SetActive(false);
        //GetComponent<Button>().interactable = true;
    }

   public void MakeNotInteractble()
    {
        NotInteract.SetActive(true);
      //  GetComponent<Button>().interactable = false;
    }

    void UpdateGui()
    {
       GetComponentInChildren<TextMeshProUGUI>().text = ammount.ToString();
    }

    void MakeColorGray() 
    {
       GetComponent<Image>().color = Color.gray;
       transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.gray;
    }

    void MakeColorNormal()
    {
        GetComponent<Image>().color = Color.white;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    public void ReadInfoFromServer()
    {
        ammount = 1;
        UpdateGui();
    }

    public void WriteInfotoServer()
    {

    }

    protected bool CanIUseHelper() 
    {
        if (ammount>0 &&!isUsed && !GameManager.instance.IsSessionOver)
            return true;
        return false;
    }

    public void SetUsed() 
    {
        isUsed = true;
        MakeNotInteractble();
    }

    public void SetUnused() 
    {
        isUsed = false;
        MakeInteractble();
    }

    public bool AmiUsed() { return isUsed; } 

    public void CheckAndMakeGray() 
    {
        if (ammount<1)
        {
            //MakeColorGray();
            MakeNotInteractble();
        }
        else
            MakeInteractble();

    }

     






}
