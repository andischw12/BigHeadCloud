using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MusicOnOff : MonoBehaviour
{
    public Sprite[] MusicSprites;
    static bool MusicOn = true;
    private void Start()
    {
        int soundOn = 0;
        if (PlayerPrefs.GetInt("BigHeadSound") != null)
            soundOn = PlayerPrefs.GetInt("BigHeadSound");
        
        // check on start of scene
        if (MusicOn && soundOn!=0) 
        {
            TurnMusicOn();
        }
        else
        {
            TurnMusicOff();
        }
    }
    public void MusicButton()
    {
        if (MusicOn)
        {
            TurnMusicOff();
        }
        else
        {
            TurnMusicOn();
        }
    }
    void TurnMusicOn() 
    {
        if (!Application.isEditor)
        {
          //  GetJSData.soundHagim81(1);

        }
        AudioListener.volume = 1f;
        GetComponent<Image>().sprite = MusicSprites[0];
        MusicOn = true;
        PlayerPrefs.SetInt("BigHeadSound", 1);
    }
    void TurnMusicOff()
    {
        if (!Application.isEditor)
        {
           // GetJSData.soundHagim81(0);

        }
        AudioListener.volume = 0f;
        GetComponent<Image>().sprite = MusicSprites[1];
        MusicOn = false;
        PlayerPrefs.SetInt("BigHeadSound", 0);

    }
}
