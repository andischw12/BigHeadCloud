using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

//public enum itemsCategory { Signs, Hats, Shirts, Pants, Shoes };
public enum UserArrayEnum {IsActive,Number,Gender,Gems,Points,Wins,Lose,CorrectAnswers,JockerUse,FreezeUse,_5050Use,PlayTime,GemsSpent,Rank};
public enum AvatarArrayEnum {Hats,Glasses,Signates,Capes,ChestGM,ChestMat,LegsGm,LegsMat,FeetGM,FeetMat,AvatarPrefab}


public class KidUser
{
    //Ishay:
    [DllImport("__Internal")]
    public static extern string loadDataJS(int player);
    [DllImport("__Internal")]
    public static extern void saveDataJS(string data, int player, int Points, int ShabbatPoints, int HanukkaPoints, int PurimPoints);
    [DllImport("__Internal")]
    public static extern string getLastNameJS();
    public static string sendJson = "";
    public static string getJson = "";
    public string[] StoreInfo;
    // End of Isay
    

    //user properties
    public string UserName;
    public string FirstName;
    public int[] UserGeneralInfoArr = new int[Enum.GetNames(typeof(UserArrayEnum)).Length];
    public int[] UserAvatarArr = new int[Enum.GetNames(typeof(AvatarArrayEnum)).Length];
    public int[,] UserStoreMatrix = new int[Enum.GetNames(typeof(AvatarArrayEnum)).Length,100];
    /*
    public int shabbatPoints;
    public int hanukkaPoints;
    public int purimPoints;
    */
    //functions
    public bool IsActive
    {
        get { return UserGeneralInfoArr[UserArrayEnum.IsActive.GetHashCode()] == 1; }
        set { if (value) UserGeneralInfoArr[UserArrayEnum.IsActive.GetHashCode()] = 1; else UserGeneralInfoArr[UserArrayEnum.IsActive.GetHashCode()] = 0; }
    }
    // constructor
    public KidUser(int number) 
    {
        UserGeneralInfoArr[UserArrayEnum.Number.GetHashCode()] = number;
    }
    public int GetInfoVal(UserArrayEnum info) { return UserGeneralInfoArr[info.GetHashCode()]; }
    public void SetInfoVal(UserArrayEnum info, int val) { UserGeneralInfoArr[info.GetHashCode()] = val; }

    public void ResetAllInfo() 
    {
        UserName= "";
        UserGeneralInfoArr = new int[Enum.GetNames(typeof(UserArrayEnum)).Length];
        UserAvatarArr = new int[Enum.GetNames(typeof(AvatarArrayEnum)).Length];
        UserStoreMatrix = new int[Enum.GetNames(typeof(AvatarArrayEnum)).Length, 100];
    }

    //Ishay functions: Saveing data & getting data
    public string JsonPrefer()
    {
        //Entering the data from Mystore into StoreInfo - for storage on server
        Array.Resize(ref StoreInfo, 0);
        for (int i = 0; i < UserStoreMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < UserStoreMatrix.GetLength(1); j++)
            {
                if (UserStoreMatrix[i, j] == 1)
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
            UserStoreMatrix[XItem, YItem] = 1;
            //Debug.Log(Mystore[XItem,YItem]);
           // Debug.Log(XItem.ToString() + "/" + YItem.ToString());
        }
        Array.Resize(ref StoreInfo, 0);
    }

    //Saving all data of player (by player-num) on serverש
    public void savePlayerData()
    {


#if (!UNITY_EDITOR && !DEVELOPMENT_BUILD )
        UserName = FirstName.Replace("?","").Replace(":","").Replace("(","").Replace(")","").Replace("{","").Replace("}","").Replace("[","").Replace("]","");
        int player = GetInfoVal(UserArrayEnum.Number);
        int points = GetInfoVal(UserArrayEnum.Points);
        //int shabbatPointsToSave = 0;
        sendJson = JsonPrefer();
        saveDataJS(sendJson, player,points,0, 0, 0);
#endif

    }

    //Getting all data of player (by player-num) from server
    public void loadPlayerData()
    {
#if (!UNITY_EDITOR && !DEVELOPMENT_BUILD)
 
        int player = GetInfoVal(UserArrayEnum.Number); ;
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
    public int GetStoreItemState(AvatarArrayEnum item, int collum)
    {
        int toReturn = ActiveKid.UserStoreMatrix[item.GetHashCode(), collum];
            print(toReturn);
        return toReturn;
    }
    public void SetStoreItemState(AvatarArrayEnum item, int collum, int Val)
    {
        ActiveKid.UserStoreMatrix[item.GetHashCode(), collum] = Val;
        SaveActiveKidInfo();
    }
    public void SetActiveKidInfoValue(UserArrayEnum info, int newVal)
    {
        ActiveKid.SetInfoVal(info, newVal);
        SaveActiveKidInfo();
    }
    public void SetAvatarForActiveKid(int[] arr)
    {
        ActiveKid.UserAvatarArr = arr;
        SaveActiveKidInfo();
    }
    public int[] GetAvatarForActiveKid()
    {
        return ActiveKid.UserAvatarArr;
    }

    public int GetInfoValForActiveKid(UserArrayEnum info)
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
        _kidsUserArr[i].SetInfoVal(UserArrayEnum.Gems, 300000000);
        _kidsUserArr[i].SetInfoVal(UserArrayEnum.Points, 300000000);


#if (!UNITY_EDITOR && !DEVELOPMENT_BUILD)
 
            _kidsUserArr[i].SetInfoVal(UserArrayEnum.Gems, 300000000);
             _kidsUserArr[i].SetInfoVal(UserArrayEnum.Points, ProfileManager.FIRST_RANK_POINTS);
#endif



        //  _kidsUserArr[i].SetInfoVal(UserInfoList.Points, 80000);
        /*
         _kidsUserArr[i].UserStoreMatrix[AvatarArrayEnum.ChestGM.GetHashCode(), 5] = 1;
         _kidsUserArr[i].UserStoreMatrix[AvatarArrayEnum.FeetGM.GetHashCode(), 1] = 1;
         _kidsUserArr[i].UserStoreMatrix[AvatarArrayEnum.LegsGm.GetHashCode(), 0] = 1;
         _kidsUserArr[i].UserStoreMatrix[AvatarArrayEnum.Signates.GetHashCode(), 0] = 1;
        
         _kidsUserArr[i].UserStoreMatrix[AvatarArrayEnum.Glasses.GetHashCode(), 0] = 1;
        */
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
                _kidsUserArr[i].UserName = _firstName;
                _kidsUserArr[i].UserAvatarArr = _userInfo;
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
        return _kidsUserArr[kid].UserName;
    }

    public string GetActiveKidFullName()
    {
        return ActiveKid.UserName;
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

    public bool ActiveAvatarIsGirl()
    {
        print("return number is: "+ GetAvatarForActiveKid()[AvatarArrayEnum.AvatarPrefab.GetHashCode()]);
        if (GetAvatarForActiveKid()[AvatarArrayEnum.AvatarPrefab.GetHashCode()] > 15)
            return true;
        return false;
    }

    public int GetNextFreeUserSlot() 
    {

        for (int i = 0; i < _kidsUserArr.Length; i++)
        {
            if (!_kidsUserArr[i].IsActive)
            {

                print("GetNextFreeUserSlot is returning: " + i);
                return i;
            }
        }
        return -1;

    }

    

    public void SetShabbatPoints(int val) 
    {
        //ActiveKid.shabbatPoints = val;
        print("shabat point is updated. points now: " + GetShabbatPoints()); ;
    }


    public int GetShabbatPoints()
    {
        return 0;
    }

    public void SetHanukkaPoints(int val)
    {
        //ActiveKid.hanukkaPoints = val;
        print("Hanuka points is updated. points now: " + GetHanukkaPoints()); ;
    }


    public int GetHanukkaPoints()
    {
        return 0;
    }


    public void SetPurimPoints(int val)
    {
        //ActiveKid.purimPoints = val;
        print("Hanuka points is updated. points now: " + GetPurimPoints()); ;
    }


    public int GetPurimPoints()
    {
        return 0;
    }

    

}
