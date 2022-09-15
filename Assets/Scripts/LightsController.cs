using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsController : MonoBehaviour
{
    // Start is called before the first frame update
    public static LightsController instance;
    [SerializeField] GameObject[] SpotLights = new GameObject[3];
    [SerializeField] GameObject MainLight;
    float _tempMainLight;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _tempMainLight = MainLight.GetComponent<Light>().intensity;
       // LightsOff();
        

    }

    public void LightsOn() 
    {
        foreach(GameObject Gm in SpotLights) 
        {
          //  Gm.SetActive(true);
        }
       // MainLight.GetComponent<Animator>().enabled = true;
         


    }

    public void LightsOff()
    {
        foreach (GameObject Gm in SpotLights)
        {
            Gm.SetActive(false);
        }
        MainLight.GetComponent<Animator>().enabled = false;
        MainLight.GetComponent<Light>().intensity = _tempMainLight;
        

    }

     



}
