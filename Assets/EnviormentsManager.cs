using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviormentsManager : MonoBehaviour
{
    [SerializeField] GameObject[] EnviormentsArr;
    public static EnviormentsManager instance;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this.gameObject); else instance = this; // singelton
    }

    // Start is called before the first frame update
    public void ChooseEnviorment(int env) 
    {
        for(int i = 0; i < EnviormentsArr.Length; i++) 
        {
            if (i == env)
                EnviormentsArr[i].SetActive(true);
            else
                EnviormentsArr[i].SetActive(false);
        }
    }
}
