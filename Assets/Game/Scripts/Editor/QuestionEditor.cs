using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class QuestionEditor : EditorWindow 
{
    [MenuItem("Shredder Tools/Question Set Editor")]
    public static void GetWindow()
    {
        QuestionEditor editor = EditorWindow.GetWindow<QuestionEditor>("Question Set Editor");
        editor.minSize = new Vector2(480, 320);
    }

    private bool isInitialized = false;
    private List<QuestionSet>  questionSets;
    private List<QuestionData> questionsList;

    void OnGUI()
    {
        if(isInitialized)
        {
            
        }
        else
        {
            this.InitializeQuestionSets();
        }
    }

    private void SaveQuestionnaire()
    {
        string[] questions = new string[this.questionSets.Count];
        for(int i = 0; i < questions.Length; i++)
        {
            questions[i] = "set_" + this.questionSets[i].id.ToString();
        }

        this.Save<string[]>(streamingAssets + questionSetsPath + questionnaire, questions);
    }

    private static void Save<T>(string file, T data)
    {
        JsonWriterSettings settings = new JsonWriterSettings();
        settings.PrettyPrint = true;
        settings.TypeHintName = "__Type";

        JsonWriter writer = new JsonWriter(file, settings);
        writer.Write(data);
        writer.TextWriter.Flush();
        writer.TextWriter.Close();

        AssetDatabase.Refresh();
    }

    private static string streamingAssets   = Application.dataPath + "/StreamingAssets/";
    private static string questionSetsPath  = "QuestionSets/";
    private static string questionsPath     = "Questions/";
    private static string questionnaire     = "questionnaire.json";
    private static string questionsFile     = "questions.json";

    private void InitializeQuestionSets()
    {
        this.questionSets  = new List<QuestionSet>();
        this.questionsList = new List<QuestionData>();

        QuestionsPool.LoadList<QuestionSet>(questionSetsPath, questionnaire, ref this.questionSets);
        QuestionsPool.LoadList<QuestionData>(questionsPath,   questionsFile, ref this.questionsList);
    }
}