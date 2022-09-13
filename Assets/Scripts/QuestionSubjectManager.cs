using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestionSubject {Random = -1,Bereshit,ShmotVaikra,BamidbarDvarim,Tanak,Avot,Israel,Mada,Tarbut,RoshHashana,Kippur,Sukkot,Shabat,Hanuka,Purim,tfila,mizvot,shirim,numbers,israelHistory,PlacesInIsrael,geography,Biology,food}

public class QuestionSubjectManager : MonoBehaviour
{
    public static QuestionSubjectManager instance;
    [SerializeField] QuestionSubject _chosen = QuestionSubject.Random;
    void Awake()
    {
        instance = this;
    }
    public void ChooseQuestionSubject(int chosen) 
    {
        if (GameManager.instance.IsNewRandomMode()&& GameProcess.instance.currentQuestionNumber>0)
            Cameras.instance.SelectCamera(Cameras.instance.CameraArray.Length - 1);

        _chosen = (QuestionSubject)chosen;
    }

    public int GetActiveQuestionSubject() 
    {
        return (int)_chosen;
    }
}
