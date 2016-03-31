using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class RoomTime : MonoBehaviour 
{
    [SerializeField] private float timeForThreeLetter = 60.0f;
    [SerializeField] private float timeForFourLetter  = 45.0f;
    [SerializeField] private float timeForFiveLetter  = 30.0f;

    [SerializeField] private GameObject threeLetterWall;
    [SerializeField] private GameObject fourLetterWall;
    [SerializeField] private GameObject fiveLetterWall;

    [SerializeField] private RectTransform dog;
    [SerializeField] private RectTransform cat;

    [SerializeField] private GameObject endingFlash;

    private float timeLimit = 0.0f;
    private bool exiting = false;

    private Tweener dogTweener;
    private Tweener catTweener;

    void Awake()
    {
        this.threeLetterWall.SetActive(true);
        this.fourLetterWall.SetActive(true);
        this.fiveLetterWall.SetActive(true);

        this.endingFlash.transform.localScale = Vector3.zero;

        this.exiting = false;
        this.timeLimit = 9999.0f;
    }

    void Start()
    {
        GameplayManager.Instance.OnGameStarted     += this.OnGameStarted;
        GameplayManager.Instance.CentralizedUpdate += this.UpdateTime;
        GameplayManager.Instance.OnPlayerExited    += this.PlayExitAnimation;
        QuestionsManager.Instance.OnQuestionCategoryCompleted += OnCategoryCompleted;
    }

    private void OnGameStarted()
    {
        this.timeLimit = 0.0f;
        this.ActivateNewWall(this.timeForThreeLetter, null, this.threeLetterWall);
    }

    private void OnCategoryCompleted(QUESTION_CATEGORY category)
    {
        switch(category)
        {
            case QUESTION_CATEGORY.THREE_LETTERS : this.ActivateNewWall(timeForFourLetter, this.threeLetterWall, this.fourLetterWall); break;
            case QUESTION_CATEGORY.FOUR_LETTERS  : this.ActivateNewWall(timeForFiveLetter, this.fourLetterWall, this.fiveLetterWall);  break;
            case QUESTION_CATEGORY.FIVE_LETTERS  : this.ActivateNewWall(0.0f, this.fiveLetterWall, null); break;
        }
    }

    private void ActivateNewWall(float addlTime, GameObject wall, GameObject newWall)
    {
        this.timeLimit += addlTime;
        if(wall != null)
        {
            wall.SetActive(false);
        }

        if(this.catTweener != null)
        {
            this.catTweener.Kill();
        }

        if(this.dogTweener != null)
        {
            this.dogTweener.Kill();
        }

        if(newWall != null)
        {
            this.catTweener = this.cat.transform.DOLocalMoveX(newWall.transform.localPosition.x - cat.sizeDelta.x, PathManager.Instance.TimePerWall).SetDelay(0.75f);
            this.dogTweener = this.dog.transform.DOLocalMoveX(newWall.transform.localPosition.x - dog.sizeDelta.x, this.timeLimit);
        }
    }

    private void PlayExitAnimation()
    {
        this.exiting = true;
        this.endingFlash.transform.DOScale(Vector3.one, 2.0f).SetEase(Ease.InQuint).SetDelay(0.75f).OnComplete(
            delegate()
            {
                GameplayManager.Instance.OnPlayerExitedEventsCompleted();
            });
    }

    private void UpdateTime(float deltaTime, float gameRunTime)
    {
        if(!GameplayManager.Instance.GameStarted || GameplayManager.Instance.GameOver || this.exiting)
        {
            return;
        }

        this.timeLimit -= deltaTime;

        if(this.timeLimit <= 0.0f)
        {
            GameplayManager.Instance.PlayerGotCaught();
        }
    }
}