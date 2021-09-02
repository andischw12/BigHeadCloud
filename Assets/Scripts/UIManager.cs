using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using System.Linq;
using Febucci.UI.Core;
using UnityEngine.Assertions;
public class UIManager : MonoBehaviour
{
    public static UIManager instance; // static variable which is used to get reference to UIManager instance from every script
    public List<Sprite> lozengeSprites; // list of all sprites used at logenze panel | left(inact, act, final, correct) then right (inact, act, final, correct)
    public GameObject lozengePanel;                 //
    public GameObject currentMassgaePanel;            //
    public GameObject facesAnswersHolder;
    PlayerAnswerFacesScript[] facesAnswers;
    public GameObject questionNumberPanel;
    public GameObject countDownPanel;
    [SerializeField] GameObject Arie;
    [HideInInspector] public bool allAnswersAreDisplayed = false;
    void Awake()
    { 
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        facesAnswers = facesAnswersHolder.GetComponentsInChildren<PlayerAnswerFacesScript>();
    }
    public void ShowQuestion(string questionText, string[] answers)
    {
        StartCoroutine(ShowQuestionIE(questionText, answers));
    }
    IEnumerator ShowQuestionIE(string questionText, string[] answers) 
    {
        yield return new WaitUntil(() => Arie.GetComponentInChildren<Animator>().GetAnimatorTransitionInfo(0).IsName("Lion Talk -> Lion Idle"));
        //setting question text
        lozengePanel.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = questionText;
        MakeAnswersNotInteractable();
        lozengePanel.SetActive(true);
       
        //setting answers texts
        for (int i = 0; i < 4; i++)
        {
            lozengePanel.transform.GetChild(i + 3).GetChild(1).GetComponent<Text>().text = answers[i];
        }
        ResetFacesOnAnswers();
    }
    public void SetFinalAnswer(int answerNumber)
    {
        allAnswersAreDisplayed = false;
        if (answerNumber == 1 || answerNumber == 3)
        {
            lozengePanel.transform.GetChild(answerNumber + 2).GetComponent<Image>().sprite = lozengeSprites[2];
            lozengePanel.transform.GetChild(answerNumber + 2).GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
          //  lozengePanel.transform.GetChild(answerNumber + 2).GetChild(1).GetComponent<Text>().color = new Color32(0, 0, 0, 255);
        }
        else
        {
            lozengePanel.transform.GetChild(answerNumber + 2).GetComponent<Image>().sprite = lozengeSprites[6];
            lozengePanel.transform.GetChild(answerNumber + 2).GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            //lozengePanel.transform.GetChild(answerNumber + 2).GetChild(1).GetComponent<Text>().color = new Color32(0, 0, 0, 255);
        }
        MakeAnswersNotInteractable();
    }

    public void MakeAnswersNotInteractable() 
    {
        for (int i = 0; i < 4; i++)
            lozengePanel.transform.GetChild(i + 3).GetChild(2).GetComponent<Button>().interactable = false;
        
    }

    public void MakeAnswersInteractable()
    {
        for (int i = 0; i < 4; i++)
            lozengePanel.transform.GetChild(i + 3).GetChild(2).GetComponent<Button>().interactable = true;
         
    }

    public IEnumerator CorrectAnswer(int numberOfCorrectAnswer, string profit)
    {
        ////if((GameProcess.instance.currentQuestionNumber) %3!=0){
        UI_Effects.instance.StartCorrectAnswerEffect(GameProcess.instance.question.CorrectAnswer);
        int i = 0;
        while (i < 3)
        {
            //setting correct answer sprite and black color of text
            if (numberOfCorrectAnswer == 1 || numberOfCorrectAnswer == 3)
            {
               // lozengePanel.transform.GetChild(numberOfCorrectAnswer + 2).GetComponent<Image>().sprite = lozengeSprites[3];
                lozengePanel.transform.GetChild(numberOfCorrectAnswer + 2).GetChild(1).GetComponent<Text>().color = new Color32(0, 0, 0, 255);
            }
            else
            {
                //lozengePanel.transform.GetChild(numberOfCorrectAnswer + 2).GetComponent<Image>().sprite = lozengeSprites[7];
                lozengePanel.transform.GetChild(numberOfCorrectAnswer + 2).GetChild(1).GetComponent<Text>().color = new Color32(0, 0, 0, 255);
            }
            yield return new WaitForSecondsRealtime(0.5f);
            //setting final answer sprite and black color of text
            if (numberOfCorrectAnswer == 1 || numberOfCorrectAnswer == 3)
            {
               // lozengePanel.transform.GetChild(numberOfCorrectAnswer + 2).GetComponent<Image>().sprite = lozengeSprites[2];
               // lozengePanel.transform.GetChild(numberOfCorrectAnswer + 2).GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            }
            else
            {
               // lozengePanel.transform.GetChild(numberOfCorrectAnswer + 2).GetComponent<Image>().sprite = lozengeSprites[6];
               // lozengePanel.transform.GetChild(numberOfCorrectAnswer + 2).GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            }
            yield return new WaitForSecondsRealtime(0.5f);
            i++;
        }
        UI_Effects.instance.StopCorrectAnswerEffect(GameProcess.instance.question.CorrectAnswer);
        CloseLozengePanel();
    ////}
       // StartCoroutine(ShowCurrentPrizePanel(profit, false));
    }
    public void CloseLozengePanel()
    {
         
        lozengePanel.GetComponent<Animator>().enabled = true;
        allAnswersAreDisplayed = false;
        // making logenze panel invisible
        lozengePanel.SetActive(false);
        // making visible question text
        lozengePanel.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        //seting apropriate sprite at answer A and making text invisible and white
        lozengePanel.transform.GetChild(3).GetComponent<Image>().sprite = lozengeSprites[0];
        lozengePanel.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
        lozengePanel.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
        //lozengePanel.transform.GetChild(3).GetChild(0).GetComponent<Text>().color = new Color32(246, 162, 0, 255);
        lozengePanel.transform.GetChild(3).GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        //seting apropriate sprite at answer B and making text invisible and white
        lozengePanel.transform.GetChild(4).GetComponent<Image>().sprite = lozengeSprites[4];
        lozengePanel.transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
        lozengePanel.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);
        //lozengePanel.transform.GetChild(4).GetChild(0).GetComponent<Text>().color = new Color32(246, 162, 0, 255);
        lozengePanel.transform.GetChild(4).GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        //seting apropriate sprite at answer C and making text invisible and white
        lozengePanel.transform.GetChild(5).GetComponent<Image>().sprite = lozengeSprites[0];
        lozengePanel.transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
        lozengePanel.transform.GetChild(5).GetChild(1).gameObject.SetActive(true);
       // lozengePanel.transform.GetChild(5).GetChild(0).GetComponent<Text>().color = new Color32(246, 162, 0, 255);
        lozengePanel.transform.GetChild(5).GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        //seting apropriate sprite at answer D and making text invisible and white
        lozengePanel.transform.GetChild(6).GetComponent<Image>().sprite = lozengeSprites[4];
        lozengePanel.transform.GetChild(6).GetChild(0).gameObject.SetActive(true);
        lozengePanel.transform.GetChild(6).GetChild(1).gameObject.SetActive(true);
        //lozengePanel.transform.GetChild(6).GetChild(0).GetComponent<Text>().color = new Color32(246, 162, 0, 255);
        lozengePanel.transform.GetChild(6).GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
    }
    public void Lifeline5050()
    {
        if (allAnswersAreDisplayed)
        {
            Lifeline50x50 lifeline5050 = new Lifeline50x50();
            int[] wrongAnswers = lifeline5050.Use();
            lozengePanel.GetComponent<Animator>().enabled = false;
            //hiding wrong answer 1
            lozengePanel.transform.GetChild(wrongAnswers[0] + 2).GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 255, 0);
            lozengePanel.transform.GetChild(wrongAnswers[0] + 2).GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 255, 0);
            lozengePanel.transform.GetChild(wrongAnswers[0] + 2).GetChild(2).gameObject.SetActive(false);
            //hiding wrong answer 2
            lozengePanel.transform.GetChild(wrongAnswers[1] + 2).GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 255, 0);
            lozengePanel.transform.GetChild(wrongAnswers[1] + 2).GetChild(1).GetComponent<Text>().color = new Color32(255, 255, 255, 0);
            lozengePanel.transform.GetChild(wrongAnswers[1] + 2).GetChild(2).gameObject.SetActive(false);
        }
    }
    public void ShowFaceOnAnswer(int PlayerSide,int answer) 
    {
        for (int i = 0; i < 4; i++)
            if (i + 1 == answer) 
            {
                facesAnswers[i].choosePlayerFace(PlayerSide);
                SoundManager.instance.PlaySoundEffect(SoundEffectsList.SmallFaceOnAnswer);
            }
    }
    public void ShowBigFaceWhenChosen(int PlayerSide) 
    {
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.Bell);
        facesAnswers[facesAnswers.Length - 1].choosePlayerFace(PlayerSide);
    }
    public void HideBigFaces()
    {
        //for (int i = 1; i < facesAnswers[facesAnswers.Length - 1].children.Length; i++)
            facesAnswers[facesAnswers.Length - 1].resetPlayerFaces();
    }
    public void ShowMassageToUser(string temp) 
    {
        currentMassgaePanel.SetActive(true);
        currentMassgaePanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = temp;
    }
    public void ShowQuestionNumber() 
    {
       currentMassgaePanel.SetActive(true);
        string temp = "";
        int questionNumber = GameProcess.instance.currentQuestionNumber;
        if (questionNumber <= GameManager.instance.totalNumOfQuestions) 
        {
            switch (questionNumber)
            {
                case 1:
                    temp = "הנושאר הלאש";
                    break;
                case 2:
                    temp = "הינש הלאש";
                    break;
                case 3:
                    temp = "תישילש הלאש";
                    break;
                case 4:
                    temp = "תיעיבר הלאש";
                    break;
                case 5:
                    temp = "תישימח הלאש";
                    break;
                case 6:
                    temp = "תישיש הלאש";
                    break;
                case 7:
                    temp = "תיעיבש הלאש";
                    break;
                default:
                    temp = "השדח הלאש";
                    break;
            }
        }
        else
            temp = "ןויווש רבוש";
        currentMassgaePanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = temp;
    }
    public void CloseAllPanels() 
    {
        lozengePanel.SetActive(false);                //
        currentMassgaePanel.SetActive(false);            //
        facesAnswersHolder.SetActive(false); 
        questionNumberPanel.SetActive(false); 
        countDownPanel.SetActive(false); 
    }
    public void HideQuestionAndPrizePannel() 
    {
        //questionNumberPanel.GetComponentInChildren<TextMeshProUGUI>().ClearMesh();
        currentMassgaePanel.SetActive(false);
    }
    public void ShowTotalPrizeWin(string temp) 
    {
        currentMassgaePanel.SetActive(true);
        currentMassgaePanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = temp;
        currentMassgaePanel.transform.GetChild(0).gameObject.SetActive(true);
        currentMassgaePanel.transform.GetChild(3).gameObject.SetActive(true);
        currentMassgaePanel.transform.GetChild(4).gameObject.SetActive(true);
        currentMassgaePanel.transform.GetChild(5).gameObject.SetActive(true);
    }
    public void startCountDown(int secs)
    {
        StartCoroutine(startCountDownHelper(secs));
    }
    IEnumerator startCountDownHelper(int secs) 
    {
        countDownPanel.SetActive(true);
        Debug.Log(secs);
        for (int i = secs ; i>0; i--) 
        {
            SoundManager.instance.PlaySoundEffect(SoundEffectsList.TimerBeep);
            countDownPanel.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }
        countDownPanel.SetActive(false);
    }
    public void ResetFacesOnAnswers() 
    {
        foreach (PlayerAnswerFacesScript T in facesAnswers)
            T.resetPlayerFaces();
    }
}
