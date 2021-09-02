using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostCharachter : MonoBehaviour
{
    public static HostCharachter instance;
    public bool IsTalkingInProgress = false;
    AudioSource Narration;
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.transform.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Narration = GetComponent<AudioSource>();
    }

    public void dancing(float danceTime)
    {
        StartCoroutine(DancingAnimaiton(danceTime));
    }

    public void talking(AudioClip[] CurrentNarration) 
    {
        StartCoroutine(talkingAnimation(CurrentNarration));
    }


    public void TalkingOnly(AudioClip[] CurrentNarration)
    {
        if (GameProcess.instance.currentQuestionNumber - 1 < CurrentNarration.Length && (CurrentNarration == GetComponent<NarrationHolder>().questionProcess || CurrentNarration == GetComponent<NarrationHolder>().Result))
            Narration.clip = CurrentNarration[GameProcess.instance.currentQuestionNumber - 1];
        else
            Narration.clip = CurrentNarration[Random.Range(0, CurrentNarration.Length)];
        Narration.Play();
    }


    private IEnumerator DancingAnimaiton(float dancingTime)
    {

        GetComponentInChildren<Animator>().SetInteger("Random", Random.Range(0, 3));
        GetComponentInChildren<Animator>().SetBool("Dancing", true);
        yield return new WaitForSecondsRealtime(dancingTime);

        GetComponentInChildren<Animator>().SetBool("Dancing", false);
    }





    private IEnumerator talkingAnimation(AudioClip[] CurrentNarration)
    {
        IsTalkingInProgress = true;
        Cameras.instance.SelectCamera(Cameras.instance.CameraArray.Length-1);
        GameManager.instance.thisComputerPlayer.myEmojiesGuiButton.MakeButtonNotAvaliable();
        if (GameProcess.instance.currentQuestionNumber - 1 < CurrentNarration.Length && (CurrentNarration == GetComponent<NarrationHolder>().questionProcess || CurrentNarration == GetComponent<NarrationHolder>().Result))
            Narration.clip = CurrentNarration[GameProcess.instance.currentQuestionNumber - 1];
        else
            Narration.clip = CurrentNarration[Random.Range(0, CurrentNarration.Length)];
        Narration.Play();
        GetComponentInChildren<Animator>().SetBool("Talking", true);
        yield return new WaitForSecondsRealtime(Mathf.Max(Narration.clip.length));
        GetComponentInChildren<Animator>().SetBool("Talking", false);
        IsTalkingInProgress = false;
        GameManager.instance.thisComputerPlayer.myEmojiesGuiButton.MakeButtonAvaliable();
    }

}
