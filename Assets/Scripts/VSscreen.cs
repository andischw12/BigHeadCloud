using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Febucci.UI;

public class VSscreen : MonoBehaviour
{
     
    public void PlaySwosh() 
    {
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.SignateIn);
    }

    public void PlayBell()
    {
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.Bell);
    }
}
