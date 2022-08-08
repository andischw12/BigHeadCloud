using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
public enum LoginScreenNotificationList {DeleteUser}
 
public class RGNotificationsManager : MonoBehaviour
{
    public ModalWindowManager[] CurrentSceneNotifications;
    public static RGNotificationsManager instance;
    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this.gameObject); else instance = this; // singelton
    }
}
 