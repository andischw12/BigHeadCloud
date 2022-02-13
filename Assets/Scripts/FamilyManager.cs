using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

//public enum itemsCategory { Signs, Hats, Shirts, Pants, Shoes };
public enum UserInfoList { IsActive, Number, Gender, Gems, Points, Wins, Lose, CorrectAnswers, JockerUse, FreezeUse, _5050Use, PlayTime, GemsSpent, Rank, };
public enum AvatarInfoList { Hats, Glasses, Signates, Capes, ChestGM, ChestMat, LegsGm, LegsMat, FeetGM, FeetMat, AvatarPrefab }


public class KidUser
{
    //string firstName;
    // string lastName;
    [DllImport("__Internal")]
    public static extern string loadDataJS(int player);

    [DllImport("__Internal")]
    public static extern void saveDataJS(string data, int player, int Points, int ShabbatPoints, int HanukkaPoints);

    [DllImport("__Internal")]
    public static extern string getLastNameJS();

    public static string sendJson = "";
    public static string getJson = "";
    public string[] StoreInfo;

    //variables
    public string UserName;
    public int shabbatPoints;
    public int hanukkaPoints;

    public int[] myInfo = new int[Enum.GetNames(typeof(UserInfoList)).Length];
    public int[] myAvatar = new int[Enum.GetNames(typeof(AvatarInfoList)).Length];
    public int[,] mystore = new int[Enum.GetNames(typeof(AvatarInfoList)).Length, 100];
    //properties
    public int ShabbatPoints { get { return shabbatPoints; } set { shabbatPoints = value; } }
    public int HanukkaPoints { get { return hanukkaPoints; } set { hanukkaPoints = value; } }
    public int PurimPoints { get { return hanukkaPoints; } set { hanukkaPoints = value; } }
    public int[,] Store { get { return mystore; } set { mystore = Store; } }
    public string FirstName { get { return UserName; } set { UserName = value; } }
    public int[] FullAvatar { get { return myAvatar; } set { myAvatar = value; } }
    public bool IsActive
    {
        get { return myInfo[UserInfoList.IsActive.GetHashCode()] == 1; }
        set { if (value) myInfo[UserInfoList.IsActive.GetHashCode()] = 1; else myInfo[UserInfoList.IsActive.GetHashCode()] = 0; }
    }
    // constructor
    public KidUser(int number) 
    {
        myInfo[UserInfoList.Number.GetHashCode()] = number;
    }
    public int GetInfoVal(UserInfoList info) { return myInfo[info.GetHashCode()]; }
    public void SetInfoVal(UserInfoList info, int val) { myInfo[info.GetHashCode()] = val; }

    public void ResetAllInfo() 
    {
        UserName= "";
        myInfo = new int[Enum.GetNames(typeof(UserInfoList)).Length];
        myAvatar = new int[Enum.GetNames(typeof(AvatarInfoList)).Length];
        mystore = new int[Enum.GetNames(typeof(AvatarInfoList)).Length, 100];
    }

    //Ishay functions: Saveing data & getting data
    public string JsonPrefer()
    {
        //Entering the data from Mystore into StoreInfo - for storage on server
        Array.Resize(ref StoreInfo, 0);
        for (int i = 0; i < mystore.GetLength(0); i++)
        {
            for (int j = 0; j < mystore.GetLength(1); j++)
            {
                if (mystore[i, j] == 1)
                {
                    Array.Resize(ref StoreInfo, StoreInfo.Length + 1);
                    StoreInfo[StoreInfo.Length - 1] = i.ToString() + "-" + j.ToString();
                }
            }
        }
        //Create JSON for server storage
        return JsonUtility.ToJson(this);
    }

    //Update variables from JSON data (from the server)
    public void loadParametersFromData(string jsonString)
    {
        JsonUtility.FromJsonOverwrite(jsonString, this);
        MystoreUpdadeFromStoreInfoData();
    }

    //Update Mystore by entering the data from StoreInfo (from JSON)
    public void MystoreUpdadeFromStoreInfoData()
    {
        for (int i = 0; i < StoreInfo.Length; i++)
        {
            string[] itemInfo = StoreInfo[i].Split('-');
            int XItem = Convert.ToInt32(itemInfo[0]);
            int YItem = Convert.ToInt32(itemInfo[1]);
            mystore[XItem, YItem] = 1;
            //Debug.Log(Mystore[XItem,YItem]);
           // Debug.Log(XItem.ToString() + "/" + YItem.ToString());
        }
        Array.Resize(ref StoreInfo, 0);
    }

    //Saving all data of player (by player-num) on serverש
    public void savePlayerData()
    {


#if (!UNITY_EDITOR && !DEVELOPMENT_BUILD)
        UserName = FirstName.Replace("?","").Replace(":","").Replace("(","").Replace(")","").Replace("{","").Replace("}","").Replace("[","").Replace("]","");
        int player = GetInfoVal(UserInfoList.Number);
        int points = GetInfoVal(UserInfoList.Points);
        //int shabbatPointsToSave = GetShabbatPoints();
        sendJson = JsonPrefer();
        saveDataJS(sendJson, player,points,ShabbatPoints, HanukkaPoints);
#endif

    }

    //Getting all data of player (by player-num) from server
    public void loadPlayerData()
    {
#if (!UNITY_EDITOR && !DEVELOPMENT_BUILD)
 
        int player = GetInfoVal(UserInfoList.Number); ;
        getJson = loadDataJS(player);
        if (getJson != null && getJson != "" && getJson != "undefined")
        {
            loadParametersFromData(getJson);
            FirstName = UserName.Replace("?","");
        }
#endif

    }
}


public class FamilyManager : MonoBehaviour
{
    ///
    [DllImport("__Internal")]
    public static extern int getPlayersCountJS();
    int countPlayer = 0;

    public static FamilyManager instance;
    public KidUser[] _kidsUserArr = new KidUser[4];
    KidUser ActiveKid = null;
    public bool UseServer;

    void Start()
    {
       // InvokeRepeating("OutputTime", 5f, 5f);  //1s delay, repeat every 1s
    }

    private void Update()
    {
         /*
        if (InternetReachabilityVerifier.Instance.status == InternetReachabilityVerifier.Status.Offline && SceneManager.GetActiveScene().buildIndex!=5)
        {
            SceneManager.LoadScene(5);
            PhotonNetwork.Disconnect();
            AudioListener.pause = true;
            
        }
         */  
        
    }
    void OutputTime()
    {
        //Debug.Log(Time.time);
        SaveActiveKidInfo();
    }

    private void SaveActiveKidInfo()
    {
        if(ActiveKid != null)
        ActiveKid.savePlayerData();

    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        CreateEmptyUsers();
    }

    public void SetActiveKid(int player)
    {
        ActiveKid = _kidsUserArr[player];

    }
    public int GetStoreItemState(AvatarInfoList item, int collum)
    {
        return ActiveKid.Store[item.GetHashCode(), collum];
    }
    public void SetStoreItemState(AvatarInfoList item, int collum, int Val)
    {
        ActiveKid.Store[item.GetHashCode(), collum] = Val;
        SaveActiveKidInfo();
    }
    public void SetActiveKidInfoValue(UserInfoList info, int newVal)
    {
        ActiveKid.SetInfoVal(info, newVal);
        SaveActiveKidInfo();
    }
    public void SetAvatarForActiveKid(int[] arr)
    {
        ActiveKid.FullAvatar = arr;
        SaveActiveKidInfo();
    }
    public int[] GetAvatarForActiveKid()
    {
        return ActiveKid.FullAvatar;
    }

    public int GetInfoValForActiveKid(UserInfoList info)
    {
        return ActiveKid.GetInfoVal(info);
    }
    public void CreateEmptyUsers()
    {
#if (!UNITY_EDITOR && !DEVELOPMENT_BUILD)
        int playersCount = getPlayersCountJS();
#endif

        for (int i = 0; i < _kidsUserArr.Length; i++)
        {
            SetValsForEmptyUser(i);

#if   (!UNITY_EDITOR && !DEVELOPMENT_BUILD)

            if (playersCount > 0)
            {
                _kidsUserArr[i].loadPlayerData();
            }
            else
            {
                _kidsUserArr[i].savePlayerData();
            }
#endif

        }
    }
     
    public void SetValsForEmptyUser(int i) 
    {
        _kidsUserArr[i] = new KidUser(i);
        _kidsUserArr[i].SetInfoVal(UserInfoList.Gems, 300000000);
        

#if (!UNITY_EDITOR && !DEVELOPMENT_BUILD)
 
            _kidsUserArr[i].SetInfoVal(UserInfoList.Gems, 3000);
#endif

        _kidsUserArr[i].SetInfoVal(UserInfoList.Points, ProfileManager.FIRST_RANK_POINTS);
       //  _kidsUserArr[i].SetInfoVal(UserInfoList.Points, 80000);

        _kidsUserArr[i].Store[AvatarInfoList.ChestGM.GetHashCode(), 5] = 1;
        _kidsUserArr[i].Store[AvatarInfoList.FeetGM.GetHashCode(), 1] = 1;
        _kidsUserArr[i].Store[AvatarInfoList.LegsGm.GetHashCode(), 0] = 1;
        _kidsUserArr[i].Store[AvatarInfoList.Signates.GetHashCode(), 0] = 1;
        _kidsUserArr[i].Store[AvatarInfoList.Hats.GetHashCode(), 0] = 1;
        _kidsUserArr[i].Store[AvatarInfoList.Glasses.GetHashCode(), 0] = 1;
    }
    
    public void DeleteKidUser(int kidUserNum)
    {
        _kidsUserArr[kidUserNum].IsActive = false;
        Debug.Log("deleting user num: " + kidUserNum);
        _kidsUserArr[kidUserNum].ResetAllInfo();
        SetValsForEmptyUser(kidUserNum);
        //להוסיף איפוס נתונים

        _kidsUserArr[kidUserNum].savePlayerData();//Ishay - Save on server
        SceneManager.LoadScene(0);
    }
    public int CreateNewKidUser(string _firstName, int[] _userInfo)
    {
        for (int i = 0; i < _kidsUserArr.Length; i++)
        {
            if (!_kidsUserArr[i].IsActive)
            {
                _kidsUserArr[i].IsActive = true;
                _kidsUserArr[i].FirstName = _firstName;
                _kidsUserArr[i].FullAvatar = _userInfo;
                _kidsUserArr[i].savePlayerData(); //Ishay - Save on server
                _kidsUserArr[i].loadPlayerData();

                return i;
            }
        }
        return -1;
    }
    public bool IsKidActive(int kid)
    {
        return _kidsUserArr[kid].IsActive;
    }

    public string GetKidFirstName(int kid)
    {
        return _kidsUserArr[kid].FirstName;
    }

    public string GetActiveKidFullName()
    {
        return ActiveKid.FirstName;
    }
    public int GetNumberOfKidUsers()
    {
        int toReturn = 0;
        for (int i = 0; i < 4; i++)
        {
            if (_kidsUserArr[i].IsActive)
            {
                toReturn++;
            }
        }
        return toReturn;
    }

    public void SetShabbatPoints(int val) 
    {
        ActiveKid.ShabbatPoints = val;
        print("shabat point is updated. points now: " + GetShabbatPoints()); ;
    }


    public int GetShabbatPoints()
    {
        return ActiveKid.ShabbatPoints;
    }

    public void SetHanukkaPoints(int val)
    {
        ActiveKid.HanukkaPoints = val;
        print("Hanuka points is updated. points now: " + GetHanukkaPoints()); ;
    }


    public int GetHanukkaPoints()
    {
        return ActiveKid.HanukkaPoints;
    }


    public void SetPurimPoints(int val)
    {
        ActiveKid.HanukkaPoints = val;
        print("Hanuka points is updated. points now: " + GetHanukkaPoints()); ;
    }


    public int GetPurimPoints()
    {
        return ActiveKid.HanukkaPoints;
    }

}
