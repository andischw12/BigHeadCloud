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
        FindObjectOfType<KidAvatarSelector>().SetAvatar(FamilyManager.instance.GetAvatarForActiveKid());
       FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Camera>().targetTexture = UserFace;
        try { FindObjectOfType<KidAvatarSelector>().SetAvatar(FindObjectOfType<FamilyManager>().GetAvatarForActiveKid()); } catch { }
        CorrectAnsweresBar.currentPercent = 0;
        SoundManager.instance.PlayMenuMusic();
        UserGemsAmmount.text = FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Gems).ToString();
        UserName.text = FamilyManager.instance.GetActiveKidFullName();
        StartCoroutine(PostGameProgress());
        RematchAndHomeButton.SetActive(false);
        FindObjectOfType<ProfileManager>().SetValues(FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Points));
       


    }


    IEnumerator PostGameProgress()
    {
        UserGemsAmmount.GetComponentInParent<Animator>().enabled = false;
        CalculationsManager.instance.PostGame = true;
        int thisRoundGems = 0;
        int NewPoints = 0;
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
                FamilyManager.instance.SetActiveKidInfoValue(UserInfoList.Wins, FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Wins) + 1); //update win
                yield return new WaitForSecondsRealtime(0.85f);
                BonusBar.isOn = true;
                SoundManager.instance.PlaySoundEffect(SoundEffectsList.Calculation);
                yield return new WaitUntil(() => BonusBar.currentPercent > CalculationsManager.instance.GetCaluclatedBonus() || BonusBar.currentPercent >= 100);
                SoundManager.instance.StopSoundEffect();
                SoundManager.instance.PlaySoundEffect(SoundEffectsList.CalculationEnd);
                BonusBar.isOn = false;
            }
            else
            {
                FamilyManager.instance.SetActiveKidInfoValue(UserInfoList.Lose, FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Lose) + 1); //update lose
            }

            
            FamilyManager.instance.SetActiveKidInfoValue(UserInfoList.Points, FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Points) + CalculationsManager.instance.GetCaluclatedBonus()); //update Score
            FamilyManager.instance.SetActiveKidInfoValue(UserInfoList.PlayTime, FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.PlayTime) + CalculationsManager.instance.GetPlayTime()); //update Score
            thisRoundGems = CalculationsManager.instance.CalculateGems();
            NewPoints = FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Points) + CalculationsManager.instance.GetCaluclatedBonus();
            yield return new WaitForSecondsRealtime(1f);

        }

        else
        {
            NewPoints = FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Points) + Random.Range(50, 150);
            thisRoundGems = Random.Range(350, 500);
            FamilyManager.instance.SetActiveKidInfoValue(UserInfoList.Wins, FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Wins) + 1); //update tech win
        }

        //points
         UserPoints.text = "+ " + NewPoints.ToString();
        FindObjectOfType<WindowManager>().OpenPanel(1);
        yield return new WaitForSecondsRealtime(0.3f);
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.reavelAnswer);
        if(!FindObjectOfType<ProfileManager>().IsNewRank(NewPoints))
            FindObjectOfType<ProfileManager>().AnimateSliderOverTime(FindObjectOfType<ProfileManager>().GetSliderState(NewPoints));
        else
            FindObjectOfType<ProfileManager>().AnimateSliderOverTime(1f);
        yield return new WaitForSecondsRealtime(1f);
        FindObjectOfType<StarsEffect>().Play();
        FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetBool("Waiting", false);
        FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetTrigger("Happy");
        yield return new WaitForSecondsRealtime(2.5f);

        //new rank
        if(FindObjectOfType<ProfileManager>().IsNewRank(NewPoints))
        {
            FindObjectOfType<WindowManager>().OpenPanel(2);
            Rank.text = FindObjectOfType<ProfileManager>().GetRank(NewPoints).ToString();
            FindObjectOfType<KidAvatarSelector>().GetComponentInChildren<Animator>().SetTrigger("Happy");
            FindObjectOfType<StarsEffect>().Play();
            SoundManager.instance.PlaySoundEffect(SoundEffectsList.WinGame);
            FindObjectOfType<ProfileManager>().SetValues(NewPoints);
            yield return new WaitForSecondsRealtime(3f);

        }




        //gems
        GemsAmmount.text = "<incr>" + "<rainb>" + "+" + thisRoundGems.ToString();
        FindObjectOfType<WindowManager>().OpenPanel(3);
        FindObjectOfType<StarsEffect>().Play();
        SoundManager.instance.PlaySoundEffect(SoundEffectsList.GameOver);
        UserGemsAmmount.text = "<incr>" + "<rainb>" + FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Gems);
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
            Rank.text = FindObjectOfType<ProfileManager>().GetRank(NewPoints).ToString();
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
        PlayerPrefs.SetString("LastRoomName", "");
        Initiate.Fade(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1)), Color.black, 4f);
    }

    public void OnReMatchButtonClick()
    {
        Initiate.Fade(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(1)), Color.black, 4f);
    }

}
