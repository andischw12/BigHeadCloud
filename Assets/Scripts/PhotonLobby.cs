using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using AG_WebGLFPSAccelerator;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Michsky.UI.ModernUIPack;




public class PhotonLobby : MonoBehaviourPunCallbacks
{
    
    public static PhotonLobby lobby;
    public GameObject battleButton;
    public GameObject cancelButton;
    bool waitingTimeIsOver;
    bool connectedToMaster;
    

    private void Awake()
    {
        lobby = this;
    }
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings(); //connects to Master photon server;
        FindObjectOfType<WebGLFPSAccelerator>().dynamicResolutionSystem = false;
        PhotonNetwork.OfflineMode = false;
        ConnectToPhoton();
        if (PlayerPrefs.GetInt("AutoConnectAndSearch")==1) 
        {
            StartCoroutine(AutoStartGameFunction());
            PlayerPrefs.SetInt("AutoConnectAndSearch",0);
        }
    }

    IEnumerator AutoStartGameFunction() 
    {
        FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().OpenWindow();
        FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "בוש רבחתמ";
        yield return new WaitUntil(() => battleButton.activeInHierarchy);
        OnBattleButtonClicked();
       
    }

    public void ConnectToPhoton()
    {
          StartCoroutine(ConnectToPhotonHelper());
    }
 
    IEnumerator ConnectToPhotonHelper() 
    {
        //disconnecting
        PhotonNetwork.OfflineMode = false;
        waitingTimeIsOver = false;
        PhotonNetwork.Disconnect();
        connectedToMaster = false;
        while (PhotonNetwork.IsConnected)
            yield return null;
        //check for dpi
        float CurrentFps = FindObjectOfType<FPSCounter>().m_CurrentFps;
        Debug.Log("FPS is: " + CurrentFps);
        if (CurrentFps < 18)
        {
            yield return new WaitForSecondsRealtime(5);
            print("Offline Mode From Lobby Because DPI is low");
            PhotonNetwork.OfflineMode = true;
            battleButton.SetActive(true);
            yield break;
        }
        if(CurrentFps < 25)
            FindObjectOfType<WebGLFPSAccelerator>().dynamicResolutionSystem = true;
        print("DPI is good enough, trying to connect photon");
        //try to connect photon
        PhotonNetwork.ConnectUsingSettings();
        StartCoroutine(Waiting(10f));
    }
    IEnumerator Waiting(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        if (!connectedToMaster)
        {
            Debug.Log("Trying to connect photon again");
            ConnectToPhoton();
        }
    }
 

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        connectedToMaster = true;
        battleButton.SetActive(true);
    }

    public void OnBattleButtonClicked()
    {
        Debug.Log("battle button was clicked");
        battleButton.SetActive(false);
        FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "...דדומתמ שפחמ";
        PhotonNetwork.JoinRandomRoom();
        StartCoroutine(FindObjectOfType<PhotonRoom>().Safety(18f));
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. there must be no open games available");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("trying to create a new room");
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("We are in a room");
    }
    public void OnCanelButtonClicked(int AutoConnect) // 1 to auto start searchiing for game
    {
        Debug.Log("cancel button was clicked");
        cancelButton.SetActive(false);
        FindObjectOfType<PhotonRoom>().CancelButtonClicked = true;
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        try {CalculationsManager.instance.PostGame = false;}catch{}
        PlayerPrefs.SetInt("AutoConnectAndSearch", AutoConnect);
        SceneManager.LoadScene(1);
        Destroy(FindObjectOfType<PhotonRoom>().gameObject);
        Destroy(this.gameObject);
       
    }
 

}
