using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance = null;
    public static PlayerController Instance
    {
        get
        {
            return _instance;
        }
    }

    [SerializeField] private float hiddenPosition;
    [SerializeField] private float runningPosition;

    [SerializeField] private Animation catRunningAnim;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        Vector3 pos = this.transform.localPosition;
        pos.y = this.runningPosition;
        this.transform.localPosition = pos;

        this.catRunningAnim.Stop();
        GameplayManager.Instance.OnGameStarted += this.BeginRunning;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    private void BeginRunning()
    {
        this.catRunningAnim.Play();
    }

    public void ShowPlayerToRun()
    {
        this.transform.DOScale(Vector3.one, 0.75f).SetEase(Ease.OutQuad);
        this.transform.DOLocalMoveY(runningPosition, 0.75f).SetEase(Ease.OutQuad).OnComplete(
            delegate() 
            {
                this.BeginRunning();
            });
    }

    public void HidePlayerToAnswer()
    {
        this.catRunningAnim.Stop();
        Vector3 zoomOut = new Vector3(2.0f, 2.0f, 2.0f);
        this.transform.DOScale(zoomOut, 0.75f).SetEase(Ease.OutQuad);
        this.transform.DOLocalMoveY(hiddenPosition, 0.75f).SetEase(Ease.OutQuad);
    }
}