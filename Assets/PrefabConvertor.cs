using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabConvertor : MonoBehaviour
{
    [SerializeField] GameObject OriginalPrefab;
    [SerializeField] GameObject TargetPrefab;
    [SerializeField] SkinnedMeshRenderer[] OriginalSkinRenderer;
    [SerializeField] SkinnedMeshRenderer[] TargetSkinRenderer;


    // Start is called before the first frame update
    void Start()
    {
        OriginalSkinRenderer = OriginalPrefab.GetComponentsInChildren<SkinnedMeshRenderer>();
        TargetSkinRenderer = TargetPrefab.GetComponentsInChildren<SkinnedMeshRenderer>();

        for(int i =0;i<TargetSkinRenderer.Length;i++) 
        {
            TargetSkinRenderer[i].materials = OriginalSkinRenderer[i].materials;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
