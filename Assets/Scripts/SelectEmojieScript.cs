using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectEmojieScript : MonoBehaviour
{
    PhotonPlayerManager myPhotonPlayer;
    // Start is called before the first frame update
    public void selectEmojie()
    {
        GetComponentInParent<EmoteGuiButton>().EmojieClicked(GetComponent<Image>().sprite.name);
    }
}