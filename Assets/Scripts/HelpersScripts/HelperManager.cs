using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperManager : MonoBehaviour
{
    public Player myPlayerAvatar;// helper script is doing the connecting
    [SerializeField] Helper[] helpers;
    // Start is called before the first frame update


    private void Awake()
    {
        helpers = GetComponentsInChildren<Helper>();

    }

    public void Start()
    {
        ReadInfoFromServer();
        
        CheckAllHelpersAmmount(); 
    }

    public void ReadInfoFromServer() 
    {
        
        foreach (Helper H in helpers)
            H.ReadInfoFromServer();
    }

    public void UseHelper(int helperNumber) 
    {
        Debug.Log(helperNumber +" is clicked");
            helpers[helperNumber].UseHelper();
    }

    public void MakeHelpersNotInteract() 
    {
        foreach (Helper H in helpers)
            H.MakeNotInteractble();
        if(GameManager.instance.IsSessionOver)
            HelpersAnimaiton(false);
    }

    public void MakeHelpersInteract()
    {
        foreach (Helper H in helpers)
        {
            if(H.Ammount>0)
                H.MakeInteractble();
        }

            
        HelpersAnimaiton(true) ;

    }

    public void HelpersAnimaiton(bool b) 
    {
        GetComponent<Animator>().SetBool("ShowHelpers",b);
    }

    public void CheckAllHelpersAmmount() 
    {
        foreach (Helper H in helpers)
            H.CheckAndMakeGray();
    }

    public void MakeAllHelpersNotUsed() 
    {
        foreach (Helper H in helpers)
            H.SetUnused();
    }

    public void MakeAllHelpersNotActive()
    {
        foreach (Helper H in helpers)
            H.transform.gameObject.SetActive(false);
            
    }

    public void MakeAllHelpersActive()
    {
        foreach (Helper H in helpers)
            H.transform.gameObject.SetActive(true);

    }




}
