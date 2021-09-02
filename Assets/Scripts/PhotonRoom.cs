using Photon.Pun;
using Photon.Realtime;
using System.IO;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Michsky.UI.ModernUIPack;
using UnityEngine.UI;
public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    // Room info
    public static PhotonRoom room;
    private PhotonView PV;
    public int curentScene;
    public int multiPlayerScene;
    bool WaitingTimeIsOver;
    bool _isSinglePlayer = false;
    public bool flag;
    public bool CancelButtonClicked { get; set;}
    public bool IsSinglePlayer{get{ return _isSinglePlayer;}}

    private void Awake()
    {
        if (PhotonRoom.room == null)
            PhotonRoom.room = this;
        else
        {
            Destroy(PhotonRoom.room.gameObject);
            //PhotonRoom.room = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishLoading; // need to learn about events i think
    }

    public override void OnDisable()
    {
        base.OnDisable();
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishLoading; // need to learn about events i think
    }

    private void OnSceneFinishLoading(Scene scene, LoadSceneMode mode)
    {
        curentScene = scene.buildIndex;
        if (curentScene == multiPlayerScene)
            createPlayer();
    }

    private void createPlayer()
    {
        //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "GameManager"), transform.position, Quaternion.identity, 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MultiPlayerQuestionRandomizer"), transform.position, Quaternion.identity, 0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
        if(IsSinglePlayer)
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonBotPlayer"), transform.position, Quaternion.identity, 0);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We are in a room");
        if (!PhotonNetwork.IsMasterClient)
            return;

        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("LoadingLevel");
        StartCoroutine(LoadScene());
        //StartCoroutine(Safety(17f));


    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if(!GameManager.instance.IsGameOver)
            GameManager.instance.PlayerLeftInTheMiddle();
        Debug.Log(otherPlayer.NickName + "has left the room. you win!");
    }

    IEnumerator LoadScene() 
    {
        if (PhotonNetwork.OfflineMode)
        {
            if(!flag)
                yield return new WaitForSecondsRealtime(10f);
            _isSinglePlayer = true;
            print("Offline mode from PhotonRoom");
            flag = false;
            PhotonNetwork.LoadLevel(multiPlayerScene);
        }
        else 
        {
            WaitingTimeIsOver = false;
            StartCoroutine(Waiting(10));
            while (PhotonNetwork.CurrentRoom.PlayerCount !=2 && !WaitingTimeIsOver)
                yield return null;
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.LoadLevel(multiPlayerScene);
                print("Online mode from PhotonRoom");
            }
            else 
            {
                _isSinglePlayer = true;
                if (PhotonNetwork.InRoom)
                    PhotonNetwork.LeaveRoom();
                while (PhotonNetwork.InRoom)
                    yield return null;
                PhotonNetwork.Disconnect();
                while (PhotonNetwork.IsConnected)
                    yield return null;
                PhotonNetwork.OfflineMode = true;
                flag = true;
                while (!PhotonNetwork.OfflineMode)
                    yield return null;
                PhotonNetwork.JoinRandomRoom();
                print("Offline mode from PhotonRoom after Being Online From Lobby");
            }
        }
    }

    IEnumerator Waiting(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        WaitingTimeIsOver = true;
    }

    public IEnumerator Safety(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
            FindObjectOfType<NotificationsWindowManager>().CurrentSceneNotifications[3].GetComponent<ModalWindowManager>().windowDescription.text = "תוינש המכ דועב בוש הסנמ .תרשה לע בר סמוע";
        }
        yield return new WaitForSecondsRealtime(4f);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
         
            FindObjectOfType<PhotonLobby>().OnCanelButtonClicked(1);
        }
    }


}




 
