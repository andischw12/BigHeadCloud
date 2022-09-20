using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine.UI;
using Febucci.UI;
using UnityEngine.SceneManagement;



public class PostGame : MonoBehaviour
{
    int score;
    int speed;
    int correctAnswers;
    int wrongAnswers;
    int bonus;
    int rank;
    int gems;
    [SerializeField] ProgressBar PointsBar;
    [SerializeField] ProgressBar TimeBar;
    [SerializeField] ProgressBar CorrectAnsweresBar;
    [SerializeField] ProgressBar BonusBar;
    [SerializeField] TextMeshProUGUI GemsAmmount;
    [SerializeField] TextMeshProUGUI UserName;
    [SerializeField] TextMeshProUGUI Rank;
    [SerializeField] TextMeshProUGUI UserGemsAmmount;
    [SerializeField] TextMeshProUGUI UserRank;
    [SerializeField] TextMeshProUGUI UserPoints;

    [SerializeField] Image Gem;
    [SerializeField] RenderTexture  UserFace;
    [SerializeField] GameObject RematchAndHomeButton;
 
    // Start is called before the first frame update
    public void Start()
    {


        //CalculateInfo();
        FindObjectOfType<KidAvatarSelector>().SetActiveAvatarLook(FamilyManager.instance.GetAvatarForActiveKid());
       FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Camera>().targetTexture = UserFace;
        try { FindObjectOfType<KidAvatarSelector>().SetActiveAvatarLook(FindObjectOfType<FamilyManager>().GetAvatarForActiveKid()); } catch { }
        CorrectAnsweresBar.currentPercent = 0;
        SoundManager.instance.PlayMenuMusic();
        UserGemsAmmount.text = FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Gems).ToString();
        UserName.text = FamilyManager.instance.GetActiveKidFullName();
        StartCoroutine(PostGameProgress());
        RematchAndHomeButton.SetActive(false);
        FindObjectOfType<ProfileManager>().SetValues(FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Points));
       


    }


    IEnumerator PostGameProgress()
    {
        try { UserGemsAmmount.GetComponentInParent<Animator>().enabled = false; } catch { }
        CalculationsManager.instance.PostGame = true;
        int thisRoundGems = 0;
        int NewPoints = 0;
        float roomMulti = StudioButtonManager.BonusMulti[PlayerPrefs.GetInt("LastEvPlayed")];
        if (!CalculationsManager.instance.TechnicalWIn)
        {
            if (CalculationsManager.instance.GetCaluclatedScore() > 1)
            {

                FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetBool("Waiting", true);
                yield return new WaitForSecondsRealtime(0.85f);
                FindObjectOfType<WindowManager>().OpenPanel(0);
                PointsBar.isOn = true;
                SoundManager.instance.PlaySoundEffect(SoundEffectsList.Calculation);
                yield return new WaitUntil(() => PointsBar.currentPercent >= CalculationsManager.instance.GetCaluclatedScore() || PointsBar.currentPercent >= 100);
                SoundManager.instance.StopSoundEffect();
                SoundManager.instance.PlaySoundEffect(SoundEffectsList.CalculationEnd);
                PointsBar.isOn = false;

            }
            if (CalculationsManager.instance.GetCaluclatedSpeed() > 1)
            {
                yield return new WaitForSecondsRealtime(0.85f);
                TimeBar.isOn = true;
                SoundManager.instance.PlaySoundEffect(SoundEffectsList.Calculation);
                yield return new WaitUntil(() => TimeBar.currentPercent >= CalculationsManager.instance.GetCaluclatedSpeed() || TimeBar.currentPercent >= 100);
                SoundManager.instance.StopSoundEffect();
                SoundManager.instance.PlaySoundEffect(SoundEffectsList.CalculationEnd);
                TimeBar.isOn = false;
            }



            if (CalculationsManager.instance.LastGameCorrectAnswers > 0)
            {
                yield return new WaitForSecondsRealtime(0.85f);
                CorrectAnsweresBar.isOn = true;
                SoundManager.instance.PlaySoundEffect(SoundEffectsList.Calculation);
                yield return new WaitUntil(() => CorrectAnsweresBar.currentPercent >= CalculationsManager.instance.GetCaluclatedCorrectAnswers() || CorrectAnsweresBar.currentPercent >= 100);
                SoundManager.instance.StopSoundEffect();
                SoundManager.instance.PlaySoundEffect(SoundEffectsList.CalculationEnd);
                CorrectAnsweresBar.isOn = false;
            }



            if (CalculationsManager.instance.AmIWinner)
            {
                FamilyManager.instance.SetActiveKidInfoValue(UserArrayEnum.Wins, FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Wins) + 1); //update win
                yield return new WaitForSecondsRealtime(0.85f);
                BonusBar.isOn = true;
                SoundManager.instance.PlaySoundEffect(SoundEffectsList.Calculation);
                yield return new WaitUntil(() => BonusBar.currentPercent >= CalculationsManager.instance.GetCaluclatedBonus() || BonusBar.currentPercent >= 100);
                SoundManager.instance.StopSoundEffect();
                SoundManager.instance.PlaySoundEffect(SoundEffectsList.CalculationEnd);
                BonusBar.isOn = false;
            }
            else
            {
                FamilyManager.instance.SetActiveKidInfoValue(UserArrayEnum.Lose, FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Lose) + 1); //update lose
            }

            
           
            thisRoundGems = (int)(CalculationsManager.instance.CalculateGems()*roomMulti/100);
            print("old gems is: " + CalculationsManager.instance.CalculateGems() + " new gems is: " + thisRoundGems);

            NewPoints = (int)(FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Points) + CalculationsManager.instance.GetCaluclatedBonus()*roomMulti / 100);
            print("old points is: " + FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Points) + CalculationsManager.instance.GetCaluclatedBonus() + " new points is: " + NewPoints);
            yield return new WaitForSecondsRealtime(1f);

        }

        else
        {
            NewPoints = FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Points) + Random.Range(50, 150);
            thisRoundGems = Random.Range(350, 500);
            FamilyManager.instance.SetActiveKidInfoValue(UserArrayEnum.Wins, FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Wins) + 1); //update tech win
        }


        //updateScore

        FamilyManager.instance.SetActiveKidInfoValue(UserArrayEnum.Gems, FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Gems) + thisRoundGems);

        FamilyManager.instance.SetActiveKidInfoValue(UserArrayEnum.PlayTime, FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.PlayTime) + CalculationsManager.instance.GetPlayTime()); //update Score

        //points
        UserPoints.text = "+ " + CalculationsManager.instance.GetCaluclatedBonus();
        FindObjectOfType<WindowManager>().OpenPanel(1);
        FindObjectOfType<StarsEffect>().Play();
        FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetBool("Waiting", false);
        FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetTrigger("Happy");
       
        yield return new WaitForSecondsRealtime(2f);
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.reavelAnswer);
        bool tmp = FindObjectOfType<ProfileManager>().IsNewRank(NewPoints);
        if (tmp) 
        {
            FindObjectOfType<ProfileManager>().AnimateSliderOverTime(0.99f,2f);
            yield return new WaitForSecondsRealtime(1f);
        }
            

        else
        {
            FindObjectOfType<ProfileManager>().AnimateSliderOverTime(FindObjectOfType<ProfileManager>().GetSliderState(NewPoints),2);
            yield return new WaitForSecondsRealtime(1.3f);
        }
           

        //new rank
        if(tmp)
        {
            FindObjectOfType<WindowManager>().OpenPanel(2);
            Rank.text = ProfileManager.GetRank(NewPoints).ToString();
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetTrigger("Happy");
            FindObjectOfType<StarsEffect>().Play();
            SoundManager.instance.PlaySoundEffect(SoundEffectsList.WinGame);
            FindObjectOfType<ProfileManager>().SetUserRank(NewPoints);
            //print("next line is resetingggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg");
            FindObjectOfType<ProfileManager>().ResetSlider();
            yield return new WaitForSecondsRealtime(1f);
            FindObjectOfType<ProfileManager>().AnimateSliderOverTime(FindObjectOfType<ProfileManager>().GetSliderState(NewPoints),1);
            yield return new WaitForSecondsRealtime(2f);

        }
        FamilyManager.instance.SetActiveKidInfoValue(UserArrayEnum.Points, NewPoints);




        //gems
        GemsAmmount.text = "<incr>" + "<rainb>" + "+" + thisRoundGems.ToString();
        FindObjectOfType<WindowManager>().OpenPanel(3);
        FindObjectOfType<StarsEffect>().Play();
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.GameOver);
        UserGemsAmmount.text = "<incr>" + "<rainb>" + FamilyManager.instance.GetInfoValForActiveKid(UserArrayEnum.Gems);
        FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetTrigger("Happy");
        yield return new WaitForSecondsRealtime(2.5f);
        //

       

        // end

        RematchAndHomeButton.SetActive(true);
        //
        /*
        if (FindObjectOfType<ProfileManager>().IsNewRank(NewPoints)) 
        {
            FindObjectOfType<ProfileManager>().AnimateSliderOverTime(1f);
            yield return new WaitForSecondsRealtime(1f);
            FindObjectOfType<WindowManager>().OpenPanel(2);
            Rank.text = ProfileManager.GetRank(NewPoints).ToString();
            FindObjectsOfType<StarsEffect>()[1].Play();
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetTrigger("Happy");
        }
        else
        {
            FindObjectOfType<ProfileManager>().AnimateSliderOverTime(FindObjectOfType<ProfileManager>().GetSliderState(NewPoints));
        }
        yield return new WaitForSecondsRealtime(1f);
        FindObjectsOfType<StarsEffect>()[0].Play();
        yield return new WaitForSecondsRealtime(1f);

        */



        //Initiate.Fade(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1)), Color.black, 4f);
    }


    public void OnHomeButtonClick() 
    {
        PhotonPlayerManagerBot.LastBotChosen = -1;//reset bot
        PlayerPrefs.SetString("LastRoomName", "");
        Initiate.Fade(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1)), Color.black, 4f);
    }

    public void OnReMatchButtonClick()
    {

        Initiate.Fade(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1)), Color.black, 4f);
    }

}
