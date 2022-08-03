using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Michsky.UI.ModernUIPack;

public enum MainMenuWinows {Game, Inventory, Shop, Performence,ChooseRoom,PlayWithFriend};
public class MenuButtonRG2 : MonoBehaviour
{
    public MainMenuWinows ButtonType;
    private WindowManager myManager;

    void Start()
    {
        Button btn = gameObject.AddComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        myManager = GetComponentInParent<WindowManager>();
        myManager.OpenPanel(0);
    }

    void TaskOnClick()
    {
        myManager.OpenPanel(ButtonType);
    }
}
