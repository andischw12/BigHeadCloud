using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurimHatScript : MonoBehaviour
{
    private void Start()
    {
        if (GameManager.instance.enviorment!= EnviormentList.Purim) 
        {
            Destroy(this.gameObject);
        }
    }
}
