﻿using UnityEngine;
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
    [SerializeField] private GameObject endOfTunnel;

    [SerializeField] private float timePerWall = 4.0f;
    private float timeForWall = 0.0f;

    public float TimePerWall
    {
        get
        {
            return this.timePerWall;
        }
    }

    public void BlockByWall()
    {
        this.isBlockedByWall = true;
        this.wall.SetActive(true);

        Vector3 local = this.wall.transform.localPosition;
        local.x = -this.wall.transform.localScale.x;
        this.wall.transform.localPosition = local;

        this.wall.transform.DOLocalMoveX(0.0f, 1.0f).OnComplete(
        delegate() 
        {
            PlayerController.Instance.HidePlayerToAnswer();

            QuestionsManager.Instance.ChangeCategory();
            QuestionsManager.Instance.AskAQuestion();
            QuestionsManager.Instance.ToggleQuestionnaire(true);
        });
    }

    public void UnlockWall()
    {
        PlayerController.Instance.ShowPlayerToRun();

        QuestionsManager.Instance.ToggleQuestionnaire(false);
        this.wall.transform.DOLocalMoveX(-this.wall.transform.localScale.x, 1.0f).OnComplete(
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

        Vector3 scale = this.endOfTunnel.transform.localScale;
        scale.x += Time.deltaTime * 0.5f;
        scale.y += Time.deltaTime * 0.5f;
        this.endOfTunnel.transform.localScale = scale;

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

        this.timeForWall += Time.deltaTime;
        if(this.timeForWall >= this.timePerWall)
        {
            this.timeForWall = 0.0f;
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