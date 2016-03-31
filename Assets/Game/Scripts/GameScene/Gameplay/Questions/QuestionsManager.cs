using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class QuestionsManager : MonoBehaviour
{
    private static QuestionsManager instance = null;
    public static QuestionsManager Instance
    {
        get
        {
            return instance;
        }
    }

    public delegate void OnQuestionCategoryCompletedDelegate(QUESTION_CATEGORY category);
    public event OnQuestionCategoryCompletedDelegate OnQuestionCategoryCompleted = null;

    public delegate void QuestionAskedDelegate(string q);
    public event QuestionAskedDelegate QuestionAsked = null;

    private QuestionSet  questionSet;
    private QuestionData currentQuestion;

    [SerializeField] private int questionsPerWall = 3;

    [SerializeField] private GameObject letterButtonPrefab;
    [SerializeField] private GameObject answerButtonPrefab;

    [SerializeField] private Transform answersParent;
    [SerializeField] private Transform choicesParent;

    [SerializeField] private GameObject mainObject;
    [SerializeField] private GameObject submitButton;
    [SerializeField] private Image warningObject;

    public void ToggleQuestionnaire(bool toggle)
    {
        this.mainObject.SetActive(toggle);
        this.submitButton.SetActive(toggle);
    }

    private List<LetterButton> letterButtons;
    private List<AnswerButton> answerButtons;

    private List<int> questionsAsked;

    private int categoryLevel     = ((int)QUESTION_CATEGORY.THREE_LETTERS) - 1;
    private char[] answerInBoard;
    private int questionsAnswered = 0;

    public bool BlanksAreMaxed
    {
        get
        {
            if(this.answerInBoard == null)
            {
                return false;
            }

            for(int c = 0; c < this.answerInBoard.Length; c++)
            {
                if(char.IsWhiteSpace(this.answerInBoard[c]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    void Awake()
    {
        instance = this;

        if(!QuestionsPool.IsInitialized)
        {
            QuestionsPool.LoadQuestions();
        }

        this.letterButtons = new List<LetterButton>();
        this.answerButtons = new List<AnswerButton>();
    }

    void OnDestroy()
    {
        instance = null;
        this.letterButtons.Clear();
        this.answerButtons.Clear();
        this.OnQuestionCategoryCompleted = null;
        this.QuestionAsked = null;
    }

    public void ChangeCategory()
    {
        this.questionsAnswered = 0;
        this.categoryLevel++;

        while(this.answerButtons.Count < this.categoryLevel)
        {
            GameObject go = GameObject.Instantiate(this.answerButtonPrefab) as GameObject;
            go.transform.SetParent(this.answersParent, false);
            this.answerButtons.Add(go.GetComponent<AnswerButton>());
        }

        this.InitializeSet();
    }

    private void InitializeSet()
    {
        this.questionSet = QuestionsPool.GetRandomQuestionSet((QUESTION_CATEGORY)this.categoryLevel);
        Debug.Log("Setting question set : " + this.questionSet.id);

        if(this.questionSet == null)
        {
            Debug.LogWarning("NO SET FOUND FOR " + ((QUESTION_CATEGORY)this.categoryLevel).ToString());
            return;
        }

        while(this.letterButtons.Count < this.questionSet.charactersInChoices.Length)
        {
            GameObject go = GameObject.Instantiate(this.letterButtonPrefab) as GameObject;
            go.transform.SetParent(this.choicesParent, false);
            this.letterButtons.Add(go.GetComponent<LetterButton>());
            this.letterButtons[this.letterButtons.Count - 1].SetIndex(this.letterButtons.Count - 1);
        }

        for(int c = 0; c < this.letterButtons.Count; c++)
        {
            if(c >= this.questionSet.charactersInChoices.Length)
            {
                this.letterButtons[c].gameObject.SetActive(false);
                continue;
            }

            this.letterButtons[c].gameObject.SetActive(true);
            this.letterButtons[c].SetLetter(this.questionSet.charactersInChoices[c] + "");
        }
    }

    public void AskAQuestion()
    {
        this.ResetButtons();

        int index = Random.Range(0, this.questionSet.questionSet.Length);

        if(this.questionsAsked == null)
        {
            this.questionsAsked = new List<int>();
        }

        while(this.questionsAsked.Contains(this.questionSet.questionSet[index]))
        {
            index = Random.Range(0, this.questionSet.questionSet.Length);
        }

        this.questionsAsked.Add(this.questionSet.questionSet[index]);
        if(this.questionsAsked.Count > 3)
        {
            this.questionsAsked.RemoveAt(0);
        }

        if(index < 0 || index >= this.questionSet.questionSet.Length)
        {
            Debug.LogWarning("INDEX OUT OF BOUNDS [ " + index + " ] CANNOT GET QUESTION.");
            return;
        }

        this.currentQuestion = QuestionsPool.GetQuestionWithID(this.questionSet.questionSet[index]);
        this.answerInBoard = new char[this.currentQuestion.answer.Length];
        for(int c = 0; c < this.answerInBoard.Length; c++)
        {
            this.answerInBoard[c] = ' ';
        }

        Debug.Log("Gonna ask the question " + this.currentQuestion.question);
        if(this.QuestionAsked != null)
        {
            this.QuestionAsked(this.currentQuestion.question);
        }
    }

    private void ResetButtons()
    {
        foreach(AnswerButton answer in this.answerButtons)
        {
            answer.OnLetterTapped();
        }
    }

    public void RegisterCharacter(char charAnswer, int buttonIndex)
    {
        for(int i = 0; i < this.answerInBoard.Length; i++)
        {
            if(char.IsWhiteSpace(this.answerInBoard[i]))
            {
                this.answerInBoard[i] = charAnswer;
                this.answerButtons[i].RegisterCharacter(charAnswer, i, buttonIndex);
                break;
            }
        }
    }

    public void CheckIfAnswerIsCorrect()
    {
        if(this.BlanksAreMaxed)
        {
            char[] answer = this.currentQuestion.answer.ToCharArray();

            bool correct = true;
            for(int c = 0; c < this.answerInBoard.Length; c++)
            {
                if(this.answerInBoard[c] != answer[c])
                {
                    correct = false;
                }
            }

            if(correct)
            {
                this.questionsAnswered++;
                this.ResetButtons();

                AudioThang.Instance.PlayCorrect();

                if(this.questionsAnswered >= questionsPerWall)
                {
                    PathManager.Instance.UnlockWall();

                    if(this.OnQuestionCategoryCompleted != null)
                    {
                        this.OnQuestionCategoryCompleted((QUESTION_CATEGORY)this.categoryLevel);
                    }

                    if(this.categoryLevel == (int)QUESTION_CATEGORY.FIVE_LETTERS)
                    {
                        GameplayManager.Instance.PlayerExitsSafely();
                    }
                }
                else
                {
                    this.InitializeSet();
                    this.AskAQuestion();
                }
            }
            else
            {
                AudioThang.Instance.PlayIncorrect();
                DOTween.ToAlpha(() => this.warningObject.color, c => this.warningObject.color = c, 1.0f, 0.75f).OnComplete(
                    delegate() 
                    {
                        DOTween.ToAlpha(() => this.warningObject.color, c => this.warningObject.color = c, 0.0f, 0.75f);
                    });
            }
        }
    }

    public void ReturnLetterFromAnswer(int index, int buttonIdx)
    {
        if(index >= 0 && index < this.answerInBoard.Length)
        {
            this.answerInBoard[index] = ' ';
        }

        if(buttonIdx >= 0 && buttonIdx < this.letterButtons.Count)
        {
            this.letterButtons[buttonIdx].ReturnLetter();
        }
    }
}