using UnityEngine;
using System.Collections;

public class RoomTimeline : MonoBehaviour
{
    [SerializeField] private float roomTimeLimit;
    private float roomTimer;

    private bool isRoomActive = false;
    public bool IsRoomActive
    {
        get
        {
            return this.isActiveAndEnabled;
        }
    }

    void Start()
    {
        GameplayManager.Instance.CentralizedUpdate += this.RoomUpdate;
    }

    public void EnterRoom()
    {
        this.roomTimer = 0.0f;
        this.isRoomActive = true;
    }

    public void ExitRoom()
    {
        this.isRoomActive = false;
    }

    private void RoomUpdate(float deltaTime, float gameRunTime, float timeLimit, float ratioBeforeGameOver)
    {
        if(!this.isRoomActive)
        {
            return;
        }

        roomTimer += deltaTime;

        if(roomTimer >= this.roomTimeLimit)
        {
            
        }
    }
}
