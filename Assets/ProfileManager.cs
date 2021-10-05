using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public static readonly int FIRST_RANK_POINTS = 149; 
    [SerializeField] TextMeshProUGUI RankText;
    [SerializeField] public Slider Slider;
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Title;


    // Start is called before the first frame update


    void Start()
    {

      
        //print(GetRank(600));
        print(GetPointsByRank(1));
    }


     public void SetValues(int points) 
    {

        RankText.text = GetRank(points).ToString(); 
        try 
        {
            Slider.value = GetSliderState(points);
            SetTitleByRank(GetRank(points));
        } 
        catch { }
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
        return (int)Mathf.Sqrt((float)points/150);
    }

    int GetPointsByRank(int rank) 
    {
        return rank * rank * 150 ;
    }

    public float GetSliderState(int points) 
    {
        return ((points - GetPointsByRank(GetRankByPoints(points))) / (float)(GetPointsByRank(GetRankByPoints(points)+1) - GetPointsByRank(GetRankByPoints(points))));
    }

    public void AnimateSliderOverTime(float newVal) 
    {
        StartCoroutine(AnimateSliderOverTimeHelper(Slider, newVal, 2f));
    }

    public bool IsNewRank(int newPoints) 
    {
        int oldPoints = FamilyManager.instance.GetInfoValForActiveKid(UserInfoList.Points);
        return newPoints >= GetPointsByRank(GetRankByPoints(oldPoints) + 1);
    }

    IEnumerator AnimateSliderOverTimeHelper(Slider _slider, float newval, float seconds)
    {
       
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            _slider.value = Mathf.Lerp(_slider.value, newval, lerpValue);
            yield return null;
        }
    }

    // מתחיל חובבן מומחה מקצוען ידען גאון מלך קיסר מתקדם בר-מוח תלמיד-חכם ינוקא עילוי עוקר-הרים 
    void SetTitleByRank(int rank) 
    {
        
        switch (rank) 
        {
            case var expression when expression < 4:
                Title.text = "ליחתמ";
                break;

            default:
                Title.text = "םכח דימלת";
                break;
        }
       
    }


}
