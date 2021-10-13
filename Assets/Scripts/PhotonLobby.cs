using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using AG_WebGLFPSAccelerator;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Michsky.UI.ModernUIPack;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;




public class PhotonLobby : MonoBehaviourPunCallbacks
{
    
    public static PhotonLobby lobby;
    public GameObject Buttons;
   // public GameObject FriendBattleButton;
    //public GameObject cancelButton;
    public GameObject ConnectingGM;
    [SerializeField] TMP_InputField roomNumCode;
    public bool PlayWithFriendMode { get; set; }
    
    bool waitingTimeIsOver;
    bool connectedToMaster;
    

    private void Awake()
    {
        lobby = this;
    }
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings(); //connects to Master photon server;
        Buttons.SetActive(false);
        FindObjectOfType<WebGLFPSAccelerator>().dynamicResolutionSystem = false;
        PhotonNetwork.OfflineMode = false;
        PlayWithFriendMode = false;
        ConnectToPhoton();
        if (PlayerPrefs.GetInt("AutoConnectAndSearch")==1) 
        {
            StartCoroutine(AutoStartGameFunction());
            PlayerPrefs.SetInt("AutoConnectAndSearch",0);
        }
        else if(!PlayerPrefs.GetString("LastRoomName").Equals("")) // if they try to play again
        {
            StartCoroutine(PlayAgainWithFriend(PlayerPrefs.GetString("LastRoomName")));
            
            
        }

 

    }

    IEnumerator AutoStartGameFunction() 
    {
        FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().OpenWindow();
        FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "בוש רבחתמ";
        yield return new WaitUntil(() => Buttons.activeInHierarchy);
        OnRandomBattleButtonClicked();
       
    }
    IEnumerator PlayAgainWithFriend(string roomName)
    {
        FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().OpenWindow();
        FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "...רבחתמ";
        yield return new WaitUntil(() => Buttons.activeInHierarchy);
        FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "...ינשה שמתשמהמ הבושתל הכחמ";

        
        PhotonRoom.room.SelecetSubject(PlayerPrefs.GetInt("LastTopicPlayed"));
        if(roomName == "offline room") // if last game was against bot
        {
            print("if was against bot in room: " +  roomName);
            //StartCoroutine(FindObjectOfType<PhotonRoom>().SafetyFromPlayAgainWithFriend(2f));
            PhotonNetwork.Disconnect();
            while (PhotonNetwork.IsConnected)
                yield return null;
            int[] tmpChance = {0,1,1,0,1,1,1,0,1,0};
            int chosen = tmpChance[Random.Range(0, tmpChance.Length)];
            if (chosen == 0)
            {
                yield return new WaitForSecondsRealtime(2);
                FindObjectOfType<PhotonRoom>().flag = true;
                PhotonNetwork.OfflineMode = true;
                PhotonNetwork.JoinRandomRoom();
                StartCoroutine(FindObjectOfType<PhotonRoom>().SafetyFromPlayAgainWithFriend(18f));
            }
            else 
            {
                PhotonPlayerManagerBot.LastBotChosen = -1;
                StartCoroutine(FindObjectOfType<PhotonRoom>().SafetyFromPlayAgainWithFriend(4f));
            }
            
        }
           
        else
        {
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { IsVisible = false, IsOpen = true, MaxPlayers = 2 }, TypedLobby.Default);
            PlayWithFriendMode = true;
            StartCoroutine(FindObjectOfType<PhotonRoom>().SafetyFromPlayAgainWithFriend(18f));
        }
        PlayerPrefs.SetString("LastRoomName", "");
        

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
        PhotonNetwork.ConnectUsingSettings();
        ConnectingGM.SetActive(true);
        StartCoroutine(ConnectToPhotonHelperWaiting(10f));
        //check fps
        float CurrentFps = FindObjectOfType<FPSCounter>().m_CurrentFps;
        Debug.Log("FPS is: " + CurrentFps);
        if (CurrentFps < 25)
            FindObjectOfType<WebGLFPSAccelerator>().dynamicResolutionSystem = true;
        
    }
    IEnumerator ConnectToPhotonHelperWaiting(float time)
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
        Buttons.SetActive(true);
        ConnectingGM.SetActive(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        print("Disconnected from the photon server: "+cause );
    }

    public void OnPlayWithFriendMasterClick() 
    {
        StartCoroutine(OnPlayWithFriendMasterClickHelper());
    }

    IEnumerator OnPlayWithFriendMasterClickHelper() 
    {
        Debug.Log("i am in");
        FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().OpenWindow();
        FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "...קחשמ רצוי";
        PlayWithFriendMode = true;
        CreateRoom();
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        PhotonNetwork.CurrentRoom.IsVisible = false;
        FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = PhotonNetwork.CurrentRoom.Name + " :קחשמב רבחל הכחמ";
    }

    public void OnPlayWithFriendSlaveClick()
    {
        if (roomNumCode.text != "")
        {
            FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[4].GetComponent<ModalWindowManager>().CloseWindow();
            StartCoroutine(FindObjectOfType<PhotonRoom>().SafetyFromPlayWithFriendSlave(18f));
            FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().OpenWindow();
            FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = roomNumCode.text + " קחשמל ףרטצמ";
            PhotonNetwork.JoinRoom(roomNumCode.text);
            Debug.Log(roomNumCode.text);
        }
    }

    public void OnRandomBattleButtonClicked()
    {
        StartCoroutine(OnRandomeBattleButtonClickedHelper());
        Debug.Log("Random battle button was clicked");
    }

    IEnumerator OnRandomeBattleButtonClickedHelper() 
    {
        float CurrentFps = FindObjectOfType<FPSCounter>().m_CurrentFps;
        Debug.Log("FPS is: " + CurrentFps);
        if (CurrentFps < 18)
        {
            print("FPS is Low.Playing in offline mode");
            PhotonNetwork.Disconnect();
            while (PhotonNetwork.IsConnected)
            yield return null;
            connectedToMaster = false;
            // yield return new WaitForSecondsRealtime(5);
            //print("Offline Mode From Lobby Because DPI is low");
            PhotonNetwork.OfflineMode = true;
            // RandomBattleButton.SetActive(true);
            //ConnectingText.SetActive(false);
            //yield break;
        }
        if (CurrentFps < 25)
            FindObjectOfType<WebGLFPSAccelerator>().dynamicResolutionSystem = true;
        //print("DPI is good enough, trying to connect photon"); */
        // RandomBattleButton.SetActive(false);
        FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "...דדומתמ שפחמ";
       // if(PhotonRoom.room.enviorment == EnviormentList.Shabat)
       // {
            PhotonNetwork.JoinRandomRoom(new Hashtable { { "env", (byte)PhotonRoom.room.enviorment.GetHashCode() } }, 2);
       // }
       // else
        //PhotonNetwork.JoinRandomRoom();
        StartCoroutine(FindObjectOfType<PhotonRoom>().SafetyFromRandomButtonClick(18f));
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. there must be no open games available");
        CreateRoom();
    }



    void CreateRoom()
    {
        Debug.Log("trying to create a new room");
        int randomRoomName = Random.Range(1000, 10000);
        Debug.Log("your code is:" + randomRoomName);
         
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        string[] s = {"env"};
        roomOps.CustomRoomPropertiesForLobby = s;
        roomOps.CustomRoomProperties  = new Hashtable {{"env",(byte)PhotonRoom.room.enviorment.GetHashCode()}};
        PhotonNetwork.CreateRoom(randomRoomName.ToString(),roomOps,TypedLobby.Default);
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        if (PlayerPrefs.GetString("LastRoomName").Equals(""))// if its not another game
            CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("We are in room: " + PhotonNetwork.CurrentRoom.Name);
        
    }
    public void OnCanelButtonClicked(int AutoConnect) // 1 to auto start searchiing for game
    {
        Debug.Log("cancel button was clicked");
        //cancelButton.SetActive(false);
        FindObjectOfType<PhotonRoom>().CancelButtonClicked = true;
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();
        try {CalculationsManager.instance.PostGame = false;}catch{}
        PlayerPrefs.SetInt("AutoConnectAndSearch", AutoConnect);
        PhotonPlayerManagerBot.LastBotChosen = -1;
        SceneManager.LoadScene(1);
        Destroy(FindObjectOfType<PhotonRoom>().gameObject);
        Destroy(this.gameObject);
       
    }
 

}
