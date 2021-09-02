using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Effects : MonoBehaviour
{
    public static UI_Effects instance;
    [SerializeField] Slider player1Slider;
    [SerializeField] Slider player2Slider;
    [SerializeField] StarsEffect _starEffectP1;
    [SerializeField] StarsEffect _starEffectP2;
    [SerializeField] GameObject player1TimerMidPic;
    [SerializeField] GameObject player2TimerMidPic;
    [SerializeField] Image[] Answers;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }




    private void Start()
    {

        ResetSliders();
        UnFreezeTimer(1);
        UnFreezeTimer(2);
        StopJockerEffect();
        StopTimeIsAlmostOver(1);
        StopTimeIsAlmostOver(2);

    }
    public void ResetSliders()
    {
        player1Slider.value = 0f;
        player2Slider.value = 0f;
    }

    public void SetSliderVal(int side, float newVal)
    {
        if (side == 1)
            StartCoroutine(AnimateSliderOverTime(player1Slider, newVal, 1.5f));

        else
            StartCoroutine(AnimateSliderOverTime(player2Slider, newVal, 1.5f));
    }


    public void textEffect(TextMeshProUGUI tmpro, string input, float time) 
    {
        StartCoroutine(textEffectHelper(tmpro, input, time));
    }

    IEnumerator textEffectHelper(TextMeshProUGUI tmpro, string input, float time) 
    {
        string tmp;
        tmp = tmpro.GetParsedText();
        tmpro.SetText(input);
        yield return new WaitForSecondsRealtime(time);

        tmpro.SetText(tmp + " ");
        


    }

    IEnumerator AnimateSliderOverTime(Slider _slider, float newval, float seconds)
    {

        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            _slider.value = Mathf.Lerp(_slider.value, newval, lerpValue);
            yield return null;
        }
    }

    public void PlayStartEffect(int player)
    {
       
        if (player == 1)
            _starEffectP1.Play();
        else
            _starEffectP2.Play();
    }

    public void FreezeTimer(int side)
    {
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.Freeze);
        if (side == 1)
        {
            player1TimerMidPic.GetComponent<Image>().material.SetFloat("_FrozenFade", 1);
            StopTimeIsAlmostOver(1);
        }

        else 
        {
            player2TimerMidPic.GetComponent<Image>().material.SetFloat("_FrozenFade", 1);
            StopTimeIsAlmostOver(2);
        }
            
    }

    public void UnFreezeTimer(int side)
    {
         
        if (side == 1)
        {
            player1TimerMidPic.GetComponent<Image>().material.SetFloat("_FrozenFade", 0);
        }

        else
            player2TimerMidPic.GetComponent<Image>().material.SetFloat("_FrozenFade", 0);
    }

    public void StartJockerEffect()
    {
        foreach (Image i in Answers)
        {
            i.material.SetFloat("_AddHueFade", 1);
        }

    }

    public void MakeAllAnsersGray()
    {
        foreach (Image i in Answers)
        {
            i.material.SetFloat("_RecolorFade", 1);
        }

    }

    public void MakeAllAnsersNotGray()
    {
        foreach (Image i in Answers)
        {
            i.material.SetFloat("_RecolorFade", 0);
        }

    }

    public void StopJockerEffect()
    {
        foreach (Image i in Answers)
        {
            i.material.SetFloat("_AddHueFade", 0);
        }
    }

    public void StartCorrectAnswerEffect(int answer)
    {
        Answers[answer - 1].material.SetFloat("_AddHueFade", 0.6f);
    }

    public void StopCorrectAnswerEffect(int answer)
    {
        Answers[answer - 1].material.SetFloat("_AddHueFade", 0);
    }

    public void StartTimeIsAlmostOver(int side)
    {
        if (side == 1)
        {
            player1TimerMidPic.GetComponent<Image>().material.SetFloat("_SineGlowFade", 1f);
            player1TimerMidPic.GetComponent<Image>().material.SetColor("_SineGlowColor", Color.red);
            player1TimerMidPic.GetComponent<Image>().material.SetFloat("_SineGlowFrequency", 25f);
        }

        else
        {
            player2TimerMidPic.GetComponent<Image>().material.SetFloat("_SineGlowFade", 1f);
            player2TimerMidPic.GetComponent<Image>().material.SetColor("_SineGlowColor", Color.red);
            player2TimerMidPic.GetComponent<Image>().material.SetFloat("_SineGlowFrequency", 25f);
        }



    }


    public void StopTimeIsAlmostOver(int side)
    {
        if (side == 1)
        {
            player1TimerMidPic.GetComponent<Image>().material.SetFloat("_SineGlowFade", 0.1f);
            player1TimerMidPic.GetComponent<Image>().material.SetColor("_SineGlowColor", Color.white);
            player1TimerMidPic.GetComponent<Image>().material.SetFloat("_SineGlowFrequency", 4f);
        }

        else
        {
            player2TimerMidPic.GetComponent<Image>().material.SetFloat("_SineGlowFade", 0.1f);
            player2TimerMidPic.GetComponent<Image>().material.SetColor("_SineGlowColor", Color.white);
            player2TimerMidPic.GetComponent<Image>().material.SetFloat("_SineGlowFrequency", 4f);
        }



    }
}
