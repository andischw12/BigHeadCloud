using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDressTest : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Avater_ClothesAndSkeenMaker MyTT;
    void Start()
    {
       // MyTT.SetSpecificChest(1, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        MyTT.SetSpecificChest(1, 1);
    }

}
