using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEditor;
using System.Runtime.InteropServices;

namespace AG_WebGLFPSAccelerator
{
    public class wfa_Demo : MonoBehaviour
    {
        public static wfa_Demo instance;

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        public static extern bool isAndroid2();

        [DllImport("__Internal")]
        public static extern bool isiOS2();
#endif
        
        [HideInInspector]
        public GameObject warningText;
        
        [HideInInspector]
        public GameObject requiredSettings;
        
        [HideInInspector]
        public GameObject postProcessVolume;
        
        private float shadowDistanceTemp;

        [HideInInspector]
        public bool isiOS;
        [HideInInspector]
        public bool isAndroid;
        [HideInInspector]
        public bool receivedPlatform;

        private bool completed1;

#if UNITY_2021_1_OR_NEWER && USING_URP
        [HideInInspector]
        public VolumeProfile VolumeProfile1;
#endif

        void Update()
        {
            if (receivedPlatform && !completed1)
            {
                if (isiOS)
                {
                    WebGLFPSAccelerator.instance.fpsMin = 45;
                    WebGLFPSAccelerator.instance.fpsMax = 50;

                    WebGLFPSAccelerator.instance.dpiMin = 0.6f;
                }
                else if (isAndroid)
                {
                    WebGLFPSAccelerator.instance.fpsMin = 10;
                    WebGLFPSAccelerator.instance.fpsMax = 15;

                    WebGLFPSAccelerator.instance.dpiMin = 0.5f;
                }
                else
                {
                    WebGLFPSAccelerator.instance.fpsMin = 28;
                    WebGLFPSAccelerator.instance.fpsMax = 33;

                    WebGLFPSAccelerator.instance.dpiMin = 0.5f;
                }

                completed1 = true;
            }
        }

        void Awake()
        {
            instance = this;
        }

        public void isAndroid3()
        {
            if(isAndroid)
            {
#if USING_URP
                postProcessVolume.SetActive(false);
#endif
            }
        }

        void Start()
        {

#if UNITY_2021_1_OR_NEWER && USING_URP
            postProcessVolume.GetComponent<Volume>().profile = VolumeProfile1;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
            isiOS = isiOS2();
            isAndroid = isAndroid2();

            receivedPlatform = true;
#endif

            isAndroid3();

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += playModeStateChanged;
#endif

            QualitySettings.SetQualityLevel(3);
            QualitySettings.vSyncCount = 0;
            QualitySettings.shadowDistance = 300f;

#if USING_URP
            var rpAsset = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset;
            var urpAsset = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)rpAsset;

            shadowDistanceTemp = urpAsset.shadowDistance;
            urpAsset.shadowDistance = 200f;
#endif

            m1();
        }

#if UNITY_EDITOR
        public void playModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
#if USING_URP
                var rpAsset = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset;
                var urpAsset = (UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset)rpAsset;

                urpAsset.shadowDistance = shadowDistanceTemp;
#endif
            }
        }
#endif

        public void m1()
        {
#if UNITY_EDITOR
            if (warningText)
                warningText.SetActive(true);
#endif

            if (requiredSettings)
            {
#if !UNITY_EDITOR
                requiredSettings.SetActive(false);
#endif
            }
        }
    }
}