using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System;
using Febucci.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class GameManager : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void saveWinnersJS(string Winner, string Loser);

    [DllImport("__Internal")]
    public static extern void pushWinnersJS();


    public static GameManager instance;
    [Header("GameObject holders")]
    [SerializeField] protected HostCharachter host;
    public Player player1;
    public Player player2;
    [Header("Game results")]
    [SerializeField] public int totalNumOfQuestions;
    [SerializeField] public float timePerQuestion;
    [SerializeField] protected int currentQuestionNum;
    [SerializeField] int _pointsPerQuestion;
    [SerializeField] public Player thisComputerPlayer;
    [SerializeField] public Player otherPlayer;
    [SerializeField] GameObject Cover;
    [SerializeField] public EnviormentList enviorment;
    [SerializeField] public GameObject VsCanvas;
    [SerializeField] GameObject PostGameGM;
    [SerializeField] GameObject Scene3D;
     
    bool isGameOver;
    public bool IsGameOver{get{return isGameOver;}}
    public int PointsPerQuestion { get { return _pointsPerQuestion; } set { _pointsPerQuestion = value; } }
    public void PlayerLeftInTheMiddle() {StopAllCoroutines(); StartCoroutine(PlayerLeftInTheMiddleHelper());}
    public void StartQuestionProcess() { StartCoroutine(QuestionProcess());}
    public bool IsSessionOver {get{return IsSessionOverHelper();}}

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    void Start()
    {
        StartCoroutine(AssignmentMethod());
        
       // print("i am printing " + PhotonNetwork.CurrentRoom.CustomProperties["env"]);
    }
    private IEnumerator AssignmentMethod()
    {
       
        Assignment.instance.VSCanvas.SetActive(true);
        StartCoroutine(Saftey());
        Assignment.instance.VSCanvas.transform.GetChild(2).gameObject.SetActive(true);
        Assignment.instance.VSCanvas.transform.GetChild(3).gameObject.SetActive(false);
        enviorment = PhotonRoom.room.enviorment;
        yield return new WaitUntil(() => player1 != null && player2 != null);
        yield return new WaitUntil(() => EnviormentIsReady());
        if (IsNewRandomMode())
            Assignment.instance.QuizSubjectText.text = "איבוברע";
        StartCoroutine(VsScreen());
        if (PhotonNetwork.IsMasterClient)
        {
            thisComputerPlayer = player1;
            otherPlayer = player2;
        }
        else
        {
            thisComputerPlayer = player2;
            otherPlayer = player1;
        }
        
        host = FindObjectOfType<HostCharachter>();
        player1.myhelperManager.MakeAllHelpersNotActive();
        player2.myhelperManager.MakeAllHelpersNotActive();
        thisComputerPlayer.myhelperManager.MakeAllHelpersActive();
        player1.ResetParameters();
        player2.ResetParameters();
        thisComputerPlayer.myEmojiesGuiButton.ShowButton();
        thisComputerPlayer.myEmojiesGuiButton.MakeButtonNotAvaliable();
        player1.playeTimer.duration = timePerQuestion;
        player2.playeTimer.duration = timePerQuestion;
        // 
        PointsPerQuestion = 100 / totalNumOfQuestions;
        CalculationsManager.instance.ResetParameters();
        CalculationsManager.instance.LastGameTimePerQuestion = timePerQuestion;
        CalculationsManager.instance.LastGameTotalNumOfQuestions = totalNumOfQuestions;
        CalculationsManager.instance.LastGamePointsPerQuestion = PointsPerQuestion;

        UlpanScreenManager.instance.SetVsPic();
        UlpanScreenManager.instance.SetText("םיאבה םיכורב");

        Assignment.instance.TechMassage.text = "רבוחמ";
        //
    }


    private IEnumerator VsScreen() 
    {
        // start vs screen
        Assignment.instance.VSCanvas.transform.GetChild(2).gameObject.SetActive(false);
        Assignment.instance.VSCanvas.transform.GetChild(3).gameObject.SetActive(true);
        Assignment.instance.VsPlayerNameText[0].text = player1._myName;
        Assignment.instance.VsPlayerNameText[1].text = player2._myName;
        Assignment.instance.VSPlayerRankText[0].text = ProfileManager.GetRank(player1.PointsForScore).ToString();
        Assignment.instance.VSPlayerRankText[1].text = ProfileManager.GetRank(player2.PointsForScore).ToString();
        player1.transform.position = Assignment.instance.VsPlayersPosition[0].position;
        player2.transform.position = Assignment.instance.VsPlayersPosition[1].position;
        player1.GetComponentInChildren<Animator>().GetComponent<lookatactivecam>().enabled = false;
        player2.GetComponentInChildren<Animator>().GetComponent<lookatactivecam>().enabled = false;
        player1.GetComponentInChildren<Animator>().SetBool("Waving", true);
        yield return new WaitForSecondsRealtime(0.5f);
        player2.GetComponentInChildren<Animator>().SetBool("MirrorPushButton", true);
        player2.GetComponentInChildren<Animator>().SetBool("Waving", true);
        player2.GetComponentInChildren<Animator>().speed =0.85f;
        yield return new WaitForSecondsRealtime(2.5f);
        //waiting for other player
        thisComputerPlayer.myPhotonPlayer.SetReadyToStartGame();
        yield return new WaitUntil(() => otherPlayer.myPhotonPlayer.ReadyToStartGame);
        //set players for game
       
        player2.GetComponentInChildren<Animator>().SetBool("MirrorPushButton", false);
        player2.GetComponentInChildren<Animator>().speed = 1f;
        player1.transform.position = Assignment.instance.PlayersPosition[0].position;
        player2.transform.position = Assignment.instance.PlayersPosition[1].position;
        player1.GetComponentInChildren<Animator>().GetComponent<lookatactivecam>().enabled = true;
        player2.GetComponentInChildren<Animator>().GetComponent<lookatactivecam>().enabled = true;

        // turn vs screen off and start game
        Assignment.instance.VSCanvas.SetActive(false);
        Scene3D.SetActive(false);
        StartCoroutine(Opening());
    }

    IEnumerator Saftey() 
    {
        yield return new WaitForSecondsRealtime(15);
        if (Assignment.instance.VSCanvas.activeInHierarchy) 
        {
            PlayerPrefs.SetInt("AutoConnectAndSearch", 0);
            Destroy(PhotonRoom.room.gameObject);
            PhotonNetwork.Disconnect();
            while (PhotonNetwork.IsConnected)
                yield return null;
            Destroy(GameProcess.instance.gameObject);
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator Opening()
    {
        UI_Effects.instance.PlayStartEffect(1);
        UI_Effects.instance.PlayStartEffect(0);
        SoundManager.instance.PlayMusic(0);
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.Clapping);
        Cameras.instance.SelectCamera(4);
         
       
        yield return new WaitForSecondsRealtime(2f);
        host.talking(host.GetComponent<NarrationHolder>().Opening);
        
        player1.GetComponentInChildren<Animator>().SetBool("Waving", false);
        player2.GetComponentInChildren<Animator>().SetBool("Waving", false);
        player1.Dancing(20);
        player2.Dancing(20);
        yield return new WaitUntil(() => !host.IsTalkingInProgress);
        Cameras.instance.SelectCamera(0);
        thisComputerPlayer.myEmojiesGuiButton.MakeButtonNotAvaliable();
        LightsController.instance.LightsOn();
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.Clapping);

       
        //---CountDownSession

        UIManager.instance.startCountDown(3);
         
       
        EveryBodyDance(3);
        yield return new WaitForSecondsRealtime(1);
        
        Cameras.instance.SelectCamera(1);
        yield return new WaitForSecondsRealtime(1);
        Cameras.instance.SelectCamera(3);
        yield return new WaitForSecondsRealtime(1);
        LightsController.instance.LightsOff();
        // end
        GameProcess.instance.LoadQuestion();
    }
    private IEnumerator QuestionProcess()
    {
        if (GameProcess.instance.currentQuestionNumber > 3)
        {
            SoundManager.instance.PlaySoundEffect(SoundEffectsList.fiftyfifty);
            if(GameProcess.instance.currentQuestionNumber == 4)
                SoundManager.instance.PlayMusic(1);
        }
       

        player1.ResetParameters();
        player2.ResetParameters();
        host.talking(host.GetComponent<NarrationHolder>().questionProcess);
        yield return new WaitForSecondsRealtime(0.4f);
        UIManager.instance.ShowQuestionNumber();
        yield return new WaitUntil(() => !host.IsTalkingInProgress);
        player1.GetComponentInChildren<Animator>().transform.localPosition = new Vector3(0, 0, 0);
        player2.GetComponentInChildren<Animator>().transform.localPosition = new Vector3(0, 0, 0);
        UIManager.instance.HideQuestionAndPrizePannel();
        UI_Effects.instance.MakeAllAnsersNotGray();
        Cameras.instance.SelectCamera(0);
        player1.ThinkAnimation(true);
        player2.ThinkAnimation(true);
        yield return new WaitUntil(() => UIManager.instance.allAnswersAreDisplayed);
        
        thisComputerPlayer.myhelperManager.MakeAllHelpersNotUsed();
        thisComputerPlayer.myhelperManager.MakeHelpersInteract();
        thisComputerPlayer.myhelperManager.CheckAllHelpersAmmount();
        thisComputerPlayer.myPhotonPlayer.SetReadyToStartTimer();
        yield return new WaitUntil(() => otherPlayer.myPhotonPlayer.ReadyToStartTimer);
        player1.myPhotonPlayer.ReadyToStartTimer = false;
        player2.myPhotonPlayer.ReadyToStartTimer = false;
        // Assignment.instance.TechMassage.text = "רבוחמ";
        player1.playeTimer.StartTimer();
        player2.playeTimer.StartTimer();
        StartCoroutine(thisComputerPlayer.TimerProcessThisPlayer());
        StartCoroutine(otherPlayer.TimerProcessOtherPlayer());
        if (PhotonRoom.room.IsSinglePlayer)
            player2.StartPlayerAI();
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.Gong);
        UIManager.instance.MakeAnswersInteractable();
        yield return new WaitUntil(() => IsSessionOver);
        //player1.playeTimer.PauseTimer();
        //player2.playeTimer.PauseTimer();
        thisComputerPlayer.myhelperManager.MakeHelpersNotInteract();
        yield return new WaitForSecondsRealtime(2.3f);
        StartCoroutine(AnswerResult());
    }
    private IEnumerator AnswerResult()
    {
        UIManager.instance.HideBigFaces();
        Player first = player1;
        Player second = player2;
        if (player1.playeTimer.GetTimerValue() > player2.playeTimer.GetTimerValue())
        {
            first = player2;
            second = player1;
        }
         
        if (first.CurrentAnswer > 0) 
        {
            first.ShowOrHideSign(true);
            yield return new WaitForSecondsRealtime(1.2f);
            first.ShowMyFaceOnUiAnswer();
            yield return new WaitForSecondsRealtime(0.9f);
        }
        if (second.CurrentAnswer > 0)
        { 
            second.ShowOrHideSign(true);
            yield return new WaitForSecondsRealtime(1.2f);
            second.ShowMyFaceOnUiAnswer();
            yield return new WaitForSecondsRealtime(1.2f);
        }
        if(second.CurrentAnswer ==0 && first.CurrentAnswer == 0)
            yield return new WaitForSecondsRealtime(1f);

        SoundManager.instance.PlaySoundEffect(SoundEffectsList.Jocker);
        StartCoroutine(UIManager.instance.CorrectAnswer(GameProcess.instance.question.CorrectAnswer, ""));
        yield return new WaitForSecondsRealtime(1.5f);
        UIManager.instance.lozengePanel.SetActive(false);
        player1.ShowOrHideSign(false);
        player2.ShowOrHideSign(false);
        if (GameProcess.instance.question.IsAnswerCorrect(player1.CurrentAnswer))
            StartCoroutine(player1.CorrectAnswer());
        else
            StartCoroutine(player1.WrongAnswer());
        yield return new WaitForSecondsRealtime(0.5f);
        if (GameProcess.instance.question.IsAnswerCorrect(player2.CurrentAnswer))
            StartCoroutine(player2.CorrectAnswer());
        else
            StartCoroutine(player2.WrongAnswer());
        yield return new WaitForSecondsRealtime(2f);
        //Debug.Log(GameProcess.instance.currentQuestionNumber);
        if (GameProcess.instance.currentQuestionNumber >= totalNumOfQuestions)
            CheckIfGameOver();
        else
        GameProcess.instance.LoadQuestion();
    }
    private bool IsSessionOverHelper()
    {
        if (
            (player1.CurrentAnswer != 0 && player2.CurrentAnswer != 0) // if both of them answered
            || (player1.IsTimeOver() && player2.CurrentAnswer != 0)// if player1 have no time and player2 answered
            || (player2.IsTimeOver() && player1.CurrentAnswer != 0)// if player2 have no time and player1 answered
            || (player1.IsTimeOver() && player2.IsTimeOver())// if both of them have no time
            )
            return true;
        else
            return false;
    }
    private void CheckIfGameOver()
    {
        if (player1.TotalScore > player2.TotalScore)
            StartCoroutine(GameOver(player1));
        else if ((player1.TotalScore < player2.TotalScore))
            StartCoroutine(GameOver(player2));
        else
            GameProcess.instance.LoadQuestion();
    }

   

    private IEnumerator GameOver(Player winner)
    {
        isGameOver = true;
        UlpanScreenManager.instance.SetDefultPic();
        UlpanScreenManager.instance.SetText("");
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.GameOver);
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.Clapping);
        LightsController.instance.LightsOn();
        EveryBodyDance(2.5f);
        host.talking(host.GetComponent<NarrationHolder>().GameOver);
        yield return new WaitUntil(() => !host.IsTalkingInProgress);
        // UIManager.instance.ShowTotalPrizeWin("500");
        if (winner != null)
        {
            if (thisComputerPlayer == winner)
            {
                CalculationsManager.instance.AmIWinner = true;
        #if (!UNITY_EDITOR && !DEVELOPMENT_BUILD)
                saveWinnersJS(thisComputerPlayer._myName.Replace(" ",";"),otherPlayer._myName.Replace(" ", ";"));
                pushWinnersJS();
        #endif

            }
            else if (PhotonRoom.FindObjectOfType<PhotonRoom>().IsSinglePlayer)
            {
                #if (!UNITY_EDITOR && !DEVELOPMENT_BUILD)
                saveWinnersJS(otherPlayer._myName.Replace(" ", ";"), thisComputerPlayer._myName.Replace(" ", ";"));
                pushWinnersJS();
                #endif
            }


            Cameras.instance.SelectCamera(winner);
            winner.WinGame();
            yield return new WaitForSecondsRealtime(2.5f);
        }

        LightsController.instance.LightsOff();
        UIManager.instance.HideQuestionAndPrizePannel();
        host.talking(host.GetComponent<NarrationHolder>().Ending);
        yield return new WaitUntil(() => !host.IsTalkingInProgress);
        DisconnectPlayer();
    }
    private IEnumerator PlayerLeftInTheMiddleHelper()
    {
        thisComputerPlayer.GetComponentInChildren<Animator>().SetBool("Waiting", false);
        thisComputerPlayer.GetComponentInChildren<Animator>().SetBool("Think", false);
        thisComputerPlayer.GetComponentInChildren<Animator>().SetTrigger("Idle");
        yield return new WaitUntil(() => !host.IsTalkingInProgress);
        player1.playeTimer.PauseTimer();
        player2.playeTimer.PauseTimer();
        thisComputerPlayer.myhelperManager.MakeAllHelpersNotActive();
        UIManager.instance.CloseAllPanels();
        Cameras.instance.SelectCamera(2);
        host.GetComponentInChildren<Animator>().SetBool("Dancing", false);// currentli this is the only option/
                                                                          // switch to somthing better
        UIManager.instance.ShowMassageToUser("בזע ינשה דדומתמה");
        yield return new WaitForSecondsRealtime(2f);
        UIManager.instance.CloseAllPanels();
        host.talking(host.GetComponent<NarrationHolder>().TechnicalWin);
        yield return new WaitUntil(() => !host.IsTalkingInProgress);
        UIManager.instance.CloseAllPanels();
        LightsController.instance.LightsOn();
        Cameras.instance.SelectCamera(thisComputerPlayer);
        thisComputerPlayer.WinGame();
        CalculationsManager.instance.TechnicalWIn = true;
        yield return new WaitForSecondsRealtime(2.5f);
        UIManager.instance.HideQuestionAndPrizePannel();
        host.talking(host.GetComponent<NarrationHolder>().Ending);
        yield return new WaitUntil(() => !host.IsTalkingInProgress);
        DisconnectPlayer();
    }
     
    private void EveryBodyDance(float time) 
    {
        host.dancing(time);
        player1.Dancing(time);
        player2.Dancing(time);
    }
    private void DisconnectPlayer()
    {
        PlayerPrefs.SetString("LastRoomName", PhotonNetwork.CurrentRoom.Name);
        if(IsNewRandomMode())
            PlayerPrefs.SetInt("LastTopicPlayed",(int)EnviormentList.Random);
        else
            PlayerPrefs.SetInt("LastTopicPlayed", MultiPlayerQuestionRandomizer.instance.chosenEnviorment);
        Destroy(PhotonRoom.room.gameObject);
        StartCoroutine(DisconnectAndLoad());
    }
    private IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        Destroy(GameProcess.instance.gameObject);


        Initiate.Fade(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(4)), Color.black, 4f);
    }
    private bool EnviormentIsReady()
    {
        if (MultiPlayerQuestionRandomizer.instance.SetEnviorment(enviorment) < 0)
            return false;
        int chosenEnv = MultiPlayerQuestionRandomizer.instance.SetEnviorment(enviorment);
        EnviormentsManager.instance.ChooseEnviorment(chosenEnv);
        SetQuiz(chosenEnv);
        Assignment.instance.QuizSubjectText.text = GetSubjectText(chosenEnv);
        return true;
    }

    public bool IsNewRandomMode() 
    {
        byte b = 0;

        try {b=(byte)PhotonNetwork.CurrentRoom.CustomProperties["env"];} catch{ }
        return (PhotonRoom.room.IsSinglePlayer && PhotonRoom.room.enviorment == EnviormentList.Random) ||
            (!PhotonRoom.room.IsSinglePlayer && b== 255);
    }

    public void SetQuiz(int chosen)
    {
        GetComponent<QuizManager>().LoadQuestionList(chosen);
    }

    public  string GetSubjectText (int chosen)
    {
        if (chosen == EnviormentList.Bereshit.GetHashCode())
        {
             return "תישארב שמוח";
        }

        if (chosen == EnviormentList.ShmotVaikra.GetHashCode())
        {
            return "ארקיו-תומש ישמוח";
        }

        if (chosen == EnviormentList.BamidbarDvarim.GetHashCode())
        {
            return  "םירבד-רבדמב ישמוח";
        }

        if (chosen == EnviormentList.Tanak.GetHashCode())
        {
            return "םינושאר םיאיבנ";
        }

        if (chosen == EnviormentList.Avot.GetHashCode())
        {
            return "תובא יקרפ";
        }

        if (chosen == EnviormentList.Israel.GetHashCode())
        {
            return "לארשי ץרא";
        }

        if (chosen == EnviormentList.Mada.GetHashCode())
        {
            return "עדמו עבט";
        }
        if (chosen == EnviormentList.Tarbut.GetHashCode())
        {
            return "ץורעה תוינכות";
        }

        if (chosen == EnviormentList.RoshHashana.GetHashCode())
        {
            return "הנשה  שאר";
        }

        if (chosen == EnviormentList.Kippur.GetHashCode())
        {
            return "םירופיכה םוי";
        }

        if (chosen == EnviormentList.Sukkot.GetHashCode())
        {
            return "תוכוס";
        }

        if (chosen == EnviormentList.Shabat.GetHashCode())
        {
            return "ימלועה תבשה ןודיח";
        }

        if (chosen == EnviormentList.Hanuka.GetHashCode())
        {
            return "הכונח";
        }

        if (chosen == EnviormentList.Purim.GetHashCode())
        {
            return "םירופ";
        }

        if (chosen == EnviormentList.tfila.GetHashCode())
        {
            return "הליפת";
        }

        if (chosen == EnviormentList.mizvot.GetHashCode())
        {
            return "םיגהנמו תווצמ";
        }

        

        if (chosen == EnviormentList.shirim.GetHashCode())
        {
            return "םידלי יריש";
        }

        if (chosen == EnviormentList.numbers.GetHashCode())
        {
            return "תודהיב םירפסמ";
        }

        if (chosen == EnviormentList.israelHistory.GetHashCode())
        {
            return "תילארשי הירוטסה";
        }

        if (chosen == EnviormentList.PlacesInIsrael.GetHashCode())
        {
            return "ץראב תומוקמ";
        }

        if (chosen == EnviormentList.geography.GetHashCode())
        {
            return "היפרגואיג";
        }

        if (chosen == EnviormentList.Biology.GetHashCode())
        {
            return "היגולויב";
        }

        if (chosen == EnviormentList.food.GetHashCode())
        {
            return "לכוא";
        }

        else return "יללכ עדי";
    }
}
