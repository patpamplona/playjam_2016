using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;

public class QuestionsPool
{
    private static Dictionary<QUESTION_CATEGORY, List<QuestionSet>> questionSets; 
    private static Dictionary<int, QuestionData> questions;

    private static bool isInitialized = false;
    public static bool IsInitialized
    {
        get
        {
            return isInitialized;
        }
    }

    public static void LoadQuestions()
    {
        if(isInitialized)
        {
            return;
        }

        List<QuestionSet>  sets = new List<QuestionSet>();
        List<QuestionData> data = new List<QuestionData>();

#if UNITY_EDITOR_WIN
        LoadList<QuestionSet>("QuestionSets\\", "questionnaire.json", ref sets);
        LoadList<QuestionData>("Questions\\", "questions.json", ref data);
#else
        LoadList<QuestionSet>("QuestionSets/", "questionnaire.json", ref sets);
        LoadList<QuestionData>("Questions/", "questions.json", ref data);
#endif

        questionSets = new Dictionary<QUESTION_CATEGORY, List<QuestionSet>>();
        questions    = new Dictionary<int, QuestionData>();

        foreach(QuestionSet s in sets)
        {
            if(!questionSets.ContainsKey(s.category))
            {
                questionSets.Add(s.category, new List<QuestionSet>());
            }

            questionSets[s.category].Add(s);
        }

        foreach(QuestionData d in data)
        {
            if(!questions.ContainsKey(d.id))
            {
                questions.Add(d.id, d);
            }
        }

        isInitialized = true;
        Debug.Log("Question pool initialized");
    }

    public static QuestionSet GetRandomQuestionSet(QUESTION_CATEGORY category)
    {
        if(questionSets.ContainsKey(category))
        {
            List<QuestionSet> sets = questionSets[category];
            int index = Random.Range(0, sets.Count);

            if(index < 0 || index >= sets.Count)
            {
                return null;
            }

            return sets[index];
        }

        return null;
    }

    public static QuestionData GetQuestionWithID(int id)
    {
        if(questions.ContainsKey(id))
        {
            return questions[id];
        }

        return null;
    }

    public static void LoadList<T>(string filePath, string fName, ref List<T> list)
    {
        string fileNames = GameUtils.LoadAppFile(filePath + fName);
        fileNames = fileNames.Trim();

		JsonReader fileNamesReader = new JsonReader(fileNames);
        string[] fileNamesArr = fileNamesReader.Deserialize<string[]>();

        if(fileNamesArr != null)
        {
            foreach(string fileName in fileNamesArr)
            {
                string content = GameUtils.LoadAppFile(filePath + fileName + ".json");
                content        = content.Trim();

                JsonReader reader = new JsonReader(content);
                T dataToAdd       = reader.Deserialize<T>();

                list.Add(dataToAdd);
                reader = null;
            }
        }
    }
}