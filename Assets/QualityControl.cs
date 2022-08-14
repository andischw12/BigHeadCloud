using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Rendering;


public class QualityControl : MonoBehaviour
{
    public static QualityControl instance;
    [SerializeField] MeshRenderer[] AllMeshes;
    [SerializeField] SkinnedMeshRenderer[] AllSkinedMeshes;
    [SerializeField] Shader LowShader;
    [SerializeField] Shader HighShader;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) 
        {
            GoodQuality();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            BadQuality();
        }
    }


    


    void GoodQuality() 
    {
        QualitySettings.SetQualityLevel(QualityLevel.Fantastic.GetHashCode());

    }

    void BadQuality()
    {
        QualitySettings.SetQualityLevel(QualityLevel.Simple.GetHashCode());

    }

    void SetMeshArr()
    {
        AllMeshes = FindObjectsOfType<MeshRenderer>();
        AllSkinedMeshes = FindObjectsOfType<SkinnedMeshRenderer>();
    }

    void ChangeShader(Shader s)
    {
        foreach (MeshRenderer m in AllMeshes)
        {

            if (m.sharedMaterial.shader == HighShader || m.sharedMaterial.shader == LowShader)
                m.sharedMaterial.shader = Shader.Find(s.name);
        }

        foreach (SkinnedMeshRenderer m in AllSkinedMeshes)
        {
            if (m.sharedMaterial.shader == HighShader || m.sharedMaterial.shader == LowShader)
                m.sharedMaterial.shader = Shader.Find(s.name);
        }
    }
}
