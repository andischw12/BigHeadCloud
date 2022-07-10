using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{

    [SerializeField] int prefabNum;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<KidAvatarSelector>().PreperePrefabArr();
        FindObjectOfType<KidAvatarSelector>().SelectAvatarByPrefab(prefabNum);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) 
        {
            FindObjectOfType<KidAvatarSelector>().SetAvatarAccessoryItem(AvatarArrayEnum.Hats, FindObjectOfType<KidAvatarSelector>().GetActiveAvatarInfo()[AvatarArrayEnum.Hats.GetHashCode()+1]);
        }
    }
}
