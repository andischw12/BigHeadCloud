using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace AvatarStuff
{
    class KidAvatarManager : MonoBehaviour
    {
        [SerializeField] public GameObject HatsPrefab;
        [SerializeField] public GameObject GlassesPrefab;
        [SerializeField] public GameObject SignatePrefab;
        //[SerializeField] public GameObject CapesPrefab;
        [SerializeField] public GameObject[] Hats;
        [SerializeField] public GameObject[] Glasses;
        [SerializeField] public GameObject[] Signates;
        //[SerializeField] GameObject[] Capes;
        [SerializeField] int[] _avatarDress = new int[Enum.GetNames(typeof(AvatarArrayEnum)).Length];
        [SerializeField] Avater_ClothesAndSkeenMaker myKidPrefabMaker;
        int CurrentSignate;

       
        /// <summary>
        /// This Methods Should be used only by the KidAvatarSelector script;
        /// </summary>
        private void Awake()
        {
            myKidPrefabMaker = GetComponent<Avater_ClothesAndSkeenMaker>();
        }

        public void SetAcceories(GameObject HatsPrefabIn, GameObject GlassesPrefabIn, GameObject SignatePrefabIn)
        {

            HatsPrefab = Instantiate(HatsPrefabIn, transform.Find("ROOT/TT/TT Pelvis/TT Spine/TT Spine1/TT Spine2/TT Neck/TT Head"));
            GlassesPrefab = Instantiate(GlassesPrefabIn, transform.Find("ROOT/TT/TT Pelvis/TT Spine/TT Spine1/TT Spine2/TT Neck/TT Head"));
            SignatePrefab = Instantiate(SignatePrefabIn, transform.Find("ROOT/TT/TT Pelvis/TT Spine/TT Spine1/TT Spine2/TT L Clavicle/TT L UpperArm/TT L Forearm/TT L Hand"));

            Glasses = new GameObject[GlassesPrefab.transform.childCount];
            for (int i = 0; i < Glasses.Length; i++)
                Glasses[i] = GlassesPrefab.transform.GetChild(i).gameObject;

            Signates = new GameObject[SignatePrefab.transform.childCount];
            for (int i = 0; i < Signates.Length; i++)
                Signates[i] = SignatePrefab.transform.GetChild(i).gameObject;

            Hats = new GameObject[HatsPrefab.transform.childCount];
            for (int i = 0; i < Hats.Length; i++)
                Hats[i] = HatsPrefab.transform.GetChild(i).gameObject;

        }

        public void SetAvatarAccessoryItem(AvatarArrayEnum item, int num)
        {

            if (item == AvatarArrayEnum.Hats)
            {
                print("Num is: " + num);
                foreach (GameObject GM in Hats)
                    GM.SetActive(false);
                Hats[num].SetActive(true);
                print("num is : " + num);
            }
            else if (item == AvatarArrayEnum.Glasses)
            {
                foreach (GameObject GM in Glasses)
                    GM.SetActive(false);
                Glasses[num].SetActive(true);
            }
            else if (item == AvatarArrayEnum.Signates)
            {
                CurrentSignate = num;
                foreach (GameObject GM in Signates)
                    GM.SetActive(false);
                Signates[num].SetActive(true);
            }
        }


        public void SetAvatarDressItem(AvatarArrayEnum item, int gmNum, int matNum)
        {
            if (item == AvatarArrayEnum.ChestGM)
                myKidPrefabMaker.SetSpecificChest(gmNum, matNum);
            else if (item == AvatarArrayEnum.LegsGm)
                myKidPrefabMaker.SetSpecificLegs(gmNum, matNum);
            else if (item == AvatarArrayEnum.FeetGM)
                myKidPrefabMaker.SetSpecificFeet(gmNum, matNum);
        }

        public int GetAvatrItem(AvatarArrayEnum item)
        {
                if (item == AvatarArrayEnum.Hats)
                    for (int i = 0; i < Hats.Length; i++) if (Hats[i].activeInHierarchy) return i;
                if (item == AvatarArrayEnum.Glasses)
                    for (int i = 0; i < Glasses.Length; i++) if (Glasses[i].activeInHierarchy) return i;
                 if (item == AvatarArrayEnum.Signates)
                return CurrentSignate;
                if (item == AvatarArrayEnum.ChestGM)
                    return myKidPrefabMaker.GetSetSpecificChestGM();
                if (item == AvatarArrayEnum.ChestMat)
                    return myKidPrefabMaker.GetSetSpecificChestMat();
                if (item == AvatarArrayEnum.LegsGm)
                    return myKidPrefabMaker.GetSetSpecificLegsGM();
                if (item == AvatarArrayEnum.LegsMat)
                    return myKidPrefabMaker.GetSetSpecificLegsMat();
                if (item == AvatarArrayEnum.FeetGM)
                    return myKidPrefabMaker.GetSetSpecificFeetGM();
                if (item == AvatarArrayEnum.FeetMat)
                    return myKidPrefabMaker.GetSetSpecificFeetMat();
                return -1;
        }


         

        





        // 




    }

}
