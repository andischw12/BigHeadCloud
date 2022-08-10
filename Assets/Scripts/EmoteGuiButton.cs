using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EmoteGuiButton : MonoBehaviour
{
    bool isShown = false;
    public Player myPlayer;
     
    // Start is called before the first frame update

    private void Start()
    {
        HideButton();
        HideIcons();
    }

    public void ShowIcons() 
    {
        isShown = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HideIcons()
    {
        isShown = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void buttonClicked() 
    {
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.Click);
        if (isShown)
            HideIcons();
        else
            ShowIcons();
    }

    public void HideButton() 
    {

        GetComponent<Image>().enabled = false;

        HideIcons();
    }

    public void ShowButton()
    {

        GetComponent<Image>().enabled = true; 
        ShowIcons();
    }


    public void MakeButtonNotAvaliable() 
    {
        HideIcons();
        GetComponent<Button>().interactable = false;
    }

    public void MakeButtonAvaliable()
    {
        if(!HostCharachter.instance.IsTalkingInProgress)
            GetComponent<Button>().interactable = true;
    }

    public void EmojieClicked(string str) 
    {
         
        HideIcons();
        myPlayer.myPhotonPlayer.ShowEmote(str);
        MakeButtonNotAvaliable();
        Invoke("MakeButtonAvaliable",2.5f);
    }

 

}
