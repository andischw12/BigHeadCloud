using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Michsky.UI.ModernUIPack;

public enum MainMenuWinows {Game, Inventory, Shop, Performence,ChooseRoom,PlayWithFriend};
public class MenuButtonRG2 : MonoBehaviour
{
    public MainMenuWinows ButtonType;
     


    void Start()
    {
        Button btn = gameObject.AddComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        

          


    }

    void TaskOnClick()
    {
        FindObjectOfType<MainMenuManager>().MainMenuWindowsManager.OpenPanel(ButtonType);
         
         
            
    }
}
