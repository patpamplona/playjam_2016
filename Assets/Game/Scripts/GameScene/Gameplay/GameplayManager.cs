using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

    public delegate void OnGameStartedDelegate();
    public event OnGameStartedDelegate OnGameStarted = null;

    public delegate void CentralizedUpdateDelegate(float deltaTime, float gameRunTime);
    public event CentralizedUpdateDelegate CentralizedUpdate = null;

    public delegate void OnPlayerExitedDelegate();
    public event OnPlayerExitedDelegate OnPlayerExited = null;

    [SerializeField] private GameObject pauseOverlay;

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

    public bool GameOver
    {
        get
        {
            return this.gameOver;
        }
    }

    void Awake()
    {
        instance = this;

        this.runTime  = 0.0f;
        this.gameOver = false;
    }

    void Start()
    {
        InfoMessenger.Reset();
    }

    void OnDestroy()
    {
        instance = null;
        this.runTime = 0.0f;
        this.gameOver = false;

        //Clear the events
        this.OnGameStarted     = null;
        this.CentralizedUpdate = null;
        this.OnPlayerExited    = null;
    }

    void Update()
    {
        if(this.gameOver || !this.gameStarted)
        {
            return;
        }

        this.runTime += Time.deltaTime;

        if(CentralizedUpdate != null)
        {
            CentralizedUpdate(Time.deltaTime, this.runTime);
        }
    }

    public void StartGame()
    {
        this.gameStarted = true;
        this.pauseOverlay.SetActive(false);
        QuestionsManager.Instance.ToggleQuestionnaire(false);

        if(this.OnGameStarted != null)
        {
            this.OnGameStarted();
        }
    }

    public void PlayerGotCaught()
    {
        this.EndGame(false);
    }

    public void PlayerExitsSafely()
    {
        if(this.OnPlayerExited != null)
        {
            this.OnPlayerExited();
        }
        else
        {
            this.EndGame(true);
        }
    }

    public void OnPlayerExitedEventsCompleted()
    {
        this.EndGame(true);
    }

    private void EndGame(bool win)
    {
        this.gameOver = true;
        InfoMessenger.SetPlayerWin(win);
        SceneManager.LoadScene("EndScene");
    }
}