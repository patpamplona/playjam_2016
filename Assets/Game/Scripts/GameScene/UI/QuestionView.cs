using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestionView : MonoBehaviour 
{
    [SerializeField] private Text question;

    void Start()
    {
        this.question.text = string.Empty;
        QuestionsManager.Instance.QuestionAsked += this.QuestionChanged;
    }

    private void QuestionChanged(string q)
    {
        this.question.text = q;
    }
}
