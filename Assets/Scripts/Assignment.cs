using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Assignment : MonoBehaviour
{
    public Transform[] PlayersPosition = new Transform[2];
    public Transform[] VsPlayersPosition = new Transform[2];
    public TextMeshProUGUI[] VsPlayerNameText = new TextMeshProUGUI[2];
    public HelperManager[] Helpers = new HelperManager[2];
    public GameObject[] PlayerCanvas = new GameObject[2];
    public TextMeshProUGUI[] PlayerScoreText = new TextMeshProUGUI[2];
    public TextMeshProUGUI[] PlayerNameText = new TextMeshProUGUI[2];
    public EmoteGuiButton[] PlayerEmoteGuiButton = new EmoteGuiButton[2];
    public CustomRenderTexture[] PlayerRawImage = new CustomRenderTexture[2];
    public GameObject VSCanvas;
    public TextMeshProUGUI QuizSubjectText;
    public TextMeshProUGUI TechMassage;


    public static Assignment instance;
    void Awake()
    {
        instance = this;    
    }

     
}
