using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFreeze : Helper
{
    public override void UseHelper()
    {
        if (CanIUseHelper())
        {
            base.UseHelper();
            GameManager.instance.thisComputerPlayer.myPhotonPlayer.StopMyTimer(true);
            //freez timer
        }

    }

 
}
