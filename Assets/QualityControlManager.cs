using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Rendering;

public enum QualityOptionsRG2 {BadQuality,GoodQuality }
public class QualityControlManager : MonoBehaviour
{
    public static QualityControlManager instance;
    [SerializeField] MeshRenderer[] AllMeshes;
    [SerializeField] SkinnedMeshRenderer[] AllSkinedMeshes;
    [SerializeField] QualityDepended[] AllQualityDependedARR;
    [SerializeField] Shader LowShader;
    [SerializeField] Shader HighShader;
    [SerializeField] QualityOptionsRG2 currentQuality;
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
            LowQuality();
        }
    }





    void GoodQuality()
    {
        //SetMeshArr();
        // ChangeShader(HighShader);
        SetQualityDependedArr();
        QualitySettings.SetQualityLevel((int)QualityLevel.Fantastic);
        currentQuality =QualityOptionsRG2.GoodQuality;
        SetAllQualityDepended();

    }

    void LowQuality()
    {
        //SetMeshArr();
        //ChangeShader(LowShader);
        SetQualityDependedArr();
        QualitySettings.SetQualityLevel((int)QualityLevel.Fastest);
        currentQuality = QualityOptionsRG2.BadQuality;
        SetAllQualityDepended();


    }

    void SetMeshArr()
    {
        AllMeshes = FindObjectsOfType<MeshRenderer>(true);
        AllSkinedMeshes = FindObjectsOfType<SkinnedMeshRenderer>(true);
    }

    void SetQualityDependedArr() 
    {
        AllQualityDependedARR = FindObjectsOfType<QualityDepended>(true);
    }



    void SetAllQualityDepended() 
    {
        foreach (QualityDepended Q in AllQualityDependedARR)
        {
            if (Q.ChangeMat) 
            {
                ChangeShaders(Q.gameObject);
            }

        }


        if (currentQuality == QualityOptionsRG2.BadQuality)
        {
            foreach (QualityDepended Q in AllQualityDependedARR)
            {
                if (Q.MinimumQualityNeeded == QualityOptionsRG2.GoodQuality) 
                {
                    Q.gameObject.SetActive(false);
                }

                else 
                {
                    Q.gameObject.SetActive(true);
                }
                   
            }
        }
        else
        {
            foreach (QualityDepended Q in AllQualityDependedARR)
            {
                Q.gameObject.SetActive(true);
                 
            }
        }
        
    }

    void ChangeShaders(GameObject parentGM)
    {
       MeshRenderer[] AllMeshes = parentGM.GetComponentsInChildren<MeshRenderer>(true);
          
        
        foreach (MeshRenderer m in AllMeshes)
        {

            //if (m.sharedMaterial.shader == HighShader || m.sharedMaterial.shader == LowShader)
            foreach(Material mat in m.materials) 
            {
                if(mat.shader == LowShader || mat.shader == HighShader) 
                {
                    if(currentQuality == QualityOptionsRG2.BadQuality) 
                    {
                        mat.shader = Shader.Find(LowShader.name);
                    }

                    
                    else 
                    {
                        mat.shader = Shader.Find(HighShader.name);
                    }
                }
            }
        }
        /*
        foreach (SkinnedMeshRenderer m in AllSkinedMeshes)
        {
            // if (m.sharedMaterial.shader == HighShader || m.sharedMaterial.shader == LowShader)
            m.sharedMaterial.shader = Shader.Find(s.name);
        }
        */
    }
}
