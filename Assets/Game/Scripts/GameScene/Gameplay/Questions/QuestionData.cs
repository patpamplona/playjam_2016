using System;
using JsonFx.Json;

[Serializable]
[JsonName("QuestionData")]
public class QuestionData
{
    public int               id;
    public QUESTION_CATEGORY category;
    public string            question;
    public string            answer;
}

public enum QUESTION_CATEGORY
{
    THREE_LETTERS = 3,
    FOUR_LETTERS  = 4,
    FIVE_LETTERS  = 5
}