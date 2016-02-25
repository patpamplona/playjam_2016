using UnityEngine;
using System.Collections;

public class GameplayManager : MonoBehaviour
{
    private static GameplayManager instance = null;
    public static GameplayManager Instance
    {
        get
        {
            return instance;
        }
    }

    public delegate void CentralizedUpdateDelegate(float deltaTime, float gameRunTime, float timeLimit, float ratioBeforeGameOver);
    public event CentralizedUpdateDelegate CentralizedUpdate = null;

    public delegate void CentralizedTimeUpDelegate();
    public event CentralizedTimeUpDelegate CentralizedTimeUp = null; 

    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private float timeLimit = 120.0f;

    private float runTime  = 0.0f;
    private bool  gameOver = false;

    private bool gameStarted = false;
    public bool GameStarted
    {
        get
        {
            return this.gameStarted && !gameOver;
        }
    }

    void Awake()
    {
        instance = this;

        this.runTime  = 0.0f;
        this.gameOver = false;
    }

    void OnDestroy()
    {
        instance = null;
    }

    void Update()
    {
        if(this.gameOver || !this.gameStarted)
        {
            return;
        }

        this.runTime += Time.deltaTime;

        bool timeUp = this.runTime >= this.timeLimit;

        if(CentralizedUpdate != null)
        {
            float ratioBeforeGameOver = this.runTime / this.timeLimit;
            CentralizedUpdate(Time.deltaTime, this.runTime, this.timeLimit, ratioBeforeGameOver);
        }

        if(timeUp)
        {
            this.gameOver = true;

            if(CentralizedTimeUp != null)
            {
                CentralizedTimeUp();
            }
        }
    }

    public void StartGame()
    {
        this.gameStarted = true;
        QuestionsManager.Instance.ChangeCategory();
        QuestionsManager.Instance.AskAQuestion();
        this.pauseOverlay.SetActive(false);
    }

    public void PlayerExitsSafely()
    {
        this.gameOver = true;
    }
}