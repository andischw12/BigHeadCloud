using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StarsEffect : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] PS_Stars;
    public void Play()
    {
        StartCoroutine(PlayStarsEffet());
    }
 
    IEnumerator PlayStarsEffet() 
    {
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.CorrectAnswer);
        foreach (ParticleSystem p in PS_Stars)
        {
            SoundManager.instance.PlaySoundEffect(SoundEffectsList.FireWorks);
            
            p.Play();
            yield return new WaitForSecondsRealtime(0.3f);
        }
            
         
    }

    
}
