using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionsData", menuName = "QuestionsData", order = 1)]
public class QuizDataScriptable : ScriptableObject
{
    public List<QuestionGroup> groups = new List<QuestionGroup>();
}

[System.Serializable]
public class QuestionGroup
{
    public List<QuestionAndy> questions = new List<QuestionAndy>();
}