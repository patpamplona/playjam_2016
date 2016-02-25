using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LetterButton : MonoBehaviour 
{
    [SerializeField] private Text letter;

    private bool isAlreadyInBoard = false;
    private int buttonIndex = -1;

    public void SetIndex(int index)
    {
        this.buttonIndex = index;
    }

    public void SetLetter(string letter)
    {
        this.letter.text = letter;
    }

    public void OnLetterClicked()
    {
        if(this.isAlreadyInBoard || !GameplayManager.Instance.GameStarted || QuestionsManager.Instance.BlanksAreMaxed)
        {
            return;
        }

        QuestionsManager.Instance.RegisterCharacter(this.letter.text.ToCharArray()[0], this.buttonIndex);
        this.letter.gameObject.SetActive(false);
        this.isAlreadyInBoard = true;
    }

    public void ReturnLetter()
    {
        this.isAlreadyInBoard = false;
        this.letter.gameObject.SetActive(true);
    }
}