using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class QuestionEditor : EditorWindow 
{
    [MenuItem("Shredder Tools/Question Set Editor %t")]
    public static void GetWindow()
    {
        QuestionEditor editor = EditorWindow.GetWindow<QuestionEditor>("Question Set Editor");
        editor.minSize = new Vector2(1200, 1020);
        editor.maxSize = new Vector2(1200, 1020);
    }

	#region Editor
    private bool isInitialized = false;
    private List<QuestionSet>  questionSets;
    private List<QuestionData> questionsList;

	void Awake()
	{
		this.InitializeQuestionSets();
	}

    void OnGUI()
    {
        this.DrawEditor();
    }

	private void DrawEditor()
	{
		GUILayout.BeginHorizontal();

        GUILayout.BeginVertical("box", GUILayout.Width(175));
		this.DrawQuestionSets();
		GUILayout.EndVertical();

        GUILayout.BeginVertical("box", GUILayout.Width(575));
		this.DrawSelectedQuestionSet();
		GUILayout.EndVertical();

        GUILayout.BeginVertical("box", GUILayout.Width(425));
        this.DrawQuestionDataEditor();
		GUILayout.EndVertical();

		GUILayout.EndHorizontal();
	}

	private QuestionSet selectedSet;
	private Vector2 questionSetScroll;
	private void DrawQuestionSets()
	{
		this.questionSetScroll = GUILayout.BeginScrollView(this.questionSetScroll);

        GUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Set"))
		{
			QuestionSet newSet 		   = new QuestionSet();
			newSet.id				   = this.questionSets.Count + 1;
			newSet.category			   = QUESTION_CATEGORY.THREE_LETTERS;
			newSet.charactersInChoices = new char[0];
			newSet.questionSet 		   = new int[0];

			this.questionSets.Add(newSet);
		}

        if(GUILayout.Button("Save All"))
        {
            this.SaveQuestionnaire();
            this.SaveQuestionSets();
        }
        GUILayout.EndHorizontal();

		foreach(QuestionSet qSet in this.questionSets)
		{
            if(GUILayout.Button("set_" + qSet.id, (selectedSet != null && qSet == selectedSet) ? EditorStyles.boldLabel : EditorStyles.miniLabel))
			{
				selectedSet = qSet;
			}
		}

		GUILayout.EndScrollView();
	}

    private Vector2 questionSetInfoScroll;
	private void DrawSelectedQuestionSet()
	{
		if(this.selectedSet == null)
		{
			GUILayout.Label("Select a Question Set");
		}
		else
		{
			if(GUILayout.Button("Save Question Set"))
			{
				this.Save<QuestionSet>(streamingAssets + questionSetsPath + "set_" + this.selectedSet.id + ".json", this.selectedSet);
				this.SaveQuestionnaire();
			}

			this.selectedSet.id		  = EditorGUILayout.IntField("Question Set ID : ", this.selectedSet.id);
			this.selectedSet.category = (QUESTION_CATEGORY)EditorGUILayout.EnumPopup("Set Category", this.selectedSet.category);

            this.questionSetInfoScroll = GUILayout.BeginScrollView(this.questionSetInfoScroll);

			GUILayout.BeginVertical("box");
			this.DrawChoicesArray();
			GUILayout.EndVertical();

			GUILayout.BeginVertical("box");
			this.DrawQuestionIDs();
			GUILayout.EndVertical();

            GUILayout.EndScrollView();
		}
	}

	private void DrawChoicesArray()
	{
		if(this.selectedSet.charactersInChoices == null)
		{
			this.selectedSet.charactersInChoices = new char[0];
		}

		if(GUILayout.Button("Add"))
		{
			List<char> list = new List<char>(this.selectedSet.charactersInChoices);
			list.Add('a');
			this.selectedSet.charactersInChoices = list.ToArray();
			list.Clear();
			list = null;
		}

		int indexToRemove = -1;
		for(int i = 0; i < this.selectedSet.charactersInChoices.Length; i++)
		{
			GUILayout.BeginHorizontal();

			if(GUILayout.Button("X"))
			{
				indexToRemove = i;
			}

			string choice = this.selectedSet.charactersInChoices[i] + "";
			choice = EditorGUILayout.TextField("Choice " + (i + 1), choice);
			char[] c = choice.ToCharArray();
			if(c.Length > 0)
			{
				this.selectedSet.charactersInChoices[i] = c[0];
			}
			else
			{
				this.selectedSet.charactersInChoices[i] = ' ';
			}

			GUILayout.EndHorizontal();
		}

		if(indexToRemove != -1)
		{
			List<char> remove = new List<char>(this.selectedSet.charactersInChoices);
			remove.RemoveAt(indexToRemove);
			this.selectedSet.charactersInChoices = remove.ToArray();
			remove.Clear();
			remove = null;
		}
	}

	private void DrawQuestionIDs()
	{
		if(this.selectedSet.questionSet == null)
		{
			this.selectedSet.questionSet = new int[0];
		}
		
		if(GUILayout.Button("Add"))
		{
			List<int> list = new List<int>(this.selectedSet.questionSet);
			list.Add(1);
			this.selectedSet.questionSet = list.ToArray();
			list.Clear();
			list = null;
		}
		
		int indexToRemove = -1;
		for(int i = 0; i < this.selectedSet.questionSet.Length; i++)
		{
			GUILayout.BeginHorizontal();

            if(GUILayout.Button("X", GUILayout.Width(75), GUILayout.Height(75)))
			{
				indexToRemove = i;
			}
			
            GUILayout.BeginVertical("box");
			this.selectedSet.questionSet[i] = EditorGUILayout.IntField("Question id " + (i + 1), this.selectedSet.questionSet[i]);

            this.DrawQuestionData(this.GetQuestionData(this.selectedSet.questionSet[i]));
            GUILayout.EndVertical();

			GUILayout.EndHorizontal();
		}
		
		if(indexToRemove != -1)
		{
			List<int> remove = new List<int>(this.selectedSet.questionSet);
			remove.RemoveAt(indexToRemove);
			this.selectedSet.questionSet = remove.ToArray();
			remove.Clear();
			remove = null;
		}
	}

    private void DrawQuestionData(QuestionData data)
    {
        if(data == null)
        {
            GUILayout.Label("Question does not exist.");
        }
        else
        {
            GUILayout.Label("Q: " + data.question);
            GUILayout.Label("A: " + data.answer);
        }
    }

    private QuestionData GetQuestionData(int id)
    {
        bool searching = true;
        for(int forward = id, backward = id - 1; searching; forward++, backward--)
        {
            if(forward >= 0 && forward < this.questionsList.Count)
            {
                if(this.questionsList[forward].id == id)
                {
                    return this.questionsList[forward];
                }
            }

            if(backward >= 0 && backward < this.questionsList.Count)
            {
                if(this.questionsList[backward].id == id)
                {
                    return this.questionsList[backward];
                }
            }

            if((forward < 0 || forward >= this.questionsList.Count) && (backward < 0 || backward >= this.questionsList.Count))
            {
                searching = false;
            }
        }

        return null;
    }

    private Vector2 questionDataScroll;
    private QUESTION_CATEGORY categoryFilter = QUESTION_CATEGORY.THREE_LETTERS;
    private void DrawQuestionDataEditor()
    {
        this.DrawQuestionDataControl();
        this.DrawQuestionCategoryFilter();
        this.DrawQuestionDataList();
    }

    private void DrawQuestionDataControl()
    {
        GUILayout.BeginHorizontal("box");
        if(GUILayout.Button("Add a question"))
        {
            QuestionData data = new QuestionData();
            data.id           = this.questionsList.Count + 1;
            data.category     = this.categoryFilter;
            data.question     = "What is your question?";
            data.answer       = "This is the answer.";

            this.questionsList.Add(data);
        }

        if(GUILayout.Button("Save questions"))
        {
            this.SaveQuestions();
        }
        GUILayout.EndHorizontal();
    }

    private void DrawQuestionCategoryFilter()
    {
        this.categoryFilter = (QUESTION_CATEGORY)EditorGUILayout.EnumPopup("Filter Category", this.categoryFilter);
    }

    private void DrawQuestionDataList()
    {
        this.questionDataScroll = GUILayout.BeginScrollView(this.questionDataScroll);

        for(int q = 0; q < this.questionsList.Count; q++)
        {
            if(this.categoryFilter != this.questionsList[q].category)
            {
                continue;
            }

            GUILayout.BeginVertical("box");
            this.questionsList[q].id       = EditorGUILayout.IntField("Question ID: ", this.questionsList[q].id);
            this.questionsList[q].category = (QUESTION_CATEGORY)EditorGUILayout.EnumPopup("Set Category", this.questionsList[q].category);
            this.questionsList[q].question = EditorGUILayout.TextField("Question: ", this.questionsList[q].question);
            this.questionsList[q].answer   = EditorGUILayout.TextField("Answer: ",   this.questionsList[q].answer.ToLower());
            GUILayout.EndVertical();
        }

        GUILayout.EndScrollView();
    }
	#endregion

	#region Saving and Loading
    private void SaveQuestionnaire()
    {
        string[] questions = new string[this.questionSets.Count];
        for(int i = 0; i < questions.Length; i++)
        {
            questions[i] = "set_" + this.questionSets[i].id.ToString();
        }

        this.Save<string[]>(streamingAssets + questionSetsPath + questionnaire, questions);
    }

    private void SaveQuestionSets()
    {
        foreach(QuestionSet qSet in this.questionSets)
        {
            this.Save<QuestionSet>(streamingAssets + questionSetsPath + "set_" + qSet.id + ".json", qSet);
        }
    }

    private void SaveQuestions()
    {
        string[] questions = new string[this.questionsList.Count];
        for(int i = 0; i < questions.Length; i++)
        {
            questions[i] = "question_" + this.questionsList[i].id.ToString();
        }

        this.Save<string[]>(streamingAssets + questionsPath + questionsFile, questions);

        foreach(QuestionData question in this.questionsList)
        {
            this.Save<QuestionData>(streamingAssets + questionsPath + "question_" + question.id + ".json", question);
        }
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

#if UNITY_EDITOR_WIN
	private static string streamingAssets   = Application.dataPath + "\\StreamingAssets\\";
	private static string questionSetsPath  = "QuestionSets\\";
	private static string questionsPath     = "Questions\\";
	private static string questionnaire     = "questionnaire.json";
	private static string questionsFile     = "questions.json";
#else
    private static string streamingAssets   = Application.dataPath + "/StreamingAssets/";
    private static string questionSetsPath  = "QuestionSets/";
    private static string questionsPath     = "Questions/";
    private static string questionnaire     = "questionnaire.json";
    private static string questionsFile     = "questions.json";
#endif

    private void InitializeQuestionSets()
    {
        this.questionSets  = new List<QuestionSet>();
        this.questionsList = new List<QuestionData>();

        QuestionsPool.LoadList<QuestionSet>(questionSetsPath, questionnaire, ref this.questionSets);
        QuestionsPool.LoadList<QuestionData>(questionsPath,   questionsFile, ref this.questionsList);

		this.isInitialized = true;
    }
    #endregion
}