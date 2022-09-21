using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BotConfiguration : MonoBehaviour
{
     [SerializeField] int _botSmarness;
    [SerializeField] string _BotName;
    [SerializeField] int _botPrefab;
    [SerializeField] public  int _botRank;
     public int BotSmartness { get{ if (_botSmarness < 1) return Random.Range(50, 90); else return _botSmarness; }set{_botSmarness = value;}}
    public int BotPrefab {get{return _botPrefab;}}
    public string BotName { get { return _BotName; } }

    private void Start()
    {
        if (_botPrefab > 5)
            _botPrefab = Random.Range(17, 30);
        else
            _botPrefab = Random.Range(0, 15);
    }
    public int BotPoints() 
    {

        return Mathf.Max(ProfileManager.FIRST_RANK_POINTS, ProfileManager.GetPointsByRank(_botRank)/2);
     }
}
