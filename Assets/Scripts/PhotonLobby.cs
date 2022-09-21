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
    public GameObject ConnectingGM;
    //public GameObject JoinAFriendButton;
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
        Buttons.SetActive(false);
        //JoinAFriendButton.SetActive(false);
        PhotonNetwork.OfflineMode = false;
        PlayWithFriendMode = false;
        ConnectToPhoton(2);
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
        FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().OpenWindow();
        FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "בוש רבחתמ";
        yield return new WaitUntil(() => Buttons.activeInHierarchy);
        OnRandomBattleButtonClicked();
    }
    IEnumerator PlayAgainWithFriend(string roomName)
    {
        FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().OpenWindow();
        FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "...רבחתמ";
        yield return new WaitUntil(() => Buttons.activeInHierarchy);
        FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "...ינשה שמתשמהמ הבושתל הכחמ";

        
        PhotonRoom.room.SelecetSubject(PlayerPrefs.GetInt("LastTopicPlayed"));
        PhotonRoom.room.SelecetEnviorment(PlayerPrefs.GetInt("LastEvPlayed"));
        if (roomName == "offline room") // if last game was against bot
        {
            //print("if was against bot in room: " +  roomName);
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
                StartCoroutine(FindObjectOfType<PhotonRoom>().SafetyFromPlayAgainWithFriend(25f));
            }
            else 
            {
                PhotonPlayerManagerBot.LastBotChosen = -1;
                StartCoroutine(FindObjectOfType<PhotonRoom>().SafetyFromPlayAgainWithFriend(4f));
            }
            
        }
           
        else
        {
            RoomOptions roomOps = new RoomOptions() { IsVisible = false, IsOpen = true, MaxPlayers = 2 };
            string[] s = { "env" };
            roomOps.CustomRoomPropertiesForLobby = s;
            roomOps.CustomRoomProperties = new Hashtable { { "env", (byte)PhotonRoom.room.Enviorment.GetHashCode() } };
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOps, TypedLobby.Default);
            PlayWithFriendMode = true;
            StartCoroutine(FindObjectOfType<PhotonRoom>().SafetyFromPlayAgainWithFriend(25f));
        }
        PlayerPrefs.SetString("LastRoomName", "");
        

    }

    public void ConnectToPhoton(int Sessions)
    {
          StartCoroutine(ConnectToPhotonHelper(Sessions));
    }
 
    IEnumerator ConnectToPhotonHelper(int Sessions) 
    {
        //disconnecting
        PhotonNetwork.OfflineMode = false;
        waitingTimeIsOver = false;
        connectedToMaster = false;
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        PhotonNetwork.ConnectUsingSettings();
        ConnectingGM.SetActive(true);
        StartCoroutine(ConnectToPhotonSaftey(10f, Sessions));
    }
    IEnumerator ConnectToPhotonSaftey(float SessionTime,int Sessions)
    {
        yield return new WaitForSecondsRealtime(SessionTime);
        if (!connectedToMaster)
        {
            Debug.Log("Trying to connect photon again");
            if(Sessions - 1 > 0)
                ConnectToPhoton(Sessions-1);
            else 
            {
                PhotonNetwork.Disconnect();
                while (PhotonNetwork.IsConnected)
                    yield return null;
                connectedToMaster = false;
                PhotonNetwork.OfflineMode = true;
                ConnectingGM.SetActive(false);
            }
        }
    }
 

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        connectedToMaster = true;
        Buttons.SetActive(true);
       // JoinAFriendButton.SetActive(true);
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
       // Debug.Log("i am in");
        FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().OpenWindow();
        FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "...קחשמ רצוי";
        PlayWithFriendMode = true;
        PhotonRoom.room.SelecetEnviorment(0);
        PhotonRoom.room.SelecetRandomSubject();
        CreateRoom();
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        PhotonNetwork.CurrentRoom.IsVisible = false;
        FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "ינשה ןקחשל ריבעהל שי " + PhotonNetwork.CurrentRoom.Name + " :םכלש קחשמה רפסמ";
    }

    public void OnPlayWithFriendSlaveClick()
    {
        if (roomNumCode.text != "") // check the input box is not empty
        {
            PhotonRoom.room.SelecetEnviorment(0);
            FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[4].GetComponent<ModalWindowManager>().CloseWindow();
            StartCoroutine(FindObjectOfType<PhotonRoom>().SafetyFromPlayWithFriendSlave(25f));
            FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().OpenWindow();
            FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = roomNumCode.text + " קחשמל ףרטצמ";
            PhotonNetwork.JoinRoom(roomNumCode.text);
        }
    }
    
    public void OnRandomBattleButtonClicked()
    {
        StartCoroutine(OnRandomeBattleButtonClickedHelper());
    }

    IEnumerator OnRandomeBattleButtonClickedHelper() 
    {
        yield return new WaitUntil(() => PhotonRoom.room.Enviorment > -1);
                                           //this is checking fps in the moment clicking on game - if its lower than 18 user will play offline.
                                           // float CurrentFps = FindObjectOfType<FPSCounter>().m_CurrentFps;
                                           //Debug.Log("FPS is: " + CurrentFps);
         
        if (QualityControlManager.instance.GetFPS() < 15f)
        {
            print("FPS is Low.Playing in offline mode");
            PhotonNetwork.Disconnect();
            while (PhotonNetwork.IsConnected)
                yield return null;
            connectedToMaster = false;
            PhotonNetwork.OfflineMode = true;
        }
         
        FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().OpenWindow();
        FindObjectOfType<RGNotificationsManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "...דדומתמ שפחמ";
        /*
        if(PhotonRoom.room.CurrentQuestionSubject == QuestionSubject.Purim || PhotonRoom.room.CurrentQuestionSubject == QuestionSubject.Shabat || PhotonRoom.room.CurrentQuestionSubject == QuestionSubject.Random)
        {
            PhotonNetwork.JoinRandomRoom(new Hashtable {{"env", (byte)PhotonRoom.room.CurrentQuestionSubject.GetHashCode()}},2);
        }
        else
            PhotonNetwork.JoinRandomRoom(new Hashtable{{"cla",(byte)1}},2);
        */
        PhotonNetwork.JoinRandomRoom(new Hashtable { { "env", (byte)PhotonRoom.room.Enviorment.GetHashCode() } }, 2);
        StartCoroutine(PhotonRoom.room.SafetyFromRandomButtonClick(25f));
       
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. there must be no open games available");
        CreateRoom();
    }

    /*

    void CreateRoom()
    {
        Debug.Log("trying to create a new room");
        int randomRoomName = Random.Range(1000, 10000);
        Debug.Log("your code is:" + randomRoomName);
         
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        if (PhotonRoom.room.CurrentQuestionSubject == QuestionSubject.Purim || PhotonRoom.room.CurrentQuestionSubject == QuestionSubject.Shabat || PhotonRoom.room.CurrentQuestionSubject == QuestionSubject.Random)
        {
            string[] s = { "env" };
            roomOps.CustomRoomPropertiesForLobby = s;
            roomOps.CustomRoomProperties = new Hashtable { { "env", (byte)PhotonRoom.room.CurrentQuestionSubject.GetHashCode() } };
        }
        else 
        {
            string[] s = { "cla"};
            roomOps.CustomRoomPropertiesForLobby = s;
            roomOps.CustomRoomProperties = new Hashtable { { "cla", (byte)1 } } ;
        }    
      
        PhotonNetwork.CreateRoom(randomRoomName.ToString(),roomOps,TypedLobby.Default);
    }
*/

    void CreateRoom()
    {
        Debug.Log("trying to create a new room");
        int randomRoomName = Random.Range(1000, 10000);
        Debug.Log("your code is:" + randomRoomName);

        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };

        string[] s = { "env" };
        roomOps.CustomRoomPropertiesForLobby = s;
        roomOps.CustomRoomProperties = new Hashtable { { "env", (byte)PhotonRoom.room.Enviorment.GetHashCode() } };
        /*
                if (PhotonRoom.room.CurrentQuestionSubject == QuestionSubject.Purim || PhotonRoom.room.CurrentQuestionSubject == QuestionSubject.Shabat || PhotonRoom.room.CurrentQuestionSubject == QuestionSubject.Random)
                {
                    string[] s = { "env" };
                    roomOps.CustomRoomPropertiesForLobby = s;
                    roomOps.CustomRoomProperties = new Hashtable { { "env", (byte)PhotonRoom.room.CurrentQuestionSubject.GetHashCode() } };
                }

                else
                {
                    string[] s = { "cla" };
                    roomOps.CustomRoomPropertiesForLobby = s;
                    roomOps.CustomRoomProperties = new Hashtable { { "cla", (byte)1 } };
                }
        */
         
        PhotonNetwork.CreateRoom(randomRoomName.ToString(), roomOps, TypedLobby.Default);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        if (PlayerPrefs.GetString("LastRoomName").Equals(""))// if its not another game between 2 players
            CreateRoom();
    }

    public void OnCanelButtonClicked(int AutoConnect) // 1 to auto start searchiing for random game
    {
        //Debug.Log("cancel button was clicked");
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
