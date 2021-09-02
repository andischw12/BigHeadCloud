using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class Demo : MonoBehaviour 
{
	public GameObject cameraGo = null;

	public MeshRenderer cloudRenerer = null;

	public GameObject rainGo = null;

	public Light mainLight = null;

	private Material cloudMaterialOriginal = null;

	private FreeLookCam cam = null;

	private int weatherStatus = 0;

	private bool sparseCloud = false;

	private void Start()
	{
		cloudMaterialOriginal = cloudRenerer.material;
		cloudRenerer.material = new Material(cloudMaterialOriginal);
		cam = cameraGo.GetComponent<FreeLookCam>();
		RenderSettings.skybox = new Material(RenderSettings.skybox);
	}

	private void Update()
	{
		if(Input.GetKeyUp(KeyCode.Space))
		{
			cam.enabled = !cam.enabled;
		}

		{
			Material cloudMaterial = cloudRenerer.material;
			float alpha = cloudMaterial.GetFloat("_Alpha");
			
			if(weatherStatus == 1) 
			{
				alpha -= 0.075f;
				if(alpha < 0.0f)
				{
					alpha = 0.0f;
					weatherStatus = 0;
				}
				SetWeather(alpha);
			}
			if(weatherStatus == 2)
			{
				alpha += 0.075f;
				if(alpha > 1.0f)
				{
					alpha = 1.0f;
					weatherStatus = 0;
				}
				SetWeather(alpha);
			}
		}
	}

	private void SetWeather(float alpha)
	{
		Material cloudMaterial = cloudRenerer.material;

		Color sunnyCloudColor = new Color(177f/255f,201f/255f,217f/255f,1);
		Color rainyCloudColor = new Color(107f/255f,122f/255f,131f/255f,1);
		Color sunnyLightColor = new Color(1, 223f/255f, 189f/255f, 1);
		Color rainyLightColor = new Color(1,1,1,1);

		cloudMaterial.SetFloat("_Alpha", alpha);
		cloudMaterial.SetColor("_Color", Color.Lerp(sunnyCloudColor, rainyCloudColor, alpha));
		cloudMaterial.SetFloat("_SkyboxExposure", Mathf.Lerp(1,0.7f,alpha));
		cloudMaterial.SetColor("_SkyboxTint", Color.Lerp(new Color(0.5f,0.5f,0.5f, 0.5f), new Color(0.3f,0.3f,0.3f,0.5f), alpha));
		mainLight.color = Color.Lerp(sunnyLightColor, rainyLightColor, alpha);
		mainLight.intensity = Mathf.Lerp(1f, 0.7f, alpha);
		RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(1,0.7f, alpha));
		RenderSettings.skybox.SetColor("_Tint", Color.Lerp(new Color(0.5f,0.5f,0.5f, 0.5f), new Color(0.3f,0.3f,0.3f,0.5f), alpha));
	}

	private void OnGUI()
	{
		GUILayout.BeginVertical();
		{
			OnGUI_LockCamera();
			GUILayout.FlexibleSpace();
			OnGUI_Reset();
			GUILayout.FlexibleSpace();
			OnGUI_CloudColor();
			GUILayout.FlexibleSpace();
			OnGUI_WindSpeed();
			GUILayout.FlexibleSpace();
			OnGUI_Rain();
			GUILayout.FlexibleSpace();
			OnGUI_SparseCloud();
			GUILayout.FlexibleSpace();
			OnGUI_Quality();
		}
		GUILayout.EndVertical();
	}

	private void OnGUI_Quality()
	{
		Material cloudMaterial = cloudRenerer.material;
		int quality = cloudMaterial.GetInt("_Quality");
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Quality");
			if(GUILayout.Button("<<"))
			{
				quality *= 2;
			}
			if(GUILayout.Button(">>"))
			{
				quality /= 2;
			}
			quality = Mathf.Clamp(quality, 1, 16);
			cloudMaterial.SetInt("_Quality", quality);
			GUILayout.Label(quality == 16 ? "Low" : quality == 8 ? "Middle" : quality == 4 ? "Hight" : "Very Hight");
		}
		GUILayout.EndHorizontal();
	}

	private void OnGUI_SparseCloud()
	{
		bool sparseCloud = GUILayout.Toggle(this.sparseCloud, "Sparse Cloud");
		if(sparseCloud != this.sparseCloud)
		{
			Material cloudMaterial = cloudRenerer.material;
			Vector4 value = cloudMaterial.GetVector("_HeightTileSpeed");
			Vector4 value2 = cloudMaterial.GetVector("_DynamicMaskParams");
			this.sparseCloud = sparseCloud;
			if(this.sparseCloud)
			{
				value.z = 0.01f;
				value.w = 0.01f;
				value2.y = 1.88f;
				value2.z = 21.83f;
				value2.w = 0.1f;
			}
			else
			{
				value.z = 0.02f;
				value.w = 0.02f;
				value2.y = 2;
				value2.z = 3;
				value2.w = 1;
			}
			cloudMaterial.SetVector("_HeightTileSpeed", value);
			cloudMaterial.SetVector("_DynamicMaskParams", value2);
		}
	}

	private void OnGUI_LockCamera()
	{
		GUILayout.Label("Press Space Key to Lock/Unlock Camera");
	}

	private void OnGUI_Reset()
	{
		if(GUILayout.Button("Reset"))
		{
			cloudRenerer.material = new Material(cloudMaterialOriginal);
			rainGo.SetActive(false);
			weatherStatus = 1;
			sparseCloud = false;
		}
	}

	private void OnGUI_Rain()
	{
		GUILayout.BeginHorizontal();
		{
			if(GUILayout.Button("Sunny"))
			{
				rainGo.SetActive(false);
				weatherStatus = 1;
			}
			if(GUILayout.Button("Rainy"))
			{
				rainGo.SetActive(true);
				weatherStatus = 2;
			}
		}
		GUILayout.EndHorizontal();
	}

	private void OnGUI_WindSpeed()
	{
		Material cloudMaterial = cloudRenerer.material;
		Vector4 value = cloudMaterial.GetVector("_HeightTileSpeed");

		GUILayout.BeginVertical();
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Wind X");
				value.z = GUILayout.HorizontalSlider(value.z, -0.1f, 0.1f, GUILayout.Width(100));
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Wind Y");
				value.w = GUILayout.HorizontalSlider(value.w, -0.1f, 0.1f, GUILayout.Width(100));
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndHorizontal();

		cloudMaterial.SetVector("_HeightTileSpeed", value);
	}

	private void OnGUI_CloudColor()
	{
		Material cloudMaterial = cloudRenerer.material;
		Color cloudColor = cloudMaterial.GetColor("_Color");

		GUILayout.BeginVertical();
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Cloud Color R");
				cloudColor.r = GUILayout.HorizontalSlider(cloudColor.r, 0, 1, GUILayout.Width(100));
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Cloud Color G");
				cloudColor.g = GUILayout.HorizontalSlider(cloudColor.g, 0, 1, GUILayout.Width(100));
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Cloud Color B");
				cloudColor.b = GUILayout.HorizontalSlider(cloudColor.b, 0, 1, GUILayout.Width(100));
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();

		cloudMaterial.SetColor("_Color", cloudColor);
	}
}
