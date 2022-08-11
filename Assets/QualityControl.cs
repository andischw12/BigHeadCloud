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
    [SerializeField] MeshRenderer[] AllMeshes;
    [SerializeField] SkinnedMeshRenderer[] AllSkinedMeshes;
    [SerializeField] Shader LowShader;
    [SerializeField] Shader HighShader;
     [SerializeField] static bool LowQuality;


#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern float getDefaultDPR();

        [DllImport("__Internal")]
        private static extern void _setDPR(float float1);
#endif

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckAndSetFPS(5, 0.01f, 25f));
        SetQuality();

        /*
        AllMeshes = FindObjectsOfType<MeshRenderer>();
        AllSkinedMeshes = FindObjectsOfType<SkinnedMeshRenderer>();
       // if (GameManager.instance.currentLevel.levelIndex == 0)
        {
            //BlackScreen.SetActive(true);
            StartCoroutine(CheckAndSetFPS(5, 0.01f, 25f));
        }
        else
            SetQuality();
        */

    }

    // Update is called once per frame

    public void __setDPR(float float1)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
                _setDPR(float1);
#endif
    }


    void SetQuality()
    {
        if (LowQuality)
        {
            SetBadQuality();
            print("Bad Quality Chosen");
        }

        else
        {
            SetGoodQuality();
            print("Good Quality Chosen");
        }


    }
    IEnumerator CheckAndSetFPS(int timestoCheck, float currency, float minFrameRate)
    {
        float sum = 0;
        // yield return new WaitForSeconds(2);
        for (int i = 0; i < timestoCheck; i++)
        {
            sum += 1.0f / Time.deltaTime;
            print("WebGlInstance.fps is :" + Time.deltaTime);
            yield return new WaitForSecondsRealtime(0.3f);
        }
        float AvarageResult = sum / timestoCheck;
        if (AvarageResult < minFrameRate)
            LowQuality = true;// bas quality
        print("Avarage fps is: " + AvarageResult);
        // SetQuality();
        //BlackScreen.SetActive(false);
        //if (!LowQuality)
            //FindObjectOfType<MenuManager>().HeavyParticales.SetActive(true);


    }

    public void SetGoodQuality()
    {
        if (Camera.main.GetComponent<BeautifyEffect.Beautify>() != null)
            Camera.main.GetComponent<BeautifyEffect.Beautify>().enabled = true;
        SetSceneFog(true);

        ChangeShader(HighShader);
        QualitySettings.SetQualityLevel(QualityLevel.Fantastic.GetHashCode());
        __setDPR(1f);
        Camera.main.renderingPath = RenderingPath.Forward;
        print("Quality setting: " + QualitySettings.GetQualityLevel());
    }

    public void SetMedumQuality()
    {
        if (Camera.main.GetComponent<BeautifyEffect.Beautify>() != null)
            Camera.main.GetComponent<BeautifyEffect.Beautify>().enabled = true;
        SetSceneFog(true);
        ChangeShader(LowShader);
        Camera.main.renderingPath = RenderingPath.DeferredLighting;
        QualitySettings.SetQualityLevel(QualityLevel.Good.GetHashCode());
        __setDPR(0.9f);
        print("Quality setting: " + QualitySettings.GetQualityLevel());


    }

    public void SetBadQuality()
    {
        if (Camera.main.GetComponent<BeautifyEffect.Beautify>() != null)
            Camera.main.GetComponent<BeautifyEffect.Beautify>().enabled = false;
        SetSceneFog(false);
        ChangeShader(LowShader);
        Camera.main.renderingPath = RenderingPath.DeferredLighting;
        QualitySettings.SetQualityLevel(1);
        __setDPR(0.8f);
        print("Quality setting: " + QualitySettings.GetQualityLevel());


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

    void SetSceneFog(bool Mode)
    {
        if (Mode)
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Exponential;
            RenderSettings.fogDensity = 0.001f;

            RenderSettings.fogColor = new Color32(77, 176, 170, 175);
        }
        else
        {
            RenderSettings.fog = false;
        }


    }
}
