using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEffectsList {TimerBeep,FireWorks, Bell,CorrectAnswer,TimerTicking,SignateIn,SmallFaceOnAnswer
        ,Freeze,Jocker,reavelAnswer,WinGame,Click,NewMassage,Clapping,Buzzer,fiftyfifty,CalculationEnd,Calculation,GameOver,Gong};

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] AudioClip[] soundEffects;
    [SerializeField] AudioClip[] music;
    [SerializeField] AudioClip[] MenuMusic;
    [SerializeField] AudioSource effctsAC;
    [SerializeField] AudioSource musicAC;
    [SerializeField] AudioSource timerAC;
     
    public void Awake() 
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    public void PlaySoundEffect(AudioClip p) 
    {
        effctsAC.PlayOneShot(p);
    }

    public void PlaySoundEffect(SoundEffectsList sound) 
    {
        effctsAC.PlayOneShot(soundEffects[sound.GetHashCode()]);
    }
    public void PlaySoundEffect(SoundEffectsList sound,float frequency,int counter)
    {
        StartCoroutine(PlaySoundEffectHelper(sound, frequency, counter));
    }
    IEnumerator PlaySoundEffectHelper(SoundEffectsList sound,float frequency,int counter)
    {
        while (counter > 0 && GameManager.instance.thisComputerPlayer.CurrentAnswer==0 && !GameManager.instance.thisComputerPlayer.playeTimer.timerPaused)// path to fix sound when pressed 
        {
            effctsAC.PlayOneShot(soundEffects[sound.GetHashCode()]);
            counter--;
            yield return new WaitForSecondsRealtime(frequency);
        }
    }
    public void PlayMusic(int num) 
    {
        
        musicAC.Stop();
        musicAC.clip = music[num];
        musicAC.Play();
    }

    public void PlayMenuMusic()
    {
        musicAC.Stop();
        musicAC.clip = MenuMusic[0];
        musicAC.Play();
    }
    public void StartTimerSound()
    {
        timerAC.clip = soundEffects[SoundEffectsList.TimerTicking.GetHashCode()];
        timerAC.Play();
    }

    public void StopTimerSound()
    {
        timerAC.Stop();
    }


    public void StopSoundEffect()
    {
       effctsAC.Stop();
    }
}
