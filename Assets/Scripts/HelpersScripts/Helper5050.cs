using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper5050 : Helper
{
    // Start is called before the first frame update
    public override void UseHelper() 
    {
        if (CanIUseHelper()) 
        {
            base.UseHelper();
            SoundManager.instance.PlaySoundEffect(SoundEffectsList.fiftyfifty);
            UIManager.instance.Lifeline5050();
        }

    }
}
