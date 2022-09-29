using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Michsky.UI.ModernUIPack;

public enum MainMenuWinows {Game, Inventory, Shop, Performence,ChooseRoom,PlayWithFriend};
public class MenuButtonRG2 : MonoBehaviour
{
    public MainMenuWinows ButtonType;
    Button btn;


    void Start()
    {
        if (GetComponent<Button>() == null)
            btn = gameObject.AddComponent<Button>();
        else
            btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        

          


    }

    void TaskOnClick()
    {
        FindObjectOfType<MainMenuManager>().MainMenuWindowsManager.OpenPanel(ButtonType);
         
         
            
    }
}
