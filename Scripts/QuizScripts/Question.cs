using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnswerType { Multi, Single }

[Serializable()]
public class Answer
{
    public string Info = string.Empty;
    public bool IsCorrect = false;

    public Answer() { }
}

[Serializable()]
public class Question
{
    public string Info = null;
    public Answer[] answers = null;
    public AnswerType ansType = AnswerType.Single;
    public Int32 score = 0;

    public Question() { }
    
    //return the list of correct answers
    public List<int> GetCorrectAnswers ()
    {
        List<int> correctAnswersList = new List<int>();
        for (int i = 0; i < answers.Length; i++)
        {
            if(answers[i].IsCorrect)
            {
                correctAnswersList.Add(i);
            }
        }

        return correctAnswersList;
    }
}
