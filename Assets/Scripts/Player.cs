using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Febucci.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
 
    public CustomTimer playeTimer;
    [SerializeField] int _currentAnswer;
    [SerializeField] protected int _totalCorrectAnswers;
    [SerializeField] protected int _totalWrongAnswers;
    [SerializeField] public TextMeshProUGUI totalScoreTXT;
    [SerializeField] public TextMeshProUGUI PlayerNameTXT;
    [SerializeField] public EmoteGuiButton myEmojiesGuiButton;
    [SerializeField] int _playerSide;
    public int PointsForScore;
    [SerializeField] playerAI _playerAI;
    public GameObject PlayerCanvas;
    public HelperManager myhelperManager;
    public PhotonPlayerManager myPhotonPlayer;
    protected int _totalScore;
    [SerializeField] GameObject _mySignate;
    public string _myName; 
    
    public int TotalScore {get{return _totalScore;}set{_totalScore = value;}}
    public int PlayerSide { get { return _playerSide; } set { _playerSide = value; } }
    public int CurrentAnswer { get{return _currentAnswer;}set{ _currentAnswer = value;}}
    public int TotalCorrectAnswers {get{return _totalCorrectAnswers;} set{_totalCorrectAnswers = value;}}
    public void Dancing(float time) {StartCoroutine(DancingAnimation(time));}

    public int TotalTimePrecent { get; set;}
    private void Start()
    {
        Invoke("Assiment", 1.1f);
    }
    protected virtual void Assiment() 
    {
        if (myPhotonPlayer.GetType() ==  typeof(PhotonPlayerManagerBot) || myPhotonPlayer.PV.Owner.ActorNumber == 2)
        {
           
            myhelperManager = Assignment.instance.Helpers[1];
            PlayerCanvas = Assignment.instance.PlayerCanvas[1];
            totalScoreTXT = Assignment.instance.PlayerScoreText[1];
            PlayerNameTXT = Assignment.instance.PlayerNameText[1];
            myEmojiesGuiButton = Assignment.instance.PlayerEmoteGuiButton[1];
            PlayerSide = 2;
            FindObjectOfType<ButtonClicked>().PhotonPlayerM2 = myPhotonPlayer;
            myPhotonPlayer.transform.position = Assignment.instance.PlayersPosition[1].position;
            myPhotonPlayer.transform.rotation = Assignment.instance.PlayersPosition[1].rotation;
            if (GetComponent<playerAI>() != null)
                _playerAI = GetComponent<playerAI>();
            GetComponentInChildren<Camera>().targetTexture = Assignment.instance.PlayerRawImage[1];
            GameManager.instance.player2 = this;
        }
        else  
        {
          
            myhelperManager = Assignment.instance.Helpers[0];
            PlayerCanvas = Assignment.instance.PlayerCanvas[0];
            totalScoreTXT = Assignment.instance.PlayerScoreText[0];
            PlayerNameTXT = Assignment.instance.PlayerNameText[0];
            myEmojiesGuiButton = Assignment.instance.PlayerEmoteGuiButton[0];
            PlayerSide = 1;
            FindObjectOfType<ButtonClicked>().PhotonPlayerM1 = myPhotonPlayer;
            myPhotonPlayer.transform.position = Assignment.instance.PlayersPosition[0].position;
            myPhotonPlayer.transform.rotation = Assignment.instance.PlayersPosition[0].rotation;
            GetComponentInChildren<Camera>().targetTexture = Assignment.instance.PlayerRawImage[0];
            GetComponentInChildren<Animator>().SetBool("MirrorPushButton", true);
            GameManager.instance.player1 = this;

        }
        myEmojiesGuiButton.myPlayer = this;
        myhelperManager.myPlayerAvatar = this;
        TotalCorrectAnswers = 0;
        CurrentAnswer = 0;
        playeTimer = PlayerCanvas.GetComponentInChildren<CustomTimer>();
        UI_Effects.instance.ResetSliders();
        _mySignate = GetComponent<KidAvatarSelector>().GetSignateGM();
        _mySignate.SetActive(false);
        PlayerNameTXT.text = _myName;
        PlayerNameTXT.gameObject.AddComponent<TextAnimator>();
        totalScoreTXT.gameObject.AddComponent<TextAnimator>();
        GetComponentInChildren<Animator>().gameObject.AddComponent<lookatactivecam>();
    }

    public void ChooseAnswer(int answer,bool show) // called by player click or AI script  or by button itself or by jocker
    {
        if (!IsTimeOver() && CurrentAnswer==0)// check if time is over before startring the process of choosing answer;
        {
            playeTimer.PauseTimer();// pause here beacuse of unknown bug.
            StartCoroutine(ChooseAnswerHelper(answer, show));
        }
           
    }

    IEnumerator ChooseAnswerHelper(int answer, bool show) 
    {
        CurrentAnswer = answer;
        myhelperManager.MakeHelpersNotInteract();
        playeTimer.PauseTimer();
        GetComponentInChildren<Animator>().SetBool("Think", false);
        GetComponentInChildren<Animator>().SetBool("Waiting", true);
        GetComponentInChildren<Animator>().SetTrigger("WaitForAnswer");
        yield return new WaitForSecondsRealtime(0.8f);
        ShowMyBigFaceOnUi();
        if (show) 
        {
            SoundManager.instance.PlaySoundEffect(SoundEffectsList.Click);
            UIManager.instance.SetFinalAnswer(answer);
            CalculationsManager.instance.LastGameSpeed += playeTimer.GetTimerValue() * 10;
        }
            
    }
    public void StartPlayerAI() 
    {
        if (_playerAI != null)
            _playerAI.startAiSesseion();
    }
    public virtual IEnumerator CorrectAnswer()
    {
        TotalCorrectAnswers++;
        if (GameManager.instance.thisComputerPlayer == this)
            CalculationsManager.instance.LastGameCorrectAnswers++;
        myPhotonPlayer.UpdateScore(_totalScore + CalculateRoundScore());
        yield return new WaitUntil(() => myPhotonPlayer.MycoreIsUpdated);
        totalScoreTXT.SetText(_totalScore.ToString());
        yield return new WaitForSecondsRealtime(0.1f);
        UI_Effects.instance.textEffect(totalScoreTXT, "<incr>" + totalScoreTXT.text,4f);
        UI_Effects.instance.textEffect(PlayerNameTXT, "<rainb>" + PlayerNameTXT.text,4f);
        UI_Effects.instance.SetSliderVal(PlayerSide,CalculateSliderScore());
        UI_Effects.instance.PlayStartEffect(PlayerSide);
        myPhotonPlayer.MyScoreIsNotUpdated();
        GetComponentInChildren<Animator>().SetBool("Waiting", false);
        GetComponentInChildren<Animator>().SetTrigger("Happy");
    }
    public void WinGame() 
    {
        

        SoundManager.instance.PlaySoundEffect(SoundEffectsList.WinGame);
        GetComponentInChildren<Animator>().SetTrigger("Happy");
    }
    public void TimeOverAnimation()
    {
        GetComponentInChildren<Animator>().SetTrigger("TimeOver");
        //GetComponentInChildren<Animator>().SetBool("Waiting", true);
    }

    public void ThinkAnimation(bool b) 
    {
        GetComponentInChildren<Animator>().SetBool("Think", b);
    }

    public IEnumerator WrongAnswer() 
    {
        yield return new WaitForSecondsRealtime(0);
        _totalWrongAnswers++;
        if (GameManager.instance.thisComputerPlayer == this)
            CalculationsManager.instance.LastGameWrongAnswers++;
        GetComponentInChildren<Animator>().SetBool("Waiting", false);
        GetComponentInChildren<Animator>().SetTrigger("Sad");
    }
    private IEnumerator DancingAnimation(float dancingTime)
    {
        GetComponentInChildren<Animator>().SetBool("Dancing", true);
        yield return new WaitForSecondsRealtime(dancingTime);
        GetComponentInChildren<Animator>().SetBool("Dancing", false);
    }

    public void ShowMyFaceOnUiAnswer() 
    {
        UIManager.instance.ShowFaceOnAnswer(PlayerSide, CurrentAnswer);
    }

    public void  ShowMyBigFaceOnUi() 
    {
        UIManager.instance.ShowBigFaceWhenChosen(_playerSide);
    }

    /// <summary>
    /// this method is showing and hiding the playres sing according to the paramter showOrHide
    /// </summary>

    public void ShowOrHideSign(bool showOrHide) 
    {
        StartCoroutine(UseSignHelper(showOrHide));
    }
    IEnumerator UseSignHelper(bool showOrHide) 
    {
        if (showOrHide && CurrentAnswer > 0)
        {
            GetComponentInChildren<Animator>().SetBool("Waiting", false);
            GetComponentInChildren<Animator>().SetTrigger("Sign");
            yield return new WaitForSecondsRealtime(0.5f);
            SoundManager.instance.PlaySoundEffect(SoundEffectsList.SignateIn);
            _mySignate.SetActive(true);
            _mySignate.GetComponentInChildren<SignsTextures>().chooseAnswer(CurrentAnswer);
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.5f);
            _mySignate.SetActive(false);
        }
    }
    public void ResetParameters()
    {
        CurrentAnswer = 0;
        playeTimer.ResetTimer();
        
    }

    // correct answer points - 0.3 depens on the time u did it.. 0.6 is if its right or wrong
    protected int CalculateRoundScore() 
    {
        int toReturn = GameManager.instance.PointsPerQuestion/10 + (GameManager.instance.PointsPerQuestion * 1 / 3 * TimerValToPrecent() / 100) + (GameManager.instance.PointsPerQuestion * 2 / 3); // adding 10% extra beacause of time lose
        if(GameManager.instance.thisComputerPlayer ==this)
            CalculationsManager.instance.LastGameScore += toReturn;
        return toReturn;
    }

    protected float CalculateSliderScore()
    {
        return (float)TotalScore / (GameManager.instance.PointsPerQuestion * GameManager.instance.totalNumOfQuestions);
    }

    private int TimerValToPrecent() 
    {
        int current = 100- (int)(playeTimer.GetTimerValue() * 100);// adding abit time points 105 instad of 100
        TotalTimePrecent += current;
        return current;
    }

    public bool IsTimeOver() 
    {
        if (playeTimer.GetTimerValue() < 1)
            return false;
        return true;
    }


    public IEnumerator TimerProcessThisPlayer()
    {
        yield return new WaitUntil(() => playeTimer.GetTimerValue() > 0.7f); // wait 7 sec
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.TimerBeep, 1, Mathf.RoundToInt(GameManager.instance.timePerQuestion * 0.3f)); // start the sound effcet
        UI_Effects.instance.StartTimeIsAlmostOver(PlayerSide); // start red blincking
        yield return new WaitUntil(() => IsTimeOver() || CurrentAnswer>0); // wait until 
        UI_Effects.instance.StopTimeIsAlmostOver(PlayerSide);
        SoundManager.instance.StopSoundEffect();
        if (IsTimeOver()) 
        {
            SoundManager.instance.StopTimerSound();
            SoundManager.instance.PlaySoundEffect(SoundEffectsList.Buzzer);
            ThinkAnimation(false);
            GetComponentInChildren<Animator>().SetBool("Waiting", false);
            TimeOverAnimation();
            UI_Effects.instance.MakeAllAnsersGray();
        }
        CalculationsManager.instance.LastGameSpeed += GameManager.instance.timePerQuestion;
    }

    public IEnumerator TimerProcessOtherPlayer()
    {
        yield return new WaitUntil(() => IsTimeOver() || CurrentAnswer > 0); // wait until 
        if (IsTimeOver())
        {
            ThinkAnimation(false);
            GetComponentInChildren<Animator>().SetBool("Waiting", false);
            TimeOverAnimation();
        }
    }




}
