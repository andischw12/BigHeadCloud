using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using OmniSARTechnologies.Helper;
using OmniSARTechnologies.LiteFPSCounter;
using BeautifyEffect;

public enum QualityOptionsRG2 {BadQuality,GoodQuality }
public class QualityControlManager : MonoBehaviour
{
    public static QualityControlManager instance;
    [SerializeField] MeshRenderer[] AllMeshes;
    [SerializeField] SkinnedMeshRenderer[] AllSkinedMeshes;
    [SerializeField] QualityDepended[] AllQualityDependedARR;
    [SerializeField] Shader LowShader;
    [SerializeField] Shader HighShader;
    [SerializeField] public QualityOptionsRG2 currentQuality;


#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern float getDefaultDPR();

        [DllImport("__Internal")]
        private static extern void _setDPR(float float1);
#endif


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
    

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        if (scene.buildIndex > 0) 
        {
            SetQuality();
        }
        else
        StartCoroutine(CheckAndSetFPS(5, 25f));
    }

    IEnumerator CheckAndSetFPS(int timestoCheck, float minFrameRate)
    {
        
       // currentQuality = QualityOptionsRG2.BadQuality;
       // SetQuality();
        yield return new WaitUntil (()=> GetComponentInChildren<LiteFPSCounter>().frameRate>5);
        float sum = 0;
        // yield return new WaitForSeconds(2);
        for (int i = 0; i < timestoCheck; i++)
        {
            sum+=GetComponentInChildren<LiteFPSCounter>().frameRate;
            print("WebGlInstance.fps is :" + GetComponentInChildren<LiteFPSCounter>().frameRate);
            yield return new WaitForSecondsRealtime(0.3f);
        }
        float AvarageResult = sum / timestoCheck;
        print("avarage fps is: " + AvarageResult);
        if (AvarageResult < minFrameRate)
            currentQuality= QualityOptionsRG2.BadQuality;
        else
            currentQuality = QualityOptionsRG2.GoodQuality;
        SetQuality();


    }

    void SetQuality() 
    {
        if (currentQuality == QualityOptionsRG2.GoodQuality)
            GoodQuality();
        else
            LowQuality();
    }
    


    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            GoodQuality();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            LowQuality();
        }
        */
    }

    public void __setDPR(float float1)
    {
        
            #if UNITY_WEBGL && !UNITY_EDITOR
                            _setDPR(float1);
            #endif
        
    }



    void GoodQuality()
    {
        //SetMeshArr();
        // ChangeShader(HighShader);
        SetQualityDependedArr();
        QualitySettings.SetQualityLevel((int)QualityLevel.Fantastic);
        
        SetAllQualityDepended();
        __setDPR(1f);
        Camera.main.GetComponent<Beautify>().enabled = true;

    }

    void LowQuality()
    {
        //SetMeshArr();
        //ChangeShader(LowShader);
        SetQualityDependedArr();
        QualitySettings.SetQualityLevel((int)QualityLevel.Fastest);
         
        SetAllQualityDepended();
        __setDPR(0.7f);
        Camera.main.GetComponent<Beautify>().enabled = false;



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
