using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public static readonly int FIRST_RANK_POINTS = 50; 
    [SerializeField] TextMeshProUGUI RankText;
    [SerializeField] public Slider Slider;
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Title;


    // Start is called before the first frame update


    void Start()
    {

      
        //print(GetRank(600));
        print(GetRankByPoints(400));
    }


     public void SetValues(int points) 
    {

        SetUserRank(points);
        try 
        {
            AnimateSliderOverTime(GetSliderState(points),2f);
            SetTitleByRank(GetRank(points)); //title is beginer ..etc
        } 
        catch { print("SetValues func couldnt find"); }
    }

    public void SetUserRank(int points) 
    {
        RankText.text = GetRank(points).ToString();
    }


    public int GetRankFromServer() 
    {
        return GetRank(FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Points));  
    }

    public int GetRank(int points)
    {
        return GetRankByPoints(points);  
    }

    int GetRankByPoints(int points) // for inner calculation - do not use from outside
    {
        return (int)Mathf.Sqrt((float)points/FIRST_RANK_POINTS);
    }

    int GetPointsByRank(int rank) 
    {
        return rank * rank * FIRST_RANK_POINTS;
    }

    public float GetSliderState(int points) 
    {
        return ((points - GetPointsByRank(GetRankByPoints(points))) / (float)(GetPointsByRank(GetRankByPoints(points)+1) - GetPointsByRank(GetRankByPoints(points))));
    }

    public void AnimateSliderOverTime(float newVal,float sec) 
    {
        StopCoroutine(nameof(AnimateSliderOverTimeHelper));
        StartCoroutine(AnimateSliderOverTimeHelper(Slider, newVal, sec));
    }

    public bool IsNewRank(int newPoints) 
    {
        int oldPoints = FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Points);
        return newPoints >= GetPointsByRank(GetRankByPoints(oldPoints) + 1);
    }

    public void ResetSlider() 
    {
        Slider.value = 0.0f;
    }


    IEnumerator AnimateSliderOverTimeHelper(Slider _slider, float newval, float seconds)
    {
       
        float animationTime = 0f;
        while (animationTime < seconds && _slider.value!= newval)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            _slider.value = Mathf.Lerp(_slider.value, newval, lerpValue);
            print("working");
            yield return null;
        }
        
    }

    // מתחיל חובבן מומחה מקצוען ידען גאון מלך קיסר מתקדם בר-מוח תלמיד-חכם ינוקא עילוי עוקר-הרים 

    //
   
    void SetTitleByRank(int rank) 
    {
        
        switch (rank) 
        {
            case var expression when expression <= 2:
                Title.text = "אקוני";
                break;
            case var expression when expression <= 4:
                Title.text = "ףירח";
                break;
            case var expression when expression <= 9:
                Title.text = "יוליע";
                break;
            case var expression when expression <= 19:
                Title.text = "םכח דימלת";
                break;
            case var expression when expression <= 29:
                Title.text = "ןנברמ אברוצ";
                break;
            case var expression when expression <= 39:
                Title.text = "ןואג";
                break;
            case var expression when expression <= 49:
                Title.text = "ןמאנ עטנ";
                break;
            case var expression when expression <= 59:
                Title.text = "םירה רקוע";
                break;
            case var expression when expression <= 69:
                Title.text = "יניס";
                break;
            case var expression when expression <= 79:
                Title.text = "ארתאד ארמ";
                break;
            case var expression when expression <= 99:
                Title.text = "אתולג שיר";
                break;
            default:
                Title.text = "ןרמ";
                break;
        }
       
    }


}
