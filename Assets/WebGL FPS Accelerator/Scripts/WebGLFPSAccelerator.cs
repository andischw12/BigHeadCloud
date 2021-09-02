using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine.Rendering;

namespace AG_WebGLFPSAccelerator
{
    public class WebGLFPSAccelerator : MonoBehaviour
    {

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void _getDefaultDPR();

    [DllImport("__Internal")]
    private static extern void _setDPR(float float1);
#endif

        public static WebGLFPSAccelerator instance;

        [Header("___SETTINGS____________________________________________________________________________________________________________________")]
        [Read_Only_Field]
        public float dpi = 1;
        public bool dynamicResolutionSystem;
        public bool ShowHideUI;
        public float dpiDecrease = 0.050f;
        public float dpiIncrease = 0.050f;
        public int fpsMax = 35;
        public int fpsMin = 30;
        public float dpiMin = 0.6f;
        public float dpiMax = 1f;
        public float measurePeriod = 4f;
        public float fixedDPI = 1f;
        public bool useRenderScaleURP;

        [Header("___UI ELEMENTS____________________________________________________________________________________________________________________")]
        public TMP_Text dpi_TMP_Text;
        public AG_inputManager1 AG_inputManager1_fpsMax;
        public AG_inputManager1 AG_inputManager1_fpsMin;
        public AG_inputManager1 AG_inputManager1_dpiMax;
        public AG_inputManager1 AG_inputManager1_dpiMin;
        public AG_inputManager1 AG_inputManager1_dpiDecrease;
        public AG_inputManager1 AG_inputManager1_dpiIncrease;
        public AG_inputManager1 AG_inputManager1_measurePeriod;
        public AG_inputManager2 AG_inputManager2_fixedDPI;
        public Toggle Toggle1;
        public Toggle Toggle2;
        public GameObject dpiUIElement;
        public GameObject fixedDPIUIElement;
        public GameObject webglFpsAcceleratorInGameUI;

        [HideInInspector]
        public int fps;

        [HideInInspector]
        public float dpr = 0;

        [HideInInspector]
        private int m_FpsAccumulator = 0;

        [HideInInspector]
        public float m_FpsNextPeriod = 1.5f;

        [HideInInspector]
        public float defaultDPR = 0f;

        [HideInInspector]
        public float lastDPR;

        [HideInInspector]
        public bool lastDynamicResolutionSystem;

        [HideInInspector]
        public bool lastShowHideUI;

        [HideInInspector]
        public bool urp;

        private bool waitForRenderPipelineReady;

        public void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
        }

        void Start()
        {
            requestDefaultDPR();

            Toggle1.isOn = dynamicResolutionSystem;
            Toggle2.isOn = useRenderScaleURP;
            webglFpsAcceleratorInGameUI.transform.parent.gameObject.GetComponent<Canvas>().enabled = ShowHideUI;

#if USING_URP
            waitForRenderPipelineReady = true;
            Invoke("makeWaitForRenderPipelineReadyFalse", 0);
            urp = true;
            Toggle2.transform.parent.gameObject.SetActive(true);
            RectTransform rt = webglFpsAcceleratorInGameUI.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 580);
#endif
        }

        public void __setDPR(float float1)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        _setDPR(float1);
#endif
        }

        public void getDefaultDPR(float float1)
        {
            defaultDPR = float1;
        }

        public void requestDefaultDPR()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        _getDefaultDPR();
#endif
        }

        public void Toggle1Event()
        {
            dynamicResolutionSystem = Toggle1.isOn;
        }

        public void Toggle2Event()
        {
            useRenderScaleURP = Toggle2.isOn;
        }

        public void getAverageFPS()
        {
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup >= m_FpsNextPeriod)
            {
                fps = (int)(m_FpsAccumulator / measurePeriod);
                m_FpsAccumulator = 0;
                //m_FpsNextPeriod += measurePeriod;
                m_FpsNextPeriod = Time.realtimeSinceStartup + measurePeriod;

                dynamicResolutionSystemMethod();
            }
        }

        public void dynamicResolutionSystemMethod()
        {
            if (fps > fpsMax)
            {
                dpi += dpiIncrease;
            }
            else if (fps < fpsMin)
            {
                dpi -= dpiDecrease;
            }
            dpi = Mathf.Clamp(dpi, dpiMin, dpiMax);

            if (useRenderScaleURP && urp)
            {
                dpr = dpi;

                if (dpr != lastDPR)
                {
                    var rpAsset = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset;
#if USING_URP
                            var urpAsset = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)rpAsset;
                            urpAsset.renderScale = dpr;
#endif
                }
            }
            else
            {
                dpr = dpi * defaultDPR;

                if (dpr != lastDPR)
                {
                    __setDPR(dpr);
                }
            }

            lastDPR = dpr;
        }

        void Update()
        {
            
            if (defaultDPR != 0 && !waitForRenderPipelineReady)
            {
                if (!dynamicResolutionSystem)
                {
                    if (dynamicResolutionSystem != lastDynamicResolutionSystem || dpr == 0)
                    {
                        lastDynamicResolutionSystem = dynamicResolutionSystem;

                        fixedDPIUIElement.SetActive(true);
                        dpiUIElement.SetActive(false);

                        lastDPR = 0;
                    }

                    dpi = fixedDPI;

                    if (useRenderScaleURP && urp)
                    {
                        dpr = dpi;

                        if (dpr != lastDPR)
                        {
                            var rpAsset = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset;
#if USING_URP
                            var urpAsset = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)rpAsset;
                            urpAsset.renderScale = dpr;
#endif
                        }
                    }
                    else
                    {
                        dpr = dpi * defaultDPR;

                        if (dpr != lastDPR)
                        {
                            __setDPR(dpr);
                        }
                    }

                    lastDPR = dpr;

                }
                else
                {
                    if (dynamicResolutionSystem != lastDynamicResolutionSystem || dpr == 0)
                    {
                        lastDynamicResolutionSystem = dynamicResolutionSystem;
                        m_FpsNextPeriod = Time.realtimeSinceStartup + measurePeriod;

                        fixedDPIUIElement.SetActive(false);
                        dpiUIElement.SetActive(true);

                        lastDPR = 0;
                    }

                    getAverageFPS();
                }
            }

            dpi = (float)Math.Round(dpi * 100f) / 100f;
            dpi_TMP_Text.text = dpi.ToString();

            updateUI();

            if (ShowHideUI != lastShowHideUI)
            {
                webglFpsAcceleratorInGameUI.transform.parent.gameObject.GetComponent<Canvas>().enabled = ShowHideUI;
                lastShowHideUI = ShowHideUI;
            }
        }

        public void m_fpsMax()
        {
            fpsMax = int.Parse(AG_inputManager1_fpsMax.valueString);
        }

        public void m_fpsMin()
        {
            fpsMin = int.Parse(AG_inputManager1_fpsMin.valueString);
        }

        public void m_dpiMax()
        {
            dpiMax = float.Parse(AG_inputManager1_dpiMax.valueString);
        }

        public void m_dpiMin()
        {
            dpiMin = float.Parse(AG_inputManager1_dpiMin.valueString);
        }

        public void m_dpiDecrease()
        {
            dpiDecrease = float.Parse(AG_inputManager1_dpiDecrease.valueString);
        }

        public void m_dpiIncrease()
        {
            dpiIncrease = float.Parse(AG_inputManager1_dpiIncrease.valueString);
        }

        public void m_measurePeriod()
        {
            measurePeriod = float.Parse(AG_inputManager1_measurePeriod.valueString);
        }

        public void m_fixedDPI()
        {
            fixedDPI = AG_inputManager2_fixedDPI.value;
        }

        public void updateUI()
        {
            AG_inputManager1_fpsMax.changeValueString(fpsMax.ToString());
            AG_inputManager1_fpsMin.changeValueString(fpsMin.ToString());
            AG_inputManager1_dpiMin.changeValueString(dpiMin.ToString());
            AG_inputManager1_dpiMax.changeValueString(dpiMax.ToString());
            AG_inputManager1_dpiIncrease.changeValueString(dpiIncrease.ToString());
            AG_inputManager1_dpiDecrease.changeValueString(dpiDecrease.ToString());
            AG_inputManager1_measurePeriod.changeValueString(measurePeriod.ToString());

            if (fixedDPIUIElement.activeSelf)
                AG_inputManager2_fixedDPI.changeValue(fixedDPI);
        }

        public void makeWaitForRenderPipelineReadyFalse()
        {
            waitForRenderPipelineReady = false;
        }
    }
}
