using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Michsky;
using Michsky.UI.ModernUIPack;
 
using TMPro;
 


public class PlayerSelectionButton : MonoBehaviour
    {
        
        [SerializeField] int PlayerNumber;
        bool DeleteButtonIsClicked;
        bool HoverIsOn;
        [SerializeField] Button DeleteButton;
        public ModalWindowManager deleteNotificationWindow;
        public KidAvatarSelector PlayerAvatar;
        public RawImage PlayerAvatarImage;
        public TextMeshProUGUI PlayerName;
        public TextMeshProUGUI Rank;
        

        private void Start()
        {
            deleteNotificationWindow = FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[LoginScreenNotificationList.DeleteUser.GetHashCode()];
        //OnHoverExit();
             DeleteButton.gameObject.SetActive(false);
            
             deleteNotificationWindow.confirmButton.onClick.AddListener(DeleteThisPlayer);
            deleteNotificationWindow.cancelButton.onClick.AddListener(() => SetDeleteButtonIsClicked(false));
            DeleteButton.onClick.AddListener(() => SetDeleteButtonIsClicked(true));
            PlayerAvatarImage = FindObjectOfType<RawImage>();
           // PlayerAvatar = LoginScreenManager.instance.AvatarInstances[PlayerNumber].GetComponentInChildren<KidAvatarSelector>();
        }

        
        public void OnHoverEnter()
        {
         HoverIsOn = true;
         StartCoroutine(OnHoverEnterHelper());
        }


        IEnumerator OnHoverEnterHelper() 
        {
            yield return new WaitForSeconds(0.3f);
            if (HoverIsOn) 
            {
                PlayerAvatar.GetComponentInChildren<Animator>().SetBool("Waving", true);
                DeleteButton.gameObject.SetActive(true);
            }
        }
        public void OnHoverExit()
        {
            HoverIsOn = false;
            PlayerAvatar.GetComponentInChildren<Animator>().SetBool("Waving", false);
            DeleteButton.gameObject.SetActive(false);
        }

        public void ShowWarning()
        {
            deleteNotificationWindow.OpenWindow();
        }

        public void DeleteThisPlayer() 
        {
             print("Trying to delete player");
            if (DeleteButtonIsClicked) 
            {
                FamilyManager.instance.DeleteKidUser(PlayerNumber);
                LoginScreenManager.instance.ShowActiveKidsButtons();
                SetDeleteButtonIsClicked(false);
            }
        }

        void SetDeleteButtonIsClicked(bool b) 
        {
            DeleteButtonIsClicked = b;
        }

         
    } 
 
