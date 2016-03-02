using System;
using JsonFx.Json;

[Serializable]
[JsonName("QuestionSet")]
public class QuestionSet
{
    public int               id;
    public QUESTION_CATEGORY category;
    public char[]            charactersInChoices;
    public int[]             questionSet;
}