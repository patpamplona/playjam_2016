using UnityEngine;
using System.Collections;

public class ShredderView : MonoBehaviour 
{
    [SerializeField] private float maxPosition;
    private float startPosition;

    void Start () 
    {
        this.startPosition = this.transform.localPosition.x;
        GameplayManager.Instance.CentralizedUpdate += this.TimeUpdate;
	}

    private void TimeUpdate(float deltaTime, float gameRunTime, float timeLimit, float ratioBeforeGameOver)
    {
        float distanceRatio = (this.maxPosition - this.startPosition) * ratioBeforeGameOver;
        float newPosition   = this.startPosition + distanceRatio;

        Vector3 localPos = this.transform.localPosition;
        localPos.x       = newPosition;

        this.transform.localPosition = localPos;
    }
}