using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Cameras : MonoBehaviour
{
    public static Cameras instance;
    [SerializeField] CinemachineVirtualCamera[] _cameraArray;     // Cameras: [0]longshot,[1] playerA,[2]Screen,[3]PlayerB,[4]TopShot,[5]ArieCloseup (in multiplayer demo girl == screen)
    [SerializeField] CinemachineVirtualCamera _activeCamera;
    [SerializeField] GameObject _splitScreenCam;
    bool RandomCamBool = false;
    public CinemachineVirtualCamera ActiveCamera { get { return _activeCamera; } }
    public CinemachineVirtualCamera[] CameraArray { get { return _cameraArray; } set { _cameraArray = value; } }




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
       
    }

    private void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        CameraArray = GetComponentsInChildren<CinemachineVirtualCamera>();
    }

    public void SelectRandomCamera(float time)
    {

        RandomCamBool = true;
        // if(QuizManager.instance.IsPicQeustion())
        // StartCoroutine(RandomCameraHelperPic(time));
        // else
        StartCoroutine(RandomCameraHelper(time));
    }

    public void SelectCamera(int Camera)
    {

        RandomCamBool = false;
        SwitchCamera(Camera);
    }

    public void SelectCamera(Player player)
    {

        RandomCamBool = false;
        if (player == GameManager.instance.player1)
            SwitchCamera(1);
        else
            SwitchCamera(3);
    }



    private void SwitchCamera(int SwitchCamera)
    {
        CameraArray[SwitchCamera].enabled = true;
        _activeCamera = CameraArray[SwitchCamera];
        foreach (CinemachineVirtualCamera C in CameraArray)
        {
            if (C != CameraArray[SwitchCamera])
            {
                C.enabled = false;
            }
        }
    }

    IEnumerator RandomCameraHelper(float time)
    {
        for (int i = 0; i < CameraArray.Length && RandomCamBool; i++)
        {
            if (i == 2)// skip screen camera
                i = 3;
            SwitchCamera(i);
            yield return new WaitForSecondsRealtime(time);
            if (i == CameraArray.Length - 1)// reset i for loop
                i = 0;
        }
    }

    IEnumerator RandomCameraHelperPic(float time)
    {
        for (int i = 2; i < CameraArray.Length && RandomCamBool; i++)
        {
            SwitchCamera(i);
            if (i == 2)
                yield return new WaitForSecondsRealtime(time);
            yield return new WaitForSecondsRealtime(time);
            if (i == CameraArray.Length - 1)// reset i for loop
                i = 0;
        }
    }

}
