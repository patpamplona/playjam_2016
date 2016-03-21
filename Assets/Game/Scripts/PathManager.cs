using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PathManager : MonoBehaviour
{
    private static PathManager instance = null;
    public static PathManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField] private Transform backFloor;
    [SerializeField] private Transform currFloor;
    [SerializeField] private Transform frontFloor;

    [SerializeField] private float scrollSpeed;
    [SerializeField] private float roomSize; 

    private float movement = 0.0f;

    [SerializeField] private bool isBlockedByWall = false;
    [SerializeField] private GameObject wall;

    [SerializeField] private float distancePerWall = 10.0f;
    private float distanceTravelled = 0.0f;

    public void BlockByWall()
    {
        this.isBlockedByWall = true;
        this.wall.SetActive(true);

        Vector3 local = this.wall.transform.localPosition;
        local.x = -roomSize;
        this.wall.transform.localPosition = local;

        this.wall.transform.DOLocalMoveX(0.0f, 1.0f).OnComplete(
        delegate() 
        {
            QuestionsManager.Instance.ChangeCategory();
            QuestionsManager.Instance.AskAQuestion();
            QuestionsManager.Instance.ToggleQuestionnaire(true);
        });
    }

    public void UnlockWall()
    {
        QuestionsManager.Instance.ToggleQuestionnaire(false);
        this.wall.transform.DOLocalMoveX(-roomSize, 1.0f).OnComplete(
        delegate()
        {
            this.isBlockedByWall = false;
            this.wall.SetActive(false); 
        });
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        QuestionsManager.Instance.ToggleQuestionnaire(false);
    }

    void OnDestroy()
    {
        instance = null;
    }

    void Update()
    {
        if(!GameplayManager.Instance.GameStarted || isBlockedByWall)
        {
            return;
        }

        float posDelta = scrollSpeed * Time.deltaTime;
        movement += posDelta;

        this.MoveFloor(this.backFloor,  posDelta);
        this.MoveFloor(this.currFloor,  posDelta);
        this.MoveFloor(this.frontFloor, posDelta);

        if(movement >= roomSize)
        {
            movement = 0.0f;
            Vector3 toFront = this.backFloor.transform.localPosition;
            toFront.z = roomSize * 2;
            this.backFloor.transform.localPosition = toFront;

            Transform f = this.backFloor;
            this.backFloor  = this.currFloor;
            this.currFloor  = this.frontFloor;
            this.frontFloor = f;
        }

        this.distanceTravelled += posDelta;
        if(this.distanceTravelled >= this.distancePerWall)
        {
            this.distanceTravelled = 0.0f;
            this.BlockByWall();
        }
    }

    private void MoveFloor(Transform floor, float posDelta)
    {
        Vector3 pos = floor.transform.localPosition;
        pos.z -= posDelta;
        floor.transform.localPosition = pos;
    }
}