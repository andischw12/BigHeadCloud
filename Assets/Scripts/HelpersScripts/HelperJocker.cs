using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperJocker : Helper
{
   
    public override void UseHelper()
    {
        if (CanIUseHelper())
        {
            base.UseHelper();
            StartCoroutine(UseJocker());
            
        }

    }

    IEnumerator UseJocker() 
    {
        //helperGameObject.GetComponentInParent<HelperManager>().myPlayerAvatar.playeTimer.PauseTimer();
        GameManager.instance.thisComputerPlayer.myPhotonPlayer.StopMyTimer(false);
        UI_Effects.instance.StartJockerEffect();
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.Jocker);
        yield return new WaitForSecondsRealtime(1f);
        UI_Effects.instance.StopJockerEffect();
        GetComponentInParent<HelperManager>().myPlayerAvatar.myPhotonPlayer.chooseAnswerMP(GameProcess.instance.question.CorrectAnswer);
        
    }
}
