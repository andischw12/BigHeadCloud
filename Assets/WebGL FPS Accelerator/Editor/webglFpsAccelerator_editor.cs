using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace AG_WebGLFPSAccelerator
{
    public class webglFpsAccelerator_editor
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string targetPath)
        {
            var path = Path.Combine(targetPath, "index.html");
            var text = File.ReadAllText(path);

            string string1 = "";

            if (text.Contains("createUnityInstance") == true)
            {
                string1 = @"
then((unityInstance) => {
          unity_instance = unityInstance;
        ";

                text = text.Replace("then((unityInstance) => {", string1);

            }
            else
            {
                string[] x = text.Split(new string[] { " = UnityLoader.instantiate" }, StringSplitOptions.None);
                string[] x2 = x[0].Split(new string[] { "var " }, StringSplitOptions.None);
                string string2 = x2[x2.Length - 1];

                string1 = @"
                <script>
                    var unity_instance;

                    document.addEventListener('DOMContentLoaded', () => {
                      unity_instance = " + string2 + @";
                    });
                </script>
                    ";

                text = text.Replace("</head>", string1 + "\n" + "</head>");
            }

            File.WriteAllText(path, text);
        }
    }
}