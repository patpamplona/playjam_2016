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

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        Vector3 pos = this.transform.localPosition;
        pos.y = this.runningPosition;
        this.transform.localPosition = pos;
    }

    void OnDestroy()
    {
        _instance = null;
    }

    public void ShowPlayerToRun()
    {
        this.transform.DOLocalMoveY(runningPosition, 0.75f).SetEase(Ease.OutQuad);
    }

    public void HidePlayerToAnswer()
    {
        this.transform.DOLocalMoveY(hiddenPosition, 0.75f).SetEase(Ease.OutQuad);
    }
}