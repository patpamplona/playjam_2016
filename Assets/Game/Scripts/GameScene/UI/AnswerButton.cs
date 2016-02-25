using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnswerButton : MonoBehaviour 
{
    [SerializeField] private Text letter;

    private int currentRegistry = -1;
    private int currentButton   = -1;

    void Start()
    {
        this.letter.text = string.Empty;
    }

    public void RegisterCharacter(char c, int index, int buttonIdx)
    {
        this.currentRegistry = index;
        this.currentButton = buttonIdx;
        this.letter.text = c + "";
    }

    public void OnLetterTapped()
    {
        if(string.IsNullOrEmpty(this.letter.text) || !GameplayManager.Instance.GameStarted)
        {
            return;
        }

        this.letter.text = string.Empty;
        QuestionsManager.Instance.ReturnLetterFromAnswer(this.currentRegistry, this.currentButton);
        this.currentRegistry = this.currentButton = -1;
    }
}