using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionsData", menuName = "QuestionsData", order = 1)]
public class QuizDataScriptable : ScriptableObject
{
    public List<QuestionData> questions;
}

[System.Serializable]
public class QuestionData
{
    public string answer;
}
