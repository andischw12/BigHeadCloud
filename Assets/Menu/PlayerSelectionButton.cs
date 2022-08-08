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
        [SerializeField] Button DeleteButton;
        ModalWindowManager deleteNotificationWindow;
        public KidAvatarSelector PlayerAvatar;
        public RawImage PlayerAvatarImage;
        public TextMeshProUGUI PlayerName;
        public TextMeshProUGUI Rank;
        

        private void Start()
        {
            deleteNotificationWindow = FindObjectOfType<NotificationsManager>().CurrentSceneNotifications[LoginScreenNotificationList.DeleteUser.GetHashCode()];
            OnHoverExit();
            FindObjectOfType<ModalWindowManager>().confirmButton.onClick.AddListener(DeleteThisPlayer);
            FindObjectOfType<ModalWindowManager>().cancelButton.onClick.AddListener(() => SetDeleteButtonIsClicked(false));
            DeleteButton.onClick.AddListener(() => SetDeleteButtonIsClicked(true));
            PlayerAvatarImage = FindObjectOfType<RawImage>();
           // PlayerAvatar = LoginScreenManager.instance.AvatarInstances[PlayerNumber].GetComponentInChildren<KidAvatarSelector>();
        }

        
        public void OnHoverEnter()
        {
         PlayerAvatar.GetComponentInChildren<Animator>().SetBool("Waving", true);

        DeleteButton.gameObject.SetActive(true);
        }

        public void OnHoverExit()
        {
            PlayerAvatar.GetComponentInChildren<Animator>().SetBool("Waving", false);
            DeleteButton.gameObject.SetActive(false);
        }

        public void ShowWarning()
        {
            deleteNotificationWindow.OpenWindow();
        }

        public void DeleteThisPlayer() 
        {
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
 
